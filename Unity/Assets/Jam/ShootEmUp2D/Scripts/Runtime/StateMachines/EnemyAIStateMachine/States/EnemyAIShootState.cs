using RMC.DOTS.Systems.Player;
using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    
    public class EnemyAIShootState : EnemyAIBaseState
    {
        public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);

			// Get Component
			ShootAspect shootAspect = EntityManager.GetAspect<ShootAspect>(entity);
            shootAspect.TryShoot(ref Commands, World.Time);

			//Debug.Log("Shoot");
			RequestStateChangePerTransitions(entity);
		}
    }
}