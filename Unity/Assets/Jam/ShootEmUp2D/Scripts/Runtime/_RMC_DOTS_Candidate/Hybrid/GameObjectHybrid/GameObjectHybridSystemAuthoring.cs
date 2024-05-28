using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    public class GameObjectHybridSystemAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public bool IsSystemEnabled = true;
        
        public struct GameObjectHybridSystemIsEnabled : IComponentData {}
        
        public class GameObjectHybridSystemAuthoringBaker : Baker<GameObjectHybridSystemAuthoring>
        {
            public override void Bake(GameObjectHybridSystemAuthoring authoring)
            {
                if (authoring.IsSystemEnabled)
                {
                    Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    AddComponent<GameObjectHybridSystemIsEnabled>(entity);
                }
            }
        }
    }
}