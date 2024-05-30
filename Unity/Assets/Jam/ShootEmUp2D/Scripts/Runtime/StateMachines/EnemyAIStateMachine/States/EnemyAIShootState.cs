using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    
    public class EnemyAIShootState : EnemyAIBaseState
    {
        public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);

            //Debug.Log("Shoot");
			RequestStateChangePerTransitions(entity);
		}
    }
}