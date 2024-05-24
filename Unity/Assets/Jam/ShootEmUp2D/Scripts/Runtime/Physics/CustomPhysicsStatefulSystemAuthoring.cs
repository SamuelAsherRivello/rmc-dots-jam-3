using Unity.Entities;
using UnityEngine;

namespace Unity.Physics.PhysicsStateful
{
    public class CustomPhysicsStatefulSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsEnabled = true;
        
        public struct CustomPhysicsStatefulSystemIsEnabledTag : IComponentData {}

        public class CustomPhysicsStatefulSystemBaker : Baker<CustomPhysicsStatefulSystemAuthoring>
        {
            public override void Bake(CustomPhysicsStatefulSystemAuthoring authoring)
            {
                if (authoring.IsEnabled)
                {
                    Entity inputEntity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<CustomPhysicsStatefulSystemIsEnabledTag>(inputEntity);
                }
            }
        }
    }
}
