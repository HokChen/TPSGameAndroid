using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物移动状态
/// </summary>
public enum PlayerMoveState
{
    /// <summary>
    /// 奔跑
    /// </summary>
    Run,
    /// <summary>
    /// 行走
    /// </summary>
    Walk,
    Null
}

public class PlayerMovement2 : MonoBehaviour
{
    //public enum RotationAxes
    //{
    //    MouseXAndY = 0,
    //    MouseX = 1,
    //    MouseY = 2
    //}


    private CharacterController characterController;
    [HideInInspector]
    public Animator animator;
    private int WalkSpeedID = Animator.StringToHash("WalkSpeed");
    private int RunSpeedID = Animator.StringToHash("RunSpeed");
    private int ShootingID = Animator.StringToHash("Shooting");
    private int HorizontalMoveSpeedID = Animator.StringToHash("HorizontalMoveSpeed");
    private int IsReloadID = Animator.StringToHash("IsReload");
    private int IsSniperShootID = Animator.StringToHash("IsSniperShoot");


    //private float axisX = GameManager.Instance.AxisX;
    //private float axisY = GameManager.Instance.AxisY;
    //private float horizontal = GameManager.Instance.Horizontal;
    //private float vertical = GameManager.Instance.Vertical;

    private RotationAxes axes;
    private float sensitivityHor; //水平旋转速度
    private float sensitivityVert;//垂直旋转速度
    private float minimumVert;//垂直旋转最小角度
    private float maximumVert;//垂直旋转最大角度
    private float _rotationX = 0;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        axes = RotationAxes.MouseXAndY;
        sensitivityHor = GameManager.Instance.sensitivityHor;
        sensitivityVert = GameManager.Instance.sensitivityVert;
        minimumVert = GameManager.Instance.minimumVert;
        maximumVert = GameManager.Instance.maximumVert;
    }

    //private void Update()
    //{
    //    var horizontal = Input.GetAxis("Horizontal");
    //    var vertical = Input.GetAxis("Vertical");
    //    var movement = new Vector3(horizontal, 0, vertical);


    //    animator.SetFloat("Speed", vertical);

    //    transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

    //    if (vertical != 0)
    //    {
    //        //Quaternion newDirection = Quaternion.LookRotation(movement);
    //        //transform.rotation = Quaternion.Slerp(transform.rotation,newDirection,Time.deltaTime*turnSpeed);
    //        float moveSpeedToUse = (vertical > 0) ? forwardMoveRunSpeed : backwardMoveSpeed;

    //        characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);
    //    }

    //}

   /// <summary>
   /// 角色移动方法
   /// </summary>
   /// <param name="moveSpeed">移动速度</param>
   /// <param name="moveState">移动状态</param>
    public void Move(float moveSpeed,PlayerMoveState moveState)
    {
        //如果移动状态为奔跑
        if (moveState == PlayerMoveState.Run)
        {
            //将射击设置为false，奔跑时不能射击
            animator.SetBool(ShootingID, false);
            animator.SetBool(IsSniperShootID, false);
            animator.SetFloat(RunSpeedID, GameManager.Instance.Vertical);
        }
        //如果移动状态为行走
        else if (moveState == PlayerMoveState.Walk)
        {
            //将射击设置为true
            if (GameManager.Instance.CurrentGun.gunName == GameManager.Instance.gunType.sniperRifle)
            {
                animator.SetBool(IsSniperShootID, true);
            }
            else
            {
                animator.SetBool(ShootingID, true);
            }
            animator.SetFloat(WalkSpeedID, GameManager.Instance.Vertical);

        }
        animator.SetFloat(HorizontalMoveSpeedID, GameManager.Instance.Horizontal);

        //transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        Rotation();


        if (GameManager.Instance.Horizontal != 0 && GameManager.Instance.Vertical != 0)
        {
            characterController.SimpleMove(transform.right * GameManager.Instance.Horizontal * (GameManager.Instance.WalkSpeed / 2));
            characterController.SimpleMove(transform.forward * GameManager.Instance.Vertical * (moveSpeed / 2));
            return;
        }
        if (GameManager.Instance.Horizontal != 0 || GameManager.Instance.Vertical != 0)
        {
            characterController.SimpleMove(transform.right * GameManager.Instance.Horizontal * GameManager.Instance.WalkSpeed);
            characterController.SimpleMove(transform.forward * GameManager.Instance.Vertical * moveSpeed);
        }

    }

    public void Reload()
    {
        if (!animator.IsInTransition(4))
        {
            animator.SetTrigger(IsReloadID); 
        }
        
    }

    private void Rotation()
    {

        if (axes == RotationAxes.MouseX)
        {
            //水平旋转代码
            transform.Rotate(0, 0, GameManager.Instance.AxisY * sensitivityHor);
        }
        else if (axes == RotationAxes.MouseY)
        {
            //垂直旋转代码
            _rotationX -= GameManager.Instance.AxisY * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            float rotationY = transform.localEulerAngles.y;//保持y轴与原来一样
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        }
        else
        {
            //水平且垂直旋转
            _rotationX -= GameManager.Instance.AxisY * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);//限制角度大小

            float delta = GameManager.Instance.AxisX * sensitivityHor;//设置水平旋转的变化量
            float rotationY = transform.localEulerAngles.y + delta;//原来的角度加上变化量
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);//相对于全局坐标空间的角度
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Weapon")
        {
            GameManager.Instance.AddGun(other.GetComponent<PickableBox>().boxContent);
            other.gameObject.SetActive(false);
        }
    }


}
