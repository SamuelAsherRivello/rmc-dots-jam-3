using Codice.CM.Common.Tree.Partial;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.PhysicsVelocityImpulse;
using RMC.DOTS.Systems.VFX;
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
        public float3 WeaponUp
        {
            get
            {
                if (weaponComponent.ValueRO.ShootDirectionIsUp)
                {
                    return weaponTransform.ValueRO.Up();
                }
                else
                {
                    return -weaponTransform.ValueRO.Up();
                }
            }
        }

        public float3 GlobalRight => weaponTransform.ValueRO.Right();
        readonly RefRW<VFXEmitterComponent> vfxEmitterComponent;
        readonly RefRW<WeaponComponent> weaponComponent;
        readonly RefRO<LocalTransform> weaponTransform;

        public void Pickup(Entity entity)
        {
            weaponComponent.ValueRW.BulletFireRate *= 0.5f;
            weaponComponent.ValueRW.BulletFireRate = math.clamp(weaponComponent.ValueRW.BulletFireRate, 0.05f, 1.0f);

            weaponComponent.ValueRW.BulletSpeed *= 1.1f;
            weaponComponent.ValueRW.BulletSpeed = math.clamp(weaponComponent.ValueRW.BulletSpeed, 1.0f, 100.0f);
        }

        public bool TryShoot(ref EntityCommandBuffer ecb, TimeData time)
        {
            if (time.ElapsedTime < weaponComponent.ValueRO._NextCooldownTime)
            {
                return false;
            }

            float3 upVelocity = WeaponUp * weaponComponent.ValueRO.BulletSpeed;


            // Shoot barrel flash
            float3 bulletPosition = WeaponPosition + WeaponUp * 1.5f;

            switch (weaponComponent.ValueRO.Type)
            {
                case WeaponType.SINGLE_SHOT:
                    SpawnBullet(ref ecb, bulletPosition, upVelocity);
                    break;

                case WeaponType.SHOTGUN_SPREAD:
                    int numberOfShotgunBullets = 5;
                    float angleStep = 180.0f / (float)(numberOfShotgunBullets - 1);

                    float s = 1;
                    if (weaponComponent.ValueRO.ShootDirectionIsUp)
                    {
                        s = -1;
                    }
                    float3 baseLeftPoint = GlobalRight * 1.0f;
                    for (int i = 0; i < numberOfShotgunBullets; i++)
                    {
                        //Rotation
                        quaternion spreadRotation = quaternion.RotateZ((float)i * -angleStep * math.TORADIANS * s);

                        //Position
                        float3 newPoint = bulletPosition + math.mul(spreadRotation, baseLeftPoint);

                        //Direction
                        float3 newDirection = math.mul(spreadRotation, baseLeftPoint);
                        newDirection *= weaponComponent.ValueRO.BulletSpeed;

                        //Spawn
                        SpawnBullet(ref ecb, newPoint, newDirection);

                    }
                    break;

                case WeaponType.TRIPLE_SHOT:
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 0.5f - GlobalRight * 1.0f, upVelocity);
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 1.5f, upVelocity);
                    SpawnBullet(ref ecb, WeaponPosition + WeaponUp * 0.5f + GlobalRight * 1.0f, upVelocity);
                    break;
            }

            weaponComponent.ValueRW._NextCooldownTime = time.ElapsedTime + weaponComponent.ValueRO.BulletFireRate;

            return true;
        }

        private void SpawnBullet(ref EntityCommandBuffer ecb, float3 position, float3 velocity)
        {
			////Muzzle Flash ---------------------------------
            SpawnObject(ref ecb, position, velocity / 10, weaponComponent.ValueRO.MuzzleFlashPrefab);

            ////Bullet ---------------------------------
            SpawnObject(ref ecb, position, velocity, weaponComponent.ValueRO.BulletPrefab);
        }


        private void SpawnObject(ref EntityCommandBuffer ecb, float3 position, float3 velocity, Entity entityPrefab)
        {
            //Muzzle Flash ---------------------------------
            var entity = ecb.Instantiate(entityPrefab);

            ecb.SetComponent<LocalTransform>
            (
                entity,
                LocalTransform.FromPosition(position)
            );

            ecb.AddComponent<PhysicsVelocityImpulseComponent>
            (
                entity,
                PhysicsVelocityImpulseComponent.FromForce(velocity)
            );
        }
    }
}