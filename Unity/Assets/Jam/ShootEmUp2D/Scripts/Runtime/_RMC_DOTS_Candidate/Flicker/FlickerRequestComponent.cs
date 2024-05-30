using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.VFX
{
    public struct FlickerRequestComponent : IComponentData
    {
        public Color ToColor;
        public float DurationInSeconds;
        
        //set in system
        public Color _FromColor;
        public float _ElapsedTimeInSeconds;

        public static FlickerRequestComponent To(Color toColor, float durationInSeconds)
        {
            return new FlickerRequestComponent
            {
                ToColor = toColor,
                DurationInSeconds = durationInSeconds,
                _FromColor = Color.white,
                _ElapsedTimeInSeconds = 0
            };
        }
        
        public static FlickerRequestComponent FlickerRed025()
        {
            return new FlickerRequestComponent
            {
                ToColor = Color.red,
                DurationInSeconds = 0.25f,
                _FromColor = Color.white,
                _ElapsedTimeInSeconds = 0
            };
        }

	}
}
