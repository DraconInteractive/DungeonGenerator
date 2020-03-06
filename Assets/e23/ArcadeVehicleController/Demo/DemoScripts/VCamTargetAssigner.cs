using UnityEngine;
using Cinemachine;

public class VCamTargetAssigner : MonoBehaviour
{
    [SerializeField] Transform target;

    private CinemachineVirtualCamera virtualCamera;

    private void Start() 
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        if (target != null)
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
    }
}