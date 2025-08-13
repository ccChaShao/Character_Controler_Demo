using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float curDistance;
    
    [Range(0.5f,3)]public float minDistance;
    [Range(3,10)] public float maxDistance;
    
    public float sensitivity;
    
    public float smoothness;
    
    private CinemachineFramingTransposer m_VirtualCamera;
    private InputService m_InputService;

    private void Awake()
    {
        m_InputService = InputService.Instance;
        m_VirtualCamera = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        m_VirtualCamera.m_CameraDistance = curDistance;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateCameraDistanceVal();
    }

    private void LateUpdate()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistanceVal()
    {
        curDistance = m_InputService.scrollVal.y * Time.deltaTime * sensitivity;
        curDistance = Mathf.Clamp(curDistance, minDistance, maxDistance);
    }
    
    private void UpdateCameraDistance()
    {
        m_VirtualCamera.m_CameraDistance = Mathf.Lerp(m_VirtualCamera.m_CameraDistance, curDistance, Time.deltaTime * smoothness);
    }
}
