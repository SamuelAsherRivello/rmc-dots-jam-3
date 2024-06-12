using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
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
