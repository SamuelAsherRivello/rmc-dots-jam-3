using RMC.DOTS.SystemGroups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial struct VFXSystem : ISystem
    {
        private int _tempPitchCount;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<VFXEmitRequestComponent>();
            state.RequireForUpdate<VFXSystemAuthoring.VFXSystemIsEnabledTag>();
            state.RequireForUpdate<VFXEmitterComponent>();
        }
		
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.
                GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().
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
                
                ecb.RemoveComponent<VFXEmitRequestComponent>(entity);
            }
        }
    }
}
