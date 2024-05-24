using Codice.CM.Common.Tree.Partial;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.PhysicsVelocityImpulse;
using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    readonly partial struct ShootAspect : IAspect
    {
        public float3 WeaponPosition => weaponTransform.ValueRO.Position;
        public float3 WeaponUp => weaponTransform.ValueRO.Up();

        readonly RefRW<WeaponComponent> weaponComponent;
        readonly RefRO<LocalTransform> weaponTransform;

        public bool TryShoot(ref EntityCommandBuffer ecb, TimeData time)
        {
            if (time.ElapsedTime < weaponComponent.ValueRO._NextCooldownTime)
            {
                return false;
            }

            SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 1.5f, WeaponUp * weaponComponent.ValueRO.BulletSpeed);

            weaponComponent.ValueRW._NextCooldownTime = time.ElapsedTime + weaponComponent.ValueRO.BulletFireRate;

            return true;
        }

        private void SpawnBullet(ref EntityCommandBuffer ecb, float3 position, float3 velocity)
        {
            var newBulletEntity = ecb.Instantiate(weaponComponent.ValueRO.BulletPrefab);

            ecb.SetComponent<LocalTransform>
            (
                newBulletEntity,
                LocalTransform.FromPosition(position)
            );

            var bulletForce = velocity; // For now, we're not using it as a velocity, instead it is going to be a force
            ecb.AddComponent<PhysicsVelocityImpulseComponent>
            (
                newBulletEntity,
                PhysicsVelocityImpulseComponent.FromForce(velocity)
            );
        }   
    }
}