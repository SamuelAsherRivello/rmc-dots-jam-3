using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public struct PickupSpawnerComponent : IComponentData
    {
        public readonly Entity Prefab;
        public readonly float DefaultLinearSpeed;

        public PickupSpawnerComponent(Entity prefab, float linearSpeed)
        {
            Prefab = prefab;
            DefaultLinearSpeed = linearSpeed;
        }

        public void Spawn(EntityCommandBuffer ecb, float3 newPosition)
        {
            var newPickup = ecb.Instantiate(this.Prefab);
            ecb.SetComponent<LocalTransform>(newPickup, LocalTransform.FromPosition(newPosition));
            ecb.SetComponent<PickupComponent>(newPickup, new PickupComponent(DefaultLinearSpeed));
        }
    }
}

