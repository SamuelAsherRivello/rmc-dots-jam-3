using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    
    public class EnemyAIWaitState : EnemyAIBaseState
    {
        public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);

			// Get Component
			EnemyAIComponent enemyAIComponent = 
                EntityManager.GetComponentData<EnemyAIComponent>(entity);

            // Consider Transition
            if (StateElapsedTimeInSeconds >= enemyAIComponent.WaitDurationInSeconds)
            {
                RequestStateChangePerTransitions(entity);
            }
        }
    }
}