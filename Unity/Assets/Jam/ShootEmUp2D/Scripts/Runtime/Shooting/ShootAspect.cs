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
        public float3 WeaponRight => weaponTransform.ValueRO.Right();

        readonly RefRW<WeaponComponent> weaponComponent;
        readonly RefRO<LocalTransform> weaponTransform;

        public bool TryShoot(ref EntityCommandBuffer ecb, TimeData time)
        {
            if (time.ElapsedTime < weaponComponent.ValueRO._NextCooldownTime)
            {
                return false;
            }

            float3 upVelocity = WeaponUp * weaponComponent.ValueRO.BulletSpeed;
            switch (weaponComponent.ValueRO.Type)
            {
                case WeaponType.SINGLE_SHOT:
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 1.5f, upVelocity);
                    break;

                case WeaponType.SHOTGUN_SPREAD:
                    int numberOfShotgunBullets = 5;
                    float angleStep = 180.0f / (float) (numberOfShotgunBullets - 1);
                    float3 baseLeftPoint = -WeaponRight * 1.0f;
                    for (int i = 0; i < numberOfShotgunBullets; i++)
                    {
                        quaternion spreadRotation = quaternion.RotateZ((float) i * -angleStep * math.TORADIANS);
                        float3 newPoint = math.mul(spreadRotation, baseLeftPoint);
                        SpawnBullet(ref ecb, WeaponPosition + newPoint, newPoint * weaponComponent.ValueRO.BulletSpeed);
                    }
                    break;

                case WeaponType.TRIPLE_SHOT:
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 0.5f - WeaponRight * 1.0f, upVelocity);
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 1.5f, upVelocity);
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 0.5f + WeaponRight * 1.0f, upVelocity);
                    break;
            }

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