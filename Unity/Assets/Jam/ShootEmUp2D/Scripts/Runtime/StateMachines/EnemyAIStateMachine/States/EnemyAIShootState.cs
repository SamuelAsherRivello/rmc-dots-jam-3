using RMC.Audio.Data.Types;
using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.Player;
using Unity.Entities;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
	[UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
	public class EnemyAIShootState : EnemyAIBaseState
    {
		public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);

			var ecb = this.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
	            CreateCommandBuffer(this.StateMachineSystemBase.World.Unmanaged);

			// Get Component
			ShootAspect shootAspect = EntityManager.GetAspect<ShootAspect>(entity);

			if (shootAspect.TryShoot(ref ecb, this.World.Time))
			{
				// Play sound
				var audioEntity = ecb.CreateEntity();
				ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
				(
					ShootEmUp2DConstants.GunShot01 //Don't use pitch here
				));
			}

			//Debug.Log("Shoot");
			RequestStateChangePerTransitions(entity);
		}
    }
}