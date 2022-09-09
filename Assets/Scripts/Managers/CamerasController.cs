using Cinemachine;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startCamera;
    [SerializeField] private CinemachineVirtualCamera shipCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera endCamera;

    private const int LOW_PRIORITY = 5;
    private const int HIGH_PRIORITY = 10;
    private CinemachineVirtualCamera _currentCamera;

    private void Awake()
    {
        startCamera.Priority = HIGH_PRIORITY;
        shipCamera.Priority = LOW_PRIORITY;
        playerCamera.Priority = LOW_PRIORITY;
        endCamera.Priority = LOW_PRIORITY;
        _currentCamera = startCamera;
    }

    private void SetCamera(CinemachineVirtualCamera virtualCamera)
    {
        _currentCamera.Priority = LOW_PRIORITY;
        _currentCamera = virtualCamera;
        _currentCamera.Priority = HIGH_PRIORITY;
    }

    public void SetStartCamera()
    {
        SetCamera(startCamera);
    }
    
    public void SetShipCamera()
    {
        SetCamera(shipCamera);
    }
    
    public void SetPlayerCamera()
    {
        SetCamera(playerCamera);
    }

    public void SetEndCamera()
    {
        SetCamera(endCamera);
    }
}