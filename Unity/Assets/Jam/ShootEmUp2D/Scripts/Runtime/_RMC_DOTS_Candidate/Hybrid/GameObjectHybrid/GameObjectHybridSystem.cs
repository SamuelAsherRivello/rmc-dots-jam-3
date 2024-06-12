using System.Linq;
using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.SystemGroups;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Systems.Health
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial class GameObjectHybridSystem : SystemBase
    {
        private GameObjectHybridMonoBehavior _gameObjectHybridMonoBehavior;
        
        protected override void OnCreate()
        {
            RequireForUpdate<GameObjectHybridSystemAuthoring.GameObjectHybridSystemIsEnabled>();

            _gameObjectHybridMonoBehavior =
                UnityEngine.Object.FindObjectsByType<GameObjectHybridMonoBehavior>(FindObjectsSortMode.None).FirstOrDefault();
        }

        protected override void OnUpdate()
        {
            foreach (var (gameObjectHybridComponent, localTransform, entity) in 
                     SystemAPI.Query<GameObjectHybridComponent, RefRO<LocalTransform>>().
                         WithEntityAccess())
            {
                
                //Get reference. TODO: Better way to do this? Through scene is tricky since
                // you can't drag a non-subscene reference to a subscene object or vice vera
                if (gameObjectHybridComponent.GameObject == null)
                {
                    gameObjectHybridComponent.GameObject = _gameObjectHybridMonoBehavior.gameObject;
                }

                //sync. Works great
                if (!gameObjectHybridComponent.GameObject.transform.position.Equals(localTransform.ValueRO.Position))
                {
                    gameObjectHybridComponent.GameObject.transform.position =
                        localTransform.ValueRO.Position + gameObjectHybridComponent.PositionOffset;
                }
                
            }
        }
    }
}