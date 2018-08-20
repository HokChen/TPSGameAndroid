using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    //private Transform player;
    //private Vector3 offset;
    //private float smooth = 5;
    //private float prevY;
    ////private Transform camera;

    //public float verticalRange = 30;     
    //public Transform pTransform;


    #region

    //public enum RotationAxes
    //{
    //    MouseXAndY = 0, MouseX = 1, MouseY = 2
    //}



    ////x轴（水平）速度
    //public float sensitivityX = 15F;
    ////y轴（垂直）速度
    //public float sensitivityY = 15F;
    ////x轴（水平）最小旋转值
    //public float minimumX = -360F;
    ////x轴（水平）最大旋转值
    //public float maximumX = 360F;
    ////y轴（垂直）最小旋转值
    //public float minimumY = -60F;
    ////y轴（垂直）最大旋转值
    //public float maximumY = 60F;
    ////旋转轴
    //public RotationAxes axes = RotationAxes.MouseXAndY;
    //private float rotationY = 0F;

    #endregion




    private void Start()
    {
        ////player = GameObject.FindGameObjectWithTag("Player").transform;
        ////offset = transform.position - player.position;
        //prevY = pTransform.position.y;
        ////camera = transform.Find("PlayerCamera").GetComponent<Transform>();
    }

    private void Update()
    {
       
        

        #region
        //if (axes == RotationAxes.MouseXAndY)
        //{
        //    float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        //    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        //}
        //else if (axes == RotationAxes.MouseX)
        //{
        //    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        //}
        //else if (axes == RotationAxes.MouseY)
        //{
        //    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //    transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        //}
        #endregion



    }

    private void LateUpdate()
    {


        //SimpleFunc01();
        //NowFunc();
       
    }

    //private void SimpleFunc01()
    //{
    //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    transform.localRotation = Quaternion.LookRotation(ray.direction, Vector3.up);
    //}

    //private void NowFunc()
    //{
    //    float verMovement = Input.GetAxis("Mouse Y");
    //    Vector3 targetPosition = player.position + player.TransformDirection(offset);
    //    camera.position = Vector3.Lerp(camera.position, targetPosition, Time.deltaTime * smooth);
    //    //transform.LookAt(player.position+new Vector3(1f,2,5));
    //    //pTransform.localPosition = new Vector3(pTransform.localPosition.x, verMovement + prevY, pTransform.localPosition.z);

    //    pTransform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
    //    float targetY = verMovement + prevY;
    //    targetY = Mathf.Clamp(targetY, -2, 8);
    //    pTransform.position = new Vector3(pTransform.position.x, targetY, pTransform.position.z);

    //    camera.LookAt(pTransform.position);


    //    prevY = pTransform.position.y;

        

    //    Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
    //    Debug.DrawLine(rayOrigin, pTransform.position, Color.yellow);

    //}
}
