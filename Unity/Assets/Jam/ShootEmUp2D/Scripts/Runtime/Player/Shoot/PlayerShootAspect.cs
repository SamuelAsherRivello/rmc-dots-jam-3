﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    /// <summary>
    /// Wraps the <see cref="PlayerShootComponent"/> needed for the player to shoot via
    /// and this aspect ONLY is used by the <see cref="PlayerShootSystem"/>
    /// </summary>
    readonly partial struct PlayerShootAspect : IAspect
    {
        //  Properties ------------------------------------
        public Entity BulletPrefab => PlayerShootComponentRefRW.ValueRO.BulletPrefab;
        public float3 BulletSpeed => PlayerShootComponentRefRW.ValueRO.BulletSpeed;
        public float BulletFireRate => PlayerShootComponentRefRW.ValueRO.BulletFireRate;
        public float3 Position => LocalTransformRefRW.ValueRO.Position;
        public float3 Up => LocalTransformRefRW.ValueRO.Up();


        //  Fields ----------------------------------------
        readonly RefRW<PlayerShootComponent> PlayerShootComponentRefRW;
        readonly RefRW<LocalTransform> LocalTransformRefRW;
        
        
        //  Methods ---------------------------------------
        public bool CanShoot(float deltaTime)
        {
            PlayerShootComponentRefRW.ValueRW._CooldownTimer -= deltaTime;
            if (PlayerShootComponentRefRW.ValueRO._CooldownTimer <= 0)
            {
                return true;
            }
            return false;
        }

        public void ResetShootCooldown(float fireRate)
        {
            PlayerShootComponentRefRW.ValueRW._CooldownTimer = fireRate;
        }
    }
}