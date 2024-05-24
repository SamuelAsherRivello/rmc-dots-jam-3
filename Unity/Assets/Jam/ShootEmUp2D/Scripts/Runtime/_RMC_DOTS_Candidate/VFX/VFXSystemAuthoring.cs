using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    public class VFXSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsSystemEnabled = true;
        
        public struct VFXSystemIsEnabledTag : IComponentData {}
        
        public class VFXSystemAuthoringBaker : Baker<VFXSystemAuthoring>
        {
            public override void Bake(VFXSystemAuthoring authoring)
            {
                if (authoring.IsSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<VFXSystemIsEnabledTag>(entity);
				}
            }
        }
    }
}

