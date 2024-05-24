using Unity.Entities;

namespace RMC.DOTS.Systems.Health
{
    public struct HealthComponent : IComponentData
    {
        public const float HealthMaxDefault = 100;
        public float HealthMax;
        public float HealthCurrent;

        public static HealthComponent FromDefault()
        {
            return new HealthComponent
            {
                HealthMax = HealthMaxDefault,
                HealthCurrent = HealthMaxDefault
            };
        }

        public static HealthComponent From(float healthCurrent, float healthMax)
        {
            return new HealthComponent
            {
                HealthMax = healthCurrent,
                HealthCurrent = healthMax
            };
        }
    }
}