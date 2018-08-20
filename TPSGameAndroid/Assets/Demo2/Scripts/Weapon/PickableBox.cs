using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBox : MonoBehaviour {

    public GunType gunType;
    public string boxContent;

    private void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 20);
    }
    
}
