using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PoolManagerEditor
{
    [MenuItem("Manager/Create GameObjectPoolConfig")]
    static void CreateGameObjectPoolList()
    {
        GameObjectPoolList poolList = ScriptableObject.CreateInstance<GameObjectPoolList>();
        AssetDatabase.CreateAsset(poolList, "Assets\\Demo2\\Resources\\gameobjectpool.asset");
    }

}
