using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class EnemyBulletTagAuthoring : MonoBehaviour
    {
        public class EnemyBulletTagAuthoringBaker : Baker<EnemyBulletTagAuthoring>
        {
            public override void Bake(EnemyBulletTagAuthoring tagAuthoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                AddComponent<EnemyBulletTag>(entity);
                AddComponent<EnemyBulletNotInitializedTag>(entity);
            }
        }
    }
}