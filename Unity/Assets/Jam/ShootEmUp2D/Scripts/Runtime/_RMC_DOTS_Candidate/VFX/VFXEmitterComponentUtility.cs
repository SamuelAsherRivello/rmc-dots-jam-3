using Unity.Entities;
using Unity.Mathematics;

namespace RMC.DOTS.Systems.VFX
{
    public static class VFXEmitterComponentUtility
    {
        public static void Emit(EntityCommandBuffer ecb, Entity prefab, float3 position)
        {
            ecb.AddComponent<VFXEmitRequestComponent>(ecb.CreateEntity(), VFXEmitRequestComponent.From(prefab, position));
        }
    }
}