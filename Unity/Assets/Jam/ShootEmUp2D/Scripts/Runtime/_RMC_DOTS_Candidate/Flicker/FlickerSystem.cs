using RMC.DOTS.SystemGroups;
using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial struct FlickerSystem : ISystem
    {
        private int _tempPitchCount;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<FlickerSystemAuthoring.FlickerSystemIsEnabledTag>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);

            var deltaTime = Time.deltaTime;

            foreach (var (flickerRequestComponent, entity)
                     in SystemAPI.Query<RefRW<FlickerRequestComponent>>().WithEntityAccess())
            {

                var spriteRenderer = state.EntityManager.GetComponentObject<SpriteRenderer>(entity);

                // start flicker
                if (flickerRequestComponent.ValueRW._ElapsedTimeInSeconds == 0)
                {
                    flickerRequestComponent.ValueRW._FromColor = spriteRenderer.color;
                    spriteRenderer.color = flickerRequestComponent.ValueRO.ToColor;
                }

                // Wait
                flickerRequestComponent.ValueRW._ElapsedTimeInSeconds += deltaTime;
                
                // End flicker
                if (flickerRequestComponent.ValueRW._ElapsedTimeInSeconds >= 
                    flickerRequestComponent.ValueRO.DurationInSeconds)
                {
                    spriteRenderer.color = flickerRequestComponent.ValueRW._FromColor;
                    ecb.RemoveComponent<FlickerRequestComponent>(entity);
                }
            }
        }
    }
}
