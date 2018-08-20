using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class GunPool  {

    public string gunName;
    [SerializeField]
    private GameObject gunPrefab;
    [Range(0.1f,1.5f)]
    public float fireRate;
    [Range(1,100)]
    public int damage;
    [Range(50,100)]
    public int shootRange;
    public int clipCapacity;
    //TODO做好参数的权限控制

    public GameObject GetGunPrefab()
    {
        return gunPrefab;
    }
}
