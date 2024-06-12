using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
	public class GameObjectHybridComponentAuthoring : MonoBehaviour
    {
        [Tooltip("The GameObject will follow this entity position plus the PositionOffset")]
        public Vector3 PositionOffset = new Vector3(0, 0, 0);
        
        public class GameObjectHybridComponentAuthoringBaker : Baker<GameObjectHybridComponentAuthoring>
        {
            public override void Bake(GameObjectHybridComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponentObject<GameObjectHybridComponent>(entity, new GameObjectHybridComponent()
                {
                    PositionOffset = authoring.PositionOffset
                });
            }
        }
    }
}