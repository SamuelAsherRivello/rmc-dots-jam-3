using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    public class PlayerMoveComponentAuthoring : MonoBehaviour
    {
        public float LinearSpeed = 7.5f;
        
        public class PlayerMoveComponentAuthoringBaker : Baker<PlayerMoveComponentAuthoring>
        {
            public override void Bake(PlayerMoveComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                                
                AddComponent<PlayerMoveComponent>(entity, 
                    new PlayerMoveComponent { LinearSpeed = authoring.LinearSpeed });
            }
        }
    }
}