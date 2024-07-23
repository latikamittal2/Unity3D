using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraSettings
    {
        [Header("Camera Move Settings")]
        public float zoomSpeed = 5f;
        public float moveSpeed = 5f;
        public float rotationSpeed = 5f;
        public float originalFieldOfView = 70f;
        public float zoomFieldOfView = 20f;
        public float mouseX_Sensitivity = 5f;
        public float mouseY_Sensitivity = 5f;
        public float maxClampAngle = 90;
        public float minClampAngle = -30;
    }

    [SerializeField] public CameraSettings cameraSettings;

    [System.Serializable]
    public class CameraInputSettings
    {
        public string MouseXAxis = "Mouse X";
        public string MouseYAxis = "Mouse Y";
        public string AimingInput = "Fire2";
    }

    [SerializeField] public CameraInputSettings inputSettings;

    Transform center;
    Transform target;

    Camera mainCam;
    Camera uiCam;

    float cameraXRotation = 0;
    float cameraYRotation = 0;

    private void Start()
    {
        mainCam = Camera.main;
        uiCam = mainCam.GetComponentInChildren<Camera>();
        center = transform.GetChild(0);
        FindPlayer();
    }

    private void Update()
    {
        if(!target) return;
        if (!Application.isPlaying) return;
        RotateCamera();
        zoomCamera();
    }

    private void LateUpdate()
    {
        if(target)
        {
            FollowPlayer();
        }
        else
        {
            FindPlayer();
        }
    }

    void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, target.transform.position, cameraSettings.moveSpeed * Time.deltaTime);
        transform.position = moveVector;    
    }

    void RotateCamera()
    {
        cameraXRotation += Input.GetAxis(inputSettings.MouseYAxis) * cameraSettings.mouseY_Sensitivity;
        cameraYRotation += Input.GetAxis(inputSettings.MouseXAxis) * cameraSettings.mouseX_Sensitivity;

        cameraXRotation = Mathf.Clamp(cameraXRotation, cameraSettings.minClampAngle, cameraSettings.maxClampAngle);

        cameraYRotation = Mathf.Repeat(cameraYRotation, 360);

        Vector3 rotatingAngle = new Vector3(cameraXRotation, cameraYRotation, 0);

        Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), cameraSettings.rotationSpeed * Time.deltaTime);
        center.transform.localRotation = rotation;
    }

    void zoomCamera()
    {
        if(Input.GetButton(inputSettings.AimingInput))
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.zoomFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
            uiCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.zoomFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.originalFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
            uiCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.originalFieldOfView, cameraSettings.zoomSpeed * Time.deltaTime);
        }
    }
}
