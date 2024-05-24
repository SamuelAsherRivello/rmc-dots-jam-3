using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    public class HealthComponentAuthoring : MonoBehaviour
    {
        public float HealthMax = HealthComponent.HealthMaxDefault;
        public float HealthCurrent = HealthComponent.HealthMaxDefault;
        
		public class HealthComponentAuthoringBaker : Baker<HealthComponentAuthoring>
        {
            public override void Bake(HealthComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                //NOTE: CONSTRUCTOR is used to specify the subset of values that is required
                AddComponent(entity, HealthComponent.From(authoring.HealthCurrent, authoring.HealthMax));
			}
        }
    }
}