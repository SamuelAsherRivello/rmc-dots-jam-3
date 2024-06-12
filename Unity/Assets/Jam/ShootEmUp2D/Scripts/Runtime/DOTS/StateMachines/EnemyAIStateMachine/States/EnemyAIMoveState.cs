using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    
    public class EnemyAIMoveState : EnemyAIBaseState
    {

		public override void OnEnter(Entity entity)
		{
			base.OnEnter(entity);

			// Get Component
			EnemyAIComponent enemyAIComponent =
				EntityManager.GetComponentData<EnemyAIComponent>(entity);

			LocalTransform localTransform =
			  EntityManager.GetComponentData<LocalTransform>(entity);


            // Update Component
            enemyAIComponent._TargetPosition = localTransform.Position + enemyAIComponent.MoveOffset;

			// Set Component
			EntityManager.SetComponentData(entity, enemyAIComponent);
		}


		public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);
            var deltaTime = StateMachineSystemBase.World.Time.DeltaTime;

			// Get Component
			EnemyAIComponent enemyAIComponent = 
                EntityManager.GetComponentData<EnemyAIComponent>(entity);

            LocalTransform localTransform = 
                EntityManager.GetComponentData<LocalTransform>(entity);

            // Consider Transition
            if (math.abs(math.distance(enemyAIComponent._TargetPosition, localTransform.Position)) < 0.001f)
            {
				RequestStateChangePerTransitions(entity);
			}
            else
            {
                // Update Component
                localTransform.Position = math.lerp
                    (
                        localTransform.Position,
						enemyAIComponent._TargetPosition, 
                        enemyAIComponent.MoveSpeed * deltaTime
                    );
                
                // Set Component
                EntityManager.SetComponentData(entity, localTransform);
            }
        }
    }
}