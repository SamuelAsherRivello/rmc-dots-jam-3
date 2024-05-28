using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    public class CinemachineControllerSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsSystemEnabled = true;
        
        public struct CinemachineControllerSystemIsEnabled : IComponentData {}
        
        public class CinemachineControllerSystemAuthoringBaker : Baker<CinemachineControllerSystemAuthoring>
        {
            public override void Bake(CinemachineControllerSystemAuthoring authoring)
            {
                if (authoring.IsSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<CinemachineControllerSystemIsEnabled>(entity);
                }
            }
        }
    }
}