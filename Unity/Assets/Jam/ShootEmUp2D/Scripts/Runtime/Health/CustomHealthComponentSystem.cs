using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.DestroyEntity;
using RMC.DOTS.Systems.Health;
using Unity.Burst;
using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.Health
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    [UpdateAfter(typeof(HealthComponentSystem))]
    public partial struct CustomHealthComponentSystem : ISystem
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
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (healthComponentAspect, entity) in
                     SystemAPI.Query<HealthComponentAspect>().
                         WithEntityAccess())

            {
                
                // 2. GET STATE
                if (healthComponentAspect.HasHealthChangeThisFrame())
                {
                    if (healthComponentAspect.HealthCurrent <= healthComponentAspect.HealthMin)
                    {
                        ecb.AddComponent<DestroyEntityComponent>(entity);
                    }
                }
            }
        }
    }
}
