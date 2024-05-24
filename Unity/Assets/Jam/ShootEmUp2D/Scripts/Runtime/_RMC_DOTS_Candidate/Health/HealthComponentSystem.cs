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
            
            foreach (var (healthComponentAspect, entity) in 
                     SystemAPI.Query<HealthComponentAspect>().
                         WithEntityAccess())

            {
                if (healthComponentAspect.HasHealthChangeExecuteOnceComponent())
                {
                    healthComponentAspect.ProcessHealthChangeExecuteOnceComponent();
                    healthComponentAspect.RemoveHealthChangeExecuteOnceComponent(ecb);
                    
                    if (healthComponentAspect.HealthCurrent <= healthComponentAspect.HealthMin)
                    {
                        ecb.AddComponent<DestroyEntityComponent>(entity);
                    }
                }
            }
        }
    }
}