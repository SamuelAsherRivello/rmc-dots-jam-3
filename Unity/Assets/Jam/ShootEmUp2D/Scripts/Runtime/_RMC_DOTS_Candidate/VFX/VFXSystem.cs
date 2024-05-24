using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.SystemGroups;
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
                
                Debug.Log("2. Create Explosion for VFX on FrameCount = " + Time.frameCount);
                
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
