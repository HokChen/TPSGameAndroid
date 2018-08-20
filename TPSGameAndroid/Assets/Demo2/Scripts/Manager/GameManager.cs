using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public enum RotationAxes
{
    MouseXAndY = 0,
    MouseX = 1,
    MouseY = 2
}

public struct GunType
{
    public string rifle1;
    public string rifle2;
    public string sniperRifle;
    public string machineGun;
    public string submachineGun;
    public string pistol;

}


public class GameManager : MonoBehaviour
{


    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

    }

    #region 接收输入的参数
    private float horizontal = 0;
    private float vertical = 0;
    private float axisX = 0;
    private float axisY = 0;

    public float WalkSpeed { get { return walkSpeed; } }
    public float Horizontal { get { return horizontal; } }
    public float Vertical { get { return vertical; } }
    public float AxisX { get { return axisX; } }
    public float AxisY { get { return axisY; } }
    #endregion

    [Header("玩家参数"), Space(2)]
    [SerializeField]
    private float walkSpeed = 5;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private PlayerMovement2 playerMovement2;
    [SerializeField]
    private Transform weaponContainer;
    [SerializeField]
    private float reloadTime;


    [Header("武器参数"), Space(2)]
    [SerializeField]
    private Dictionary<string, Gun> availableGuns;          //存储当前背包里可用的枪
    private Gun currentGun;
    public GunType gunType;
    [HideInInspector]
    public string gunName;
    [Range(0.1f, 1.5f)]
    public float fireRate = 0.3f;
    [Range(1, 10)]
    public int damage = 1;
    [Range(50, 100)]
    public int shootRange = 66;
    [HideInInspector]
    public int clipCapacity;
    public int totalBulletAmount;


    #region 视角旋转参数

    [Header("相机旋转参数"), Space(2)]
    public RotationAxes axes = RotationAxes.MouseXAndY;
    /// <summary>
    /// 水平旋转速度
    /// </summary>
    public float sensitivityHor = 9.0f;
    /// <summary>
    /// 垂直旋转速度
    /// </summary>
    public float sensitivityVert = 9.0f;
    /// <summary>
    /// 垂直旋转最小角度
    /// </summary>
    public float minimumVert = -45.0f;
    /// <summary>
    /// 垂直旋转最大角度
    /// </summary>
    public float maximumVert = 45.0f;
    #endregion

    [Header("资源池配置文件路径")]
    private string poolConfigPath = "Assets\\Demo2\\Scripts\\Pool\\gameobjectpool.asset";


    public string GetPoolConfigPath
    {
        get
        {
            return poolConfigPath;
        }
    }
    public Gun CurrentGun
    {
        get
        {
            return currentGun;
        }
    }



    [SerializeField]
    [Tooltip("背包每个按钮实例")]
    private Button[] knapsackButtons;


    private AnimatorIK animatorIKScript;            //控制IK动画的脚本
    private PoolManager poolManager;                //资源池管理脚本
    private GameObject knapSackUIGo;                //背包UI
    private Text bulletText;                                //子弹数量的显示

    private bool isInputable = true;                    //是否可以接收用户输入
    private bool isCanFire = true;                      //是否可以开枪
    private float isCanFireTimer = 0;                   //判断是否可以开枪的计时器

    private void Awake()
    {
        _instance = this;
        animatorIKScript = playerMovement2.gameObject.GetComponentInChildren<AnimatorIK>();
        bulletText = GameObject.Find("Canvas/PlayerStatusUI/BulletNumber/Text").GetComponent<Text>();
        poolManager = new PoolManager();
        knapSackUIGo = GameObject.Find("KnapsackUI");
        availableGuns = new Dictionary<string, Gun>();
    }

    private void Start()
    {
        InitGunTypeName();
        InitGun();
        InitKnapSackButton();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        knapSackUIGo.SetActive(false);
        UpdateBulletUI();

    }

    private void Update()
    {
        GetUserInput();
    }
    private void GetUserInput()
    {
        if (isInputable == true)
        {
            //TODO 如果更换控制方式（手机控制），只需要将horizontal与vertical修改即可
            horizontal = Input.GetAxis("Horizontal");
            axisX = Input.GetAxis("Mouse X");
            axisY = Input.GetAxis("Mouse Y");
            vertical = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();

            }

            if (isCanFire == false)
            {
                isCanFireTimer += Time.deltaTime;
                if (isCanFireTimer >= reloadTime)
                {
                    isCanFireTimer = 0;
                    isCanFire = true;
                    UpdateBulletUI();
                }
            }

            if (Input.GetButton("Fire1") && isCanFire)
            {
                UpdateBulletUI();
                if (currentGun.clipCurrentRemain <= 0)
                {
                    Reload();
                    return;
                }
                else
                {
                    
                    playerMovement2.Move(WalkSpeed, PlayerMoveState.Walk);
                    PutGunTrigger();
                }

            }
            else
            {
                playerMovement2.Move(runSpeed, PlayerMoveState.Run);
            }


        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KnapsackUIDisplayControl();
        }

        ////测试用的
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    AddGun(gunType.rifle1);
        //}

    }

    private void EnableAnimatorIK()
    {
        animatorIKScript.enabled = true;
    }

    private void PutGunTrigger()
    {
        CurrentGun.GunTrigger();
    }

    /// <summary>
    /// 初始化枪支的名字
    /// </summary>
    private void InitGunTypeName()
    {
        gunType = new GunType();
        gunType.rifle1 = "AK47";
        gunType.sniperRifle = "SSG";
        gunType.submachineGun = "AKSU";
        gunType.machineGun = "PKM";
        gunType.rifle2 = "Fal";
    }

    /// <summary>
    /// 实例化枪支
    /// <para>传入要实例化枪支的名字</para>
    /// </summary>
    /// <param name="gunName"></param>
    private void InitGun(string gunName = null)
    {
        GunPool gunPool;
        //如果当前玩家正在使用的枪支为null，说明这是游戏刚开始
        if (CurrentGun == null)
        {
            //直接给玩家一把默认枪支
            gunPool = poolManager.GetGunInformation(gunType.rifle2);
        }
        //如果玩家当前正在使用的枪不为null，说明这是捡到枪的情况
        else
        {
            //取得枪支数据
            gunPool = poolManager.GetGunInformation(gunName);
        }
        //将枪支数据赋值给GameManager，之后用到这把枪的时候，再从GameManager把数据给枪
        SetGunParameter(gunPool);
        //实例化枪支游戏对象
        GameObject gunGo = Instantiate(gunPool.GetGunPrefab(), weaponContainer);
        //如果当前玩家有正在使用的枪，则刚实例化出来的要隐藏
        if (CurrentGun != null)
        {
            gunGo.SetActive(false);
        }
        //如果当前没有使用枪，说明是游戏开始，直接让当前枪支等于刚才实例化出来的枪
        else
        {
            currentGun = gunGo.GetComponent<Gun>();
        }
        Gun gun = gunGo.GetComponent<Gun>();
        //将枪添加进背包里
        availableGuns.Add(gunPool.gunName, gun);
        //这个枪准备完毕可以开火
        gun.SetGunReadyForFire();

    }

    /// <summary>
    /// 设置枪支参数
    /// </summary>
    /// <param name="gunPool"></param>
    private void SetGunParameter(GunPool gunPool)
    {
        this.fireRate = gunPool.fireRate;
        this.shootRange = gunPool.shootRange;
        this.damage = gunPool.damage;
        this.gunName = gunPool.gunName;
        this.clipCapacity = gunPool.clipCapacity;
    }

    /// <summary>
    /// 将背包UI里面的按钮动态绑定到对应枪支信息上
    /// </summary>
    private void InitKnapSackButton()
    {
        foreach (Button button in knapsackButtons)
        {
            switch (button.gameObject.name)
            {
                case "Rifle1_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.rifle1); });
                    break;
                case "Rifle2_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.rifle2); });
                    break;
                case "MachineGun_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.machineGun); });
                    break;
                case "SubmachineGun_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.submachineGun); });
                    break;
                case "SniperGun_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.sniperRifle); });
                    break;
                case "Pistol_Slot":
                    button.onClick.AddListener(delegate { ChangeGun(gunType.pistol); });
                    break;
            }
        }
    }

    /// <summary>
    /// 向背包里添加枪支
    /// <para>传入要添加的枪支的名字</para>
    /// </summary>
    /// <param name="gunName">枪支名称</param>
    public void AddGun(string gunName)
    {
        if (availableGuns.ContainsKey(gunName))
        {
            //如果当前背包里有这把枪，不需要添加，直接返回
            return;
        }
        else
        {
            //调用实例化枪支的办法
            InitGun(gunName);
        }
    }

    /// <summary>
    /// 换枪
    /// <para>传入一个要换的枪支名字</para>
    /// </summary>
    /// <param name="gunName"></param>
    public void ChangeGun(string gunName)
    {
        //如果背包里有这把枪，直接换
        if (availableGuns.ContainsKey(gunName))
        {
            availableGuns[CurrentGun.gunName].gameObject.SetActive(false);
            availableGuns[gunName].gameObject.SetActive(true);
            currentGun = availableGuns[gunName];
        }
        else
        {
            Debug.Log("背包里没有这把枪");
            //TODO 提示用户背包里没有这把枪
        }
        KnapsackUIDisplayControl();
        UpdateBulletUI();
    }

    /// <summary>
    /// 控制背包UI显示的方法
    /// </summary>
    private void KnapsackUIDisplayControl()
    {
        //每调用一次这个方法，如果当前是可以接收用户输入，则设置为不可接收；如果是不可接收，则设置为接收
        if (isInputable) isInputable = false;
        else isInputable = true;
        //TODO显示背包界面  这个地方要用DoTween来做
        //如果当前背包已经显示了，设置为隐藏；如果不显示，设置为显示
        if (knapSackUIGo.activeInHierarchy)
        {
            knapSackUIGo.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            knapSackUIGo.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// 更新子弹数量显示
    /// </summary>
    private void UpdateBulletUI()
    {
        bulletText.text = currentGun.clipCurrentRemain.ToString() + "/" + totalBulletAmount.ToString();
    }

    /// <summary>
    /// 换弹
    /// </summary>
    private void Reload()
    {
        //当前弹夹的子弹等于弹夹容量，不需要换弹
        if (currentGun.clipCurrentRemain == currentGun.clipCapacity)
        {
            return;
        }
        else
        {
            //设置换弹的时候不可以开枪
            isCanFire = false;
            //如果背包里没有子弹了，则不做操作
            if (totalBulletAmount == 0) return;

            //如果子弹总量大于当前弹夹需要填满的量
            if (totalBulletAmount > (currentGun.clipCapacity - currentGun.clipCurrentRemain))
            {
                //从子弹总量里面取子弹填满弹夹
                totalBulletAmount = totalBulletAmount - (currentGun.clipCapacity - currentGun.clipCurrentRemain);
                currentGun.clipCurrentRemain += currentGun.clipCapacity - currentGun.clipCurrentRemain;

            }
            //如果子弹总量不足以填满当前弹夹，则有多少就填多少
            else
            {
                currentGun.clipCurrentRemain += totalBulletAmount;
                totalBulletAmount = 0;
            }
            //操作动画
            animatorIKScript.enabled = false;
            playerMovement2.Reload();
            Invoke("EnableAnimatorIK", reloadTime);
        }
    }

}
