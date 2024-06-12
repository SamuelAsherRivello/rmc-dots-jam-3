using RMC.Audio.Data.Types;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.Destroyable;
using RMC.DOTS.Systems.DestroyEntity;
using RMC.DOTS.Systems.Health;
using RMC.DOTS.Systems.Player;
using RMC.DOTS.Systems.Tween;
using RMC.DOTS.Systems.VFX;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.PhysicsStateful;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(StatefulTriggerEventSystem))] 
    public partial struct CustomPhysicsStatefulSystem : ISystem
    {
        //TODO: Make a better way to 'have any system increment a counter' (Put in RandomComponent?)
        private int _enemyBulletGunHitPitchCounter;
        private int _playerBulletGunHitPitchCounter;
        private int _pickupHitPitchCounter;
        
        
        
        // Things PLAYER might hit (Including ENEMY)
        private ComponentLookup<PickupTag> _pickupTagLookup;
        private ComponentLookup<EnemyBulletTag> _enemyBulletTagLookup;
        private ComponentLookup<EnemyTag> _enemyTagLookup;
        
        // Things ENEMY might hit (Excluding PLAYER)
        private ComponentLookup<PlayerBulletTag> _playerBulletTagLookup;
        
        // Destroy
        private ComponentLookup<DestroyEntityComponent> _destroyEntityComponentLookup;
        private ComponentLookup<VFXEmitterComponent> _vfxEmitterComponentLookup;
        private ComponentLookup<LocalTransform> _localTransformLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<CustomPhysicsStatefulSystemAuthoring.CustomPhysicsStatefulSystemIsEnabledTag>();
            state.RequireForUpdate<PhysicsStatefulSystemAuthoring.TriggerSystemIsEnabledIsEnabledTag>();

            _pickupTagLookup = state.GetComponentLookup<PickupTag>();
            _enemyBulletTagLookup = state.GetComponentLookup<EnemyBulletTag>();
            _enemyTagLookup = state.GetComponentLookup<EnemyTag>();
            _playerBulletTagLookup = state.GetComponentLookup<PlayerBulletTag>();
            _destroyEntityComponentLookup = state.GetComponentLookup<DestroyEntityComponent>();
            _vfxEmitterComponentLookup = state.GetComponentLookup<VFXEmitterComponent>();
            _localTransformLookup = state.GetComponentLookup<LocalTransform>();

        }
        

        //No burst due to use of string -- [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _pickupTagLookup.Update(ref state);
            _enemyBulletTagLookup.Update(ref state);
            _enemyTagLookup.Update(ref state);
            _playerBulletTagLookup.Update(ref state);
            _destroyEntityComponentLookup.Update(ref state);
            _vfxEmitterComponentLookup.Update(ref state);
            _localTransformLookup.Update(ref state);
            
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            
            ///////////////////////////////////////
            // 1. Player Detecting Other Things...
            foreach (var (dynamicBuffer, healthComponentAspect, shootAspect, playerEntity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulCollisionEvent>, HealthComponentAspect, ShootAspect> ().
                         WithAll<PlayerTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulCollisionEvent statefulEvent = dynamicBuffer[bufferIndex];
                    if (statefulEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulEvent.GetOtherEntity(playerEntity);
                        
                        // Player hit Enemy
                        if (_enemyTagLookup.HasComponent(otherEntity))
                        {
                            // Debug.Log("Player's plane Hit Enemy plane");
                        }
                        
                        
                        // Player hit Pickup
                        if (_pickupTagLookup.HasComponent(otherEntity))
                        {
                            
                            ///////////////////////////////////
                            // HIT BY PICKUP
                            ///////////////////////////////////
                            shootAspect.Pickup(otherEntity);
                            
                            // Play sound
                            float pitch = ShootEmUp2DConstants.PickupHitPitches[++_pickupHitPitchCounter % ShootEmUp2DConstants.PickupHitPitches.Length];
                            var audioEntity = ecb.CreateEntity();
                            ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
                            (
                                ShootEmUp2DConstants.Pickup03,
                                AudioConstants.VolumeDefault + 0.5f, // This is louder
                                pitch
                            ));
                            
                            // Scale Up Pickup
                            float scaleUpPickupDuration = ShootEmUp2DConstants.ScaleUpDuration;
                            ecb.RemoveComponent<PhysicsCollider>(otherEntity);
                            ecb.AddComponent<TweenScaleComponent>(otherEntity, new TweenScaleComponent(1, 2, scaleUpPickupDuration));
                            DestroyableEntityUtility.DestroyEntity(ecb, _destroyEntityComponentLookup, scaleUpPickupDuration, otherEntity);
                        }
                        
                        
                        // Player hit EnemyBullet
                        if (_enemyBulletTagLookup.HasComponent(otherEntity))
                        {

							///////////////////////////////////
							// HIT BY ENEMY BULLET
							///////////////////////////////////
							// Take Damage
							healthComponentAspect.HealthChangeBy(ecb, -3.5f); //about 30 bullets = death

							// Flicker Enemy Color
							//ecb.AddComponent<FlickerRequestComponent>(playerEntity,       //SamR will add this soon
							//	FlickerRequestComponent.FlickerBlue025());

							// Make Bullet Decal
							var bulletVFX = _vfxEmitterComponentLookup.GetRefRW(otherEntity);
							VFXEmitterComponentUtility.Emit(
								ecb,
								bulletVFX.ValueRO.Prefab,
								_localTransformLookup.GetRefRO(otherEntity).ValueRO.Position);

							// Scale Down Bullet
                            float scaleDownBulletDuration = ShootEmUp2DConstants.ScaleDownDuration;
							ecb.RemoveComponent<PhysicsCollider>(otherEntity);
							ecb.AddComponent<TweenScaleComponent>(otherEntity, new TweenScaleComponent(1, 0.1f, scaleDownBulletDuration));
							DestroyableEntityUtility.DestroyEntity(ecb, _destroyEntityComponentLookup, scaleDownBulletDuration, otherEntity);

							// Play sound
							float pitch = ShootEmUp2DConstants.EnemyBulletGunHitPitches[++_enemyBulletGunHitPitchCounter % ShootEmUp2DConstants.EnemyBulletGunHitPitches.Length];
							var audioEntity = ecb.CreateEntity();
							ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
							(
                                ShootEmUp2DConstants.GunHit03,
								AudioConstants.VolumeDefault,
								pitch
							));
						}
                        
                        // Great info via debug tooling
                        //PhysicsStatefulDebugSystem.LogEvent(ref state, playerEntity, bufferIndex, statefulEvent);
                    }
                }
            }
            
            ///////////////////////////////////////
            // 2. Enemy Detecting Other Things...
            foreach (var (dynamicBuffer, healthComponentAspect, enemyEntity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulCollisionEvent>,HealthComponentAspect>().
                         WithAll<EnemyTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulCollisionEvent statefulEvent = dynamicBuffer[bufferIndex];
                    if (statefulEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulEvent.GetOtherEntity(enemyEntity);
                        
                        if (_playerBulletTagLookup.HasComponent(otherEntity))
                        {
                            // Play sound
                            float pitch = ShootEmUp2DConstants.PlayerBulletGunHitPitches[++_playerBulletGunHitPitchCounter % ShootEmUp2DConstants.PlayerBulletGunHitPitches.Length];
                            var audioEntity = ecb.CreateEntity();
                            ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
                            (
                                ShootEmUp2DConstants.GunHit03,
                                AudioConstants.VolumeDefault,
                                pitch
                            ));
                            
                            ///////////////////////////////////
                            // HIT BY PLAYER BULLET
                            ///////////////////////////////////
                            // Take Damage
                            healthComponentAspect.HealthChangeBy(ecb,-35); //about 3 bullets = death

                            // Flicker Enemy Color
                            ecb.AddComponent<FlickerRequestComponent>(enemyEntity,
                                FlickerRequestComponent.FlickerRed025());

                            // Make Bullet Decal
                            var bulletVFX = _vfxEmitterComponentLookup.GetRefRW(otherEntity);
                            VFXEmitterComponentUtility.Emit(
                                ecb, 
                                bulletVFX.ValueRO.Prefab, 
                                _localTransformLookup.GetRefRO(otherEntity).ValueRO.Position);
                            
                            // Scale Down Bullet
                            float scaleDownBulletDuration = ShootEmUp2DConstants.ScaleDownDuration;
                            ecb.RemoveComponent<PhysicsCollider>(otherEntity);
                            ecb.AddComponent<TweenScaleComponent>(otherEntity, new TweenScaleComponent(1, 0.1f, scaleDownBulletDuration)); 
                            DestroyableEntityUtility.DestroyEntity(ecb, _destroyEntityComponentLookup, scaleDownBulletDuration, otherEntity);

						}
                        
                        // Great info via debug tooling
                        //PhysicsStatefulDebugSystem.LogEvent(ref state, enemyEntity, bufferIndex, statefulEvent);
                    }
                }
            }
        }
    }
}
