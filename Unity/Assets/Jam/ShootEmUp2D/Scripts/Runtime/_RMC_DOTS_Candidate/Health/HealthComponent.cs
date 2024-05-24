using Unity.Entities;

namespace RMC.DOTS.Systems.Health
{
    public struct HealthComponent : IComponentData
    {
        public const float HealthMaxDefault = 100;
        public const float HealthMinDefault = 0;
        public float HealthMin;
        public float HealthMax;
        public float HealthCurrent;

        public static HealthComponent FromDefault()
        {
            return new HealthComponent
            {
                HealthMax = HealthMaxDefault,
                HealthMin = HealthMinDefault,
                HealthCurrent = HealthMaxDefault
            };
        }

        public static HealthComponent From(float healthMax, float healthMin, float healthCurrent)
        {
            return new HealthComponent
            {
                HealthMax = healthMax,
                HealthMin = healthMin,
                HealthCurrent = healthCurrent
            };
        }
    }
}