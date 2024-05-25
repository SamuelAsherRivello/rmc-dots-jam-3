using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Tween;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    [UpdateAfter(typeof(PlayerShootSystem))]
    public partial struct VFXSystem : ISystem
    {
        private int _tempPitchCount;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<VFXEmitRequestComponent>();
            state.RequireForUpdate<VFXSystemAuthoring.VFXSystemIsEnabledTag>();
            state.RequireForUpdate<VFXEmitterComponent>();
        }
        
		
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (vfxEmitRequestComponent, entity) 
                     in SystemAPI.Query<RefRO<VFXEmitRequestComponent>>().
                         WithEntityAccess())
            {

                var instanceEntity = ecb.Instantiate(vfxEmitRequestComponent.ValueRO.Prefab);
                
                // Move entity to initial position
                ecb.SetComponent<LocalTransform>
                (
                    instanceEntity,
                    LocalTransform.FromPosition(vfxEmitRequestComponent.ValueRO.Position)
                );
                
                //TODO: Can I pass in an array of components like this to decouple this system?
                ecb.AddComponent<TweenScaleComponent>(instanceEntity, 
                    new TweenScaleComponent(0, 1, 0.5f));
                
                ecb.RemoveComponent<VFXEmitRequestComponent>(entity);
            }
        }
    }
}
