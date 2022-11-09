using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public enum CAMERA_MODE
    {
        NORMAL,
        FIXED,
        LATE
    }

    [SerializeField] private float speed = 1.0f;

    [SerializeField] private CAMERA_MODE camera_mode = CAMERA_MODE.NORMAL;
    #endregion

    #region PRIVATE_FIELDS
    Camera cam;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        cam = Camera.main;
    }
    private void LateUpdate()
    {
        if (camera_mode == CAMERA_MODE.LATE)
        {
            UpdateInputsCamera();
        }
    }
    private void Update()
    {
        if (camera_mode == CAMERA_MODE.NORMAL)
        {
            UpdateInputsCamera();
        }
    }
    private void FixedUpdate()
    {
        if (camera_mode == CAMERA_MODE.FIXED)
        {
            UpdateInputsCamera();
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void UpdateInputsCamera()
    {
        float xvalue = Input.GetAxis("Horizontal");
        float zvalue = Input.GetAxis("Depth");
        float yvalue = Input.GetAxis("Vertical");

        
        if (cam.orthographic)
        {
            cam.orthographicSize -= zvalue;
            Vector3 newMove = new Vector3(xvalue, yvalue, 0);
            transform.position = newMove * speed * Time.deltaTime* cam.orthographicSize + transform.position;
        }
        else
        {
            Vector3 newMove = new Vector3(xvalue, yvalue, zvalue);
            transform.position = newMove*speed*Time.deltaTime + transform.position;
        }
    }

    #endregion

}
