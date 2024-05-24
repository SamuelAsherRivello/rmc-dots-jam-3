using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.DestroyEntity;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    /// <summary>
    /// Changes <see cref="HealthComponent"/> depending on <see cref="HealthChangeExecuteOnceComponent"/>
    /// </summary>
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial struct HealthComponentSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<HealthComponentSystemAuthoring.HealthComponentSystemIsEnabledSystem>();
            state.RequireForUpdate<HealthComponent>();
            state.RequireForUpdate<HealthChangeExecuteOnceComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (healthComponent, healthChangeExecuteOnceComponent, entity) in 
                     SystemAPI.Query<RefRW<HealthComponent>,RefRO<HealthChangeExecuteOnceComponent>>().
                         WithEntityAccess())

            {
                ecb.RemoveComponent<HealthChangeExecuteOnceComponent>(entity);
                
                healthComponent.ValueRW.HealthCurrent += healthChangeExecuteOnceComponent.ValueRO.HealthChangeBy;
                
                //TODO: Show red flicker of damage
                
                if (healthComponent.ValueRW.HealthCurrent <= 0)
                {
                    healthComponent.ValueRW.HealthCurrent = 0;
                    
                    //TODO: Show explosion
                   
                    //Remove entity
                    ecb.AddComponent<DestroyEntityComponent>(entity);
                }

            }
        }
    }
}