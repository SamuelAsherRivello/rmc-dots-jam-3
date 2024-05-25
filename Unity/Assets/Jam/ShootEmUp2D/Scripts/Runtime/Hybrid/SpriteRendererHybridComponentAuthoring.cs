using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class SpriteRendererHybridComponentAuthoring : MonoBehaviour
    {
        public class SpriteRendererHybridComponentAuthoringBaker : Baker<SpriteRendererHybridComponentAuthoring>
        {
            public override void Bake(SpriteRendererHybridComponentAuthoring tagAuthoring)
            {
            //     Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            //                     
            //     AddComponent<EnemyTag>(entity);
            }
        }
    }
}