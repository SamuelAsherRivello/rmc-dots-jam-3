using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public struct PickupDropComponent : IComponentData
    {
        public readonly Entity Prefab;   
        public readonly float LinearSpeed;      

        public PickupDropComponent(Entity prefab, float linearSpeed)
        {
            Prefab = prefab;
            LinearSpeed = linearSpeed;
        }
    }
}