using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public struct PickupComponent : IComponentData
    {
        public readonly float LinearSpeed;      

        public PickupComponent(float linearSpeed)
        {
            LinearSpeed = linearSpeed;
        }
    }
}