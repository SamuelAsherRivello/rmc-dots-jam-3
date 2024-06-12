using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class PickupComponentAuthoring : MonoBehaviour
    {
        // TODO: Make linear speed actually do something
        public float PickupLinearSpeed = 10;
        
		public class PickupComponentAuthoringBaker : Baker<PickupComponentAuthoring>
        {
            public override void Bake(PickupComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                //NOTE: CONSTRUCTOR is used to specify the subset of values that is required
				AddComponent(entity, new PickupComponent 
				(
					authoring.PickupLinearSpeed)
				);
			}
        }
    }
}