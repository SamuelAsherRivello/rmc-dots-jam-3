using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    public class FlickerSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsSystemEnabled = true;
        
        public struct FlickerSystemIsEnabledTag : IComponentData {}
        
        public class FlickerSystemAuthoringBaker : Baker<FlickerSystemAuthoring>
        {
            public override void Bake(FlickerSystemAuthoring authoring)
            {
                if (authoring.IsSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<FlickerSystemIsEnabledTag>(entity);
				}
            }
        }
    }
}

