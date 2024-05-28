using Unity.Cinemachine;
using UnityEngine;

namespace RMC.DOTS.Systems.Cinemachines
{
    public class CinemachineController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineBrain _cinemachineBrain;
        
        [SerializeField]
        private CinemachineImpulseListener _cinemachineImpulseListener;
        
        [SerializeField]
        private CinemachineCamera _cinemachineVirtualCamera;
    }
}
