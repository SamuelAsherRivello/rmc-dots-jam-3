using Unity.Cinemachine;
using UnityEngine;

namespace RMC.DOTS.Systems.Cinemachines
{
    public class CinemachineController : MonoBehaviour
    {

		public CinemachineCamera CinemachineCamera { get { return _cinemachineCamera; } }

		[SerializeField]
        private CinemachineBrain _cinemachineBrain;
        
        [SerializeField]
        private CinemachineImpulseListener _cinemachineImpulseListener;
        
        [SerializeField]
        private CinemachineCamera _cinemachineCamera;


	}
}
