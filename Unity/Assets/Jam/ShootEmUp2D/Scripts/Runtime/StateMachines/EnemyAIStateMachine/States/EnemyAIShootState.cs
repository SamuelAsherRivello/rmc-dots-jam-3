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

			var ecb = this.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
	            CreateCommandBuffer(this.StateMachineSystemBase.World.Unmanaged);

			// Get Component
			ShootAspect shootAspect = EntityManager.GetAspect<ShootAspect>(entity);
            shootAspect.TryShoot(ref ecb, World.Time);

			//Debug.Log("Shoot");
			RequestStateChangePerTransitions(entity);
		}
    }
}