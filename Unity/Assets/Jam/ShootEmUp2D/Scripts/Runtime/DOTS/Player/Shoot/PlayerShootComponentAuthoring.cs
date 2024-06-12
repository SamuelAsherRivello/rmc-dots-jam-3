using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class PlayerShootComponentAuthoring : MonoBehaviour
    {        
		public class PlayerShootComponentAuthoringBaker : Baker<PlayerShootComponentAuthoring>
        {
            public override void Bake(PlayerShootComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                //NOTE: CONSTRUCTOR is used to specify the subset of values that is required
                AddComponent(entity, new PlayerShootComponent());
			}
        }
    }
}