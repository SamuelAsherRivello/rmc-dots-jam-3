using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.DestroyEntity;
using RMC.DOTS.Systems.Health;
using RMC.DOTS.Systems.VFX;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    /// <summary>
    /// SAMR decided to make this separate than the in-line <see cref="CustomPhysicsStatefulSystem"/>
    /// </summary>
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    [UpdateAfter(typeof(HealthComponentSystem))]
    public partial struct CustomHealthComponentSystem : ISystem
    {
        private ComponentLookup<VFXEmitterComponent> _vfxEmitterComponentLookup;
        private ComponentLookup<LocalTransform> _localTransformComponentLookup;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<HealthComponentSystemAuthoring.HealthComponentSystemIsEnabledSystem>();
            state.RequireForUpdate<HealthComponent>();
            state.RequireForUpdate<HealthChangeExecuteOnceComponent>();

            _vfxEmitterComponentLookup = state.GetComponentLookup<VFXEmitterComponent>();
            _localTransformComponentLookup = state.GetComponentLookup<LocalTransform>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _vfxEmitterComponentLookup.Update(ref state);
            _localTransformComponentLookup.Update(ref state);
            
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
                        // Remove Enemy
                        ecb.AddComponent<DestroyEntityComponent>(entity);
                                
                        //Make Enemy FX
                        var enemyVFX = _vfxEmitterComponentLookup.GetRefRW(entity);
                        VFXEmitterComponentUtility.Emit(
                            ecb, 
                            enemyVFX.ValueRO.Prefab, 
                            _localTransformComponentLookup.GetRefRO(entity).ValueRO.Position);
                    }
                }
            }
        }
    }
}
