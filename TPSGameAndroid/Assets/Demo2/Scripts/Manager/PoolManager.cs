using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoolManager  {

    private Dictionary<string, GunPool> gunPoolDict;

    public PoolManager()
    {
        GameObjectPoolList poolList = Resources.Load<GameObjectPoolList>("gameobjectpool");
        gunPoolDict = new Dictionary<string, GunPool>();
        foreach (GunPool pool in poolList.gunPools)
        {
            gunPoolDict.Add(pool.gunName, pool);
        }
    }

    /// <summary>
    /// 获取枪支信息
    /// <para>返回一个GunPool类型，里面包含这个把枪的所有信息</para>
    /// </summary>
    /// <param name="gunName">枪支名字</param>
    /// <returns></returns>
    public GunPool GetGunInformation(string gunName)
    {
        GunPool gunPool;
        if (gunPoolDict.TryGetValue(gunName,out gunPool))
        {
            return gunPool;
        }
        Debug.LogWarning("The Gun:" + gunName + "is not exist!!!");
        return null;
    }
}
