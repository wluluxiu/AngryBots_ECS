using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Rendering.Universal;


public class CameraViewData
{
    public CinemachineTransposer.BindingMode m_BindingMode;
    public Vector3 m_FollowOffset;
    public float m_XDamping;
    public float m_YDamping;
    public float m_ZDamping;
}

public class CameraView : MonoBehaviour
{
    public MeshRenderer mapTrans;
    public PlayerMovementAndLook playCtrl;

    private float upperDistance = 25f;
    private Camera _camera;
    private Transform _cameraTrans;
    private float[] cameraView = new float[4]; 
    void Start()
    {
        _camera = Camera.main;
        _cameraTrans = _camera.transform;
        
        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = _camera.aspect;

        float height = upperDistance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        float leftBound = mapTrans.bounds.min.x + width;
        float rightBound = mapTrans.bounds.max.x - width;
        float bottomBound = mapTrans.bounds.min.z + height;
        float topBound = mapTrans.bounds.max.z - height;

        cameraView[0] = leftBound;
        cameraView[1] = rightBound;
        cameraView[2] = bottomBound;
        cameraView[3] = topBound;
    }


    void Update()
    {
        CheckCameraMove(playCtrl.transform.position);
        ShowDebugCamreaView();
    }


    public void CheckCameraMove(Vector3 pos)
    {
        float camX = Mathf.Clamp(pos.x,cameraView[0], cameraView[1]); 
        float camZ = Mathf.Clamp(pos.z,cameraView[2], cameraView[3]);
        
        _cameraTrans.position = new Vector3(camX, _cameraTrans.position.y, camZ);
    }

    private void ShowDebugCamreaView()
    {
        Vector3[] corners = new Vector3[4];

        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = _camera.aspect;

        float height = upperDistance * Mathf.Tan(halfFOV);
        float width = height * aspect;

        // UpperLeft
        corners[0] = _cameraTrans.position - (_cameraTrans.right * width);
        corners[0] += _cameraTrans.up * height;
        corners[0] += _cameraTrans.forward * upperDistance;

        // UpperRight
        corners[1] = _cameraTrans.position + (_cameraTrans.right * width);
        corners[1] += _cameraTrans.up * height;
        corners[1] += _cameraTrans.forward * upperDistance;

        // LowerLeft
        corners[2] = _cameraTrans.position - (_cameraTrans.right * width);
        corners[2] -= _cameraTrans.up * height;
        corners[2] += _cameraTrans.forward * upperDistance;

        // LowerRight
        corners[3] = _cameraTrans.position + (_cameraTrans.right * width);
        corners[3] -= _cameraTrans.up * height;
        corners[3] += _cameraTrans.forward * upperDistance;

        Debug.DrawLine(corners[0], corners[1], Color.yellow); // UpperLeft -> UpperRight
        Debug.DrawLine(corners[1], corners[3], Color.yellow); // UpperRight -> LowerRight
        Debug.DrawLine(corners[3], corners[2], Color.yellow); // LowerRight -> LowerLeft
        Debug.DrawLine(corners[2], corners[0], Color.yellow); // LowerLeft -> UpperLeft
    }
}