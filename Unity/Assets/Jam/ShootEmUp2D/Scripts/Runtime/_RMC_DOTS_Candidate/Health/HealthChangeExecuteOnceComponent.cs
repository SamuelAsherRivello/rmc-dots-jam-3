using Unity.Entities;

namespace RMC.DOTS.Systems.Health
{
    public struct HealthChangeExecuteOnceComponent : IComponentData
    {
        public float HealthChangeBy;

        public static HealthChangeExecuteOnceComponent FromHealthChangeBy(float healthChangeBy)
        {
            return new HealthChangeExecuteOnceComponent
            {
                HealthChangeBy = healthChangeBy,
            };
        }
    }
}