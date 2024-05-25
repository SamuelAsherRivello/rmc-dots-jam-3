using RMC.Audio.Data.Types;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.Destroyable;
using RMC.DOTS.Systems.DestroyEntity;
using RMC.DOTS.Systems.Health;
using RMC.DOTS.Systems.Player;
using RMC.DOTS.Systems.VFX;
using Unity.Burst;
using Unity.Entities;
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
        private int _tempPitchCount;
        
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
            
            //
            _tempPitchCount++;
            
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            
            ///////////////////////////////////////
            // 1. Player Detecting Other Things...
            foreach (var (dynamicBuffer, playerEntity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>>().
                         WithAll<PlayerTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulTriggerEvent statefulTriggerEvent = dynamicBuffer[bufferIndex];
                    if (statefulTriggerEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulTriggerEvent.GetOtherEntity(playerEntity);
                        
                        // Player hit Pickup
                        if (_pickupTagLookup.HasComponent(otherEntity))
                        {
                           // Debug.Log("Player Hit Pickup");
                        }
                        
                        // Player hit Enemy
                        if (_enemyTagLookup.HasComponent(otherEntity))
                        {
                           // Debug.Log("Player's plane Hit Enemy plane");
                        }
                        
                        // Player hit EnemyBullet
                        if (_enemyBulletTagLookup.HasComponent(otherEntity))
                        {
            
                            //Remove bullet
                            DestroyableEntityUtility.DestroyEntityImmediately(ecb, _destroyEntityComponentLookup, otherEntity);
                           
                        }
                        
                        // Great info via debug tooling
                        //PhysicsStatefulDebugSystem.LogEvent(ref state, entity, bufferIndex, statefulTriggerEvent);
                    }
                }
            }
            
            ///////////////////////////////////////
            // 2. Enemy Detecting Other Things...
            foreach (var (dynamicBuffer, healthComponentAspect, enemyEntity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>,HealthComponentAspect>().
                         WithAll<EnemyTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulTriggerEvent statefulTriggerEvent = dynamicBuffer[bufferIndex];
                    if (statefulTriggerEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulTriggerEvent.GetOtherEntity(enemyEntity);
                        
                        if (_playerBulletTagLookup.HasComponent(otherEntity))
                        {
                            // Play sound
                            float[] pitches = { 0.8f, 1.0f };
                            float pitch = pitches[++_tempPitchCount % 2];
                            var audioEntity = ecb.CreateEntity();
                            ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
                            (
                                "GunHit02",
                                AudioConstants.VolumeDefault,
                                pitch
                            ));
                            
                            
                            // Damage enemy
                            healthComponentAspect.HealthChangeBy(ecb,-35);

                            // Flicker Enemy Color
                            ecb.AddComponent<FlickerRequestComponent>(enemyEntity,
                                FlickerRequestComponent.FlickerRed025());

                            //Make bullet FX
                            var bulletVFX = _vfxEmitterComponentLookup.GetRefRW(otherEntity);
                            VFXEmitterComponentUtility.Emit(
                                ecb, 
                                bulletVFX.ValueRO.Prefab, 
                                _localTransformLookup.GetRefRO(otherEntity).ValueRO.Position);
                            
                            //Remove bullet
                            DestroyableEntityUtility.DestroyEntityImmediately(ecb, _destroyEntityComponentLookup, otherEntity);

                        }
                        
                    }
                }
            }
        }
    }
}
