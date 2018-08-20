using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /// <summary>
    /// 枪支名称
    /// </summary>
    [SerializeField]
    public GunType gunType;
    [HideInInspector]
    public string gunName;
    /// <summary>
    /// 开枪速率
    /// </summary>
    [SerializeField]
    private float fireRate;
    /// <summary>
    /// 伤害
    /// </summary>
    [SerializeField]
    private int damage;
    /// <summary>
    /// 射击距离
    /// </summary>
    [SerializeField]
    private int shootRange;
    public int clipCapacity;

    public int clipCurrentRemain = 0;

    private ParticleSystem fireEffect;  //开枪特效 
    private Transform firePosition;

    private float timer;
    private bool isInKnapsack = false;


    private void Awake()
    {

        //fireLine = transform.parent.Find("FireEffect").GetComponentInChildren<LineRenderer>();
    }

    private void OnEnable()
    {
        //如果这把枪不在背包里，说明刚捡上来，第一次捡进背包时需要初始化参数，以后就不需要了
        //OnEnable每次脚本所挂的物体启用时都会调用
        if (isInKnapsack==false)
        {
            InitGunParameter();
            clipCurrentRemain = clipCapacity;
            isInKnapsack = true;
        }
        
    }
    
    /// <summary>
    /// 扣动枪支扳机，根据此枪射速进行射击
    /// </summary>
    public void GunTrigger()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            timer = 0;
            FireGun();
        }


    }

    private void FireGun()
    {
        //每次开枪，当前弹夹子弹数-1
        clipCurrentRemain--;
        //播放特效
        fireEffect.transform.parent.position = firePosition.position;
        fireEffect.Play();
        //fireLine.SetPosition(0, firePosition.localPosition);
        //fireLine.enabled = true;

        //TODO 开枪声音

        //将相机的视口坐标转化成射线
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        //debug画出这条射线
        Debug.DrawRay(ray.origin, ray.direction * shootRange, Color.red, 2f);
        //Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, shootRange))
        {
            var health = hitInfo.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// 从GameManager取得枪支参数并设置给自身
    /// </summary>
    public void InitGunParameter()
    {
        this.gunType = GameManager.Instance.gunType;
        this.fireRate = GameManager.Instance.fireRate;
        this.damage = GameManager.Instance.damage;
        this.shootRange = GameManager.Instance.shootRange;
        this.gunName = GameManager.Instance.gunName;
        this.clipCapacity = GameManager.Instance.clipCapacity;
    }

    /// <summary>
    /// 设置枪支已经准备好了，可以开枪(设置开火特效位置以及开火位置)
    /// </summary>
    public void SetGunReadyForFire()
    {
        fireEffect = transform.parent.transform.Find("FireEffect").GetComponentInChildren<ParticleSystem>();
        firePosition = transform.Find("FirePoint");
    }

}
