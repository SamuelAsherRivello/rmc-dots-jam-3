using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.Destroyable;
using RMC.DOTS.Systems.DestroyEntity;
using RMC.DOTS.Systems.Health;
using RMC.DOTS.Systems.Player;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Unity.Physics.PhysicsStateful
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(StatefulTriggerEventSystem))] 
    public partial struct CustomPhysicsStatefulSystem : ISystem
    {
        // Things PLAYER might hit (Including ENEMY)
        private ComponentLookup<PickupTag> _pickupTagLookup;
        private ComponentLookup<EnemyBulletTag> _enemyBulletTagLookup;
        private ComponentLookup<EnemyTag> _enemyTagLookup;
        
        // Things ENEMY might hit (Excluding PLAYER)
        private ComponentLookup<PlayerBulletTag> _playerBulletTagLookup;
        
        // Destroy
        private ComponentLookup<DestroyEntityComponent> _destroyEntityComponentLookup;


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<CustomPhysicsStatefulSystemAuthoring.CustomPhysicsStatefulSystemIsEnabledTag>();
            state.RequireForUpdate<PhysicsStatefulSystemAuthoring.TriggerSystemIsEnabledIsEnabledTag>();

            _pickupTagLookup = state.GetComponentLookup<PickupTag>();
            _enemyBulletTagLookup = state.GetComponentLookup<EnemyBulletTag>();
            _enemyTagLookup = state.GetComponentLookup<EnemyTag>();
            _playerBulletTagLookup = state.GetComponentLookup<PlayerBulletTag>();
            _destroyEntityComponentLookup = state.GetComponentLookup<DestroyEntityComponent>();
        }

        //No burst due to use of string -- [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _pickupTagLookup.Update(ref state);
            _enemyBulletTagLookup.Update(ref state);
            _enemyTagLookup.Update(ref state);
            _playerBulletTagLookup.Update(ref state);
            _destroyEntityComponentLookup.Update(ref state);
            
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
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
                            Debug.Log("Player Hit Pickup");
                        }
                        
                        // Player hit Enemy
                        if (_enemyTagLookup.HasComponent(otherEntity))
                        {
                            Debug.Log("Player Hit Enemy");
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
                            Debug.Log("Enemy hit by bullet");
                            
                            // Play sound
                            var audioEntity = ecb.CreateEntity();
                            ecb.AddComponent<AudioComponent>(audioEntity, AudioComponent2.FromAudioClipName("Click01"));
                            
                            // Damage enemy
                            healthComponentAspect.HealthChangeBy(ecb,-35);
                          
                            //Remove bullet
                            DestroyableEntityUtility.DestroyEntityImmediately(ecb, _destroyEntityComponentLookup, otherEntity);
                            
                        }
                        
                    }
                }
            }
        }
    }
}
