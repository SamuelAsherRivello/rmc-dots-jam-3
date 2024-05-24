using RMC.DOTS.Samples.Games.ShootEmUp2D;
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


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CustomPhysicsStatefulSystemAuthoring.CustomPhysicsStatefulSystemIsEnabledTag>();
            state.RequireForUpdate<PhysicsStatefulSystemAuthoring.TriggerSystemIsEnabledIsEnabledTag>();

            _pickupTagLookup = state.GetComponentLookup<PickupTag>();
            _enemyBulletTagLookup = state.GetComponentLookup<EnemyBulletTag>();
            _enemyTagLookup = state.GetComponentLookup<EnemyTag>();
            _playerBulletTagLookup = state.GetComponentLookup<PlayerBulletTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _pickupTagLookup.Update(ref state);
            _enemyBulletTagLookup.Update(ref state);
            _enemyTagLookup.Update(ref state);
            _playerBulletTagLookup.Update(ref state);
            
            ///////////////////////////////////////
            // 1. Player Detecting Other Things...
            foreach (var (dynamicBuffer, entity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>>().
                         WithAll<PlayerTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulTriggerEvent statefulTriggerEvent = dynamicBuffer[bufferIndex];
                    if (statefulTriggerEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulTriggerEvent.GetOtherEntity(entity);
                        
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
                            Debug.Log("Player Hit Enemy Bullet");
                        }
                        
                        // Great info via debug tooling
                        //PhysicsStatefulDebugSystem.LogEvent(ref state, entity, bufferIndex, statefulTriggerEvent);
                    }
                }
            }
            
            ///////////////////////////////////////
            // 2. Enemy Detecting Other Things...
            foreach (var (dynamicBuffer, entity) in 
                     SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>>().
                         WithAll<EnemyTag>().
                         WithEntityAccess())
            {
                for (int bufferIndex = 0; bufferIndex < dynamicBuffer.Length; bufferIndex++) 
                {
                    StatefulTriggerEvent statefulTriggerEvent = dynamicBuffer[bufferIndex];
                    if (statefulTriggerEvent.State == StatefulEventState.Enter)
                    {
                        var otherEntity = statefulTriggerEvent.GetOtherEntity(entity);
                        
                        if (_playerBulletTagLookup.HasComponent(otherEntity))
                        {
                            Debug.Log("Enemy hit player bullets");
                        }
                        
                        
                        // Great info via debug tooling
                        //PhysicsStatefulDebugSystem.LogEvent(ref state, entity, bufferIndex, statefulTriggerEvent);
                    }
                }
            }
        }
    }
}
