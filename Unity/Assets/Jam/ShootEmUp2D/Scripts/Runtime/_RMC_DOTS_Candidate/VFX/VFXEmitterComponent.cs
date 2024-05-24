using Unity.Entities;

namespace RMC.DOTS.Systems.VFX
{
    public struct VFXEmitterComponent : IComponentData
    {
        public Entity Prefab;  

        public static VFXEmitterComponent From(Entity prefab)
        {
            return new VFXEmitterComponent
            {
                Prefab = prefab
            };
        }
    }
}
