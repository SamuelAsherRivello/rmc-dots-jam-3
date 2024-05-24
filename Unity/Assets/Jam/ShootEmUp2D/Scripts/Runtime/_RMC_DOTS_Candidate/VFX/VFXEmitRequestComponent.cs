using Unity.Entities;
using Unity.Mathematics;

namespace RMC.DOTS.Systems.VFX
{
    public struct VFXEmitRequestComponent : IComponentData
    {
        public float3 Position;
        public Entity Prefab;
        
        public static VFXEmitRequestComponent From(Entity prefab, float3 position)
        {
            return new VFXEmitRequestComponent
            {
                Prefab = prefab,
                Position = position
            };
        }
    }
}