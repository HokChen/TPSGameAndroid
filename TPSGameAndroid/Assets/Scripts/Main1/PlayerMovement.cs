using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f; //水平旋转速度
    public float sensitivityVert = 9.0f;//垂直旋转速度
    public float minimumVert = -45.0f;//垂直旋转最小角度
    public float maximumVert = 45.0f;//垂直旋转最大角度

    private float _rotationX = 0;


    float verMovement;
    float prevSpineZ;

    public float upRange = 320;
    public float downRange = 350;

    private Animator anim;
    private int speedX = Animator.StringToHash("SpeedX");
    private int speedZ = Animator.StringToHash("SpeedZ");

    public float speed = 5;


    // public Transform spine;
    private Transform pTransform;

    private CharacterController characterController;

    #region PlayerRotateVertical
    private float vertical;
    private float velcoidadeDeGiro = 20.0f;
    #endregion


    private void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //prevSpineZ = spine.localEulerAngles.z;
        //pTransform = transform.Find("P").GetComponent<Transform>();


        //vertical = spine.localEulerAngles.z;
    }

    private void Update()
    {
        PlayerMove();
        //PlayerRotateVertical();

    }

    private void LateUpdate()
    {
        //verMovement = Input.GetAxis("Mouse Y") * 10;
        //float targetZ = verMovement + prevSpineZ;
        //Debug.Log(targetZ);
        ////targetZ = Mathf.Clamp(targetZ, 0, 20);

        ////旋转腰部（处理上下视角）
        ////Vector3 offset = pTransform.position - spine.position;
        ////float targetZ = Vector3.Angle(spine.right, offset);
        //spine.localRotation = Quaternion.Euler(spine.localEulerAngles.x, spine.localEulerAngles.y, targetZ);
        //spine.LookAt(pTransform.position, -Vector3.up);
        //spine.localRotation = Quaternion.LookRotation(pTransform.forward, spine.up);

        //旋转左右视角（作人物整体旋转）
        //float y = Mathf.Clamp(-Input.GetAxis("Mouse Y") * sensitivityVert, 1, 10);
        //transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        //transform.Rotate(y,0, 0);
        //prevSpineZ = spine.localEulerAngles.z;
        PlayerRotation();

    }



    private void PlayerMove()
    {
        anim.SetFloat(speedX, Input.GetAxis("Horizontal"));
        anim.SetFloat(speedZ, Input.GetAxis("Vertical") * 2);
        characterController.SimpleMove(Input.GetAxis("Vertical") * transform.forward * speed);
        characterController.SimpleMove(Input.GetAxis("Horizontal") * transform.right * speed);
        //transform.Rotate(0, Input.GetAxis("Horizontal"), 0);   
    }

    private void PlayerRotation()
    {

        if (axes == RotationAxes.MouseX)
        {
            //水平旋转代码
            transform.Rotate(0, 0, Input.GetAxis("Mouse Y") * sensitivityHor);
        }
        else if (axes == RotationAxes.MouseY)
        {
            //垂直旋转代码
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            float rotationY = transform.localEulerAngles.y;//保持y轴与原来一样
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

        }
        else
        {
            //水平且垂直旋转
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);//限制角度大小

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;//设置水平旋转的变化量
            float rotationY = transform.localEulerAngles.y + delta;//原来的角度加上变化量
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);//相对于全局坐标空间的角度
        }

    }

    private void PlayerRotateVertical()
    {
        var mouseVertical = Input.GetAxis("Mouse Y");
        Debug.Log(mouseVertical);
        vertical = (vertical - velcoidadeDeGiro * mouseVertical) % 360f;
        vertical = Mathf.Clamp(vertical, -30, 60);
        transform.localRotation = Quaternion.AngleAxis(vertical, Vector3.right);

    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (layerIndex==1)
    //    {
    //        anim.SetLookAtPosition(Vector3.forward);
    //    }

    //}
}
