using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    private PlayInput pi;

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float height = 1.3f;

    public float yMinLimit = -60f;
    public float yMaxLimit = 60f;

    public float distanceMin = 4f;
    public float distanceMax = 8f;

    public Vector3 addPo;

    private float mutiple = 0.03f;
    private float scaleSpeed = 200f;



    float x = 0.0f;
    float y = 0.0f;

    double killTime;

    private void Awake()
    {
        if (target)
            pi = target.GetComponent<PlayInput>();
    }



    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // 围绕着这根轴进行偏移
            Vector3 negDistance = new Vector3(addPo.x, 0.0f, -distance);
            // 先旋转再平移 (rotation * negDistance 返回的是旋转之后的位置)
            Vector3 position = rotation * negDistance + target.position + new Vector3(0, height, 0);






            transform.rotation = rotation;
            transform.position = position;


            //if (Input.GetMouseButton(1))        //右键放大
            //{
            //    target.rotation = Quaternion.Euler(0, x, 0);
            //    if (addPo.x > -1f)   addPo.x -= scaleSpeed * mutiple * Time.deltaTime;
            //    else   addPo.x = -1f;

            //    if (cam.fieldOfView > 20)  cam.fieldOfView -= scaleSpeed * Time.deltaTime;
            //    else    cam.fieldOfView = 20;

            //}
            //else
            //{
            //    if (addPo.x < 0) addPo.x += scaleSpeed * mutiple * Time.deltaTime;
            //    else addPo.x = 0;

            //    if (cam.fieldOfView < 60) cam.fieldOfView += scaleSpeed * Time.deltaTime;
            //    else cam.fieldOfView = 60;
            //}




            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                distance += 1f;
                if (distance >= distanceMax) distance = distanceMax;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                distance -= 1f;
                if (distance <= distanceMin) distance = distanceMin;

            }





            Debug.DrawLine(transform.position, target.position, Color.red);



        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


}
