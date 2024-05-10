using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class PickupTagAuthoring : MonoBehaviour
    {
        public class PickupTagAuthoringBaker : Baker<PickupTagAuthoring>
        {
            public override void Bake(PickupTagAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                AddComponent<PickupTag>(entity);
            }
        }
    }
}