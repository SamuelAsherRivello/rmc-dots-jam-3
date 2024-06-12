using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class PlayerBulletTagAuthoring : MonoBehaviour
    {
        public class PlayerBulletTagAuthoringBaker : Baker<PlayerBulletTagAuthoring>
        {
            public override void Bake(PlayerBulletTagAuthoring tagAuthoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                AddComponent<PlayerBulletTag>(entity);
                AddComponent<PlayerBulletNotInitializedTag>(entity);
            }
        }
    }
}