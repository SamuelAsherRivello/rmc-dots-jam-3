using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    public class HealthComponentSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsSystemEnabled = true;
        
        public struct HealthComponentSystemIsEnabledSystem : IComponentData {}
        
        public class HealthComponentSystemAuthoringBaker : Baker<HealthComponentSystemAuthoring>
        {
            public override void Bake(HealthComponentSystemAuthoring authoring)
            {
                if (authoring.IsSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<HealthComponentSystemIsEnabledSystem>(entity);
                }
            }
        }
    }
}