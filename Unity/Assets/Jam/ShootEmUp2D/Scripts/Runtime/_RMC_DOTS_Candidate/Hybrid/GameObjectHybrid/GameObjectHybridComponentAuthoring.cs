using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class GameObjectHybridComponentAuthoring : MonoBehaviour
    {
        public class GameObjectHybridComponentAuthoringBaker : Baker<GameObjectHybridComponentAuthoring>
        {
            public override void Bake(GameObjectHybridComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponentObject<GameObjectHybridComponent>(entity, new GameObjectHybridComponent());
            }
        }
    }
}