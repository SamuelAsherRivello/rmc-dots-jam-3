using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class PickupDropComponentAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float GemSpeed = 10;
        
		public class PickupDropComponentAuthoringBaker : Baker<PickupDropComponentAuthoring>
        {
            public override void Bake(PickupDropComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                //NOTE: CONSTRUCTOR is used to specify the subset of values that is required
				AddComponent(entity, new PickupDropComponent 
				(
					GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic), 
					authoring.GemSpeed)
				);
			}
        }
    }
}