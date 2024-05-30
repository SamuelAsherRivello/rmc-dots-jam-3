using RMC.DOTS.Systems.StateMachine;
using Unity.Collections;
using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    partial class EnemyAIStateMachine : StateMachineSystem<EnemyAIComponent>
    {

		//TODO 1: Rename "Commands" in State class to "EntityCommandBuffer"


		//TODO 2: Move this new method to the dots source, and on base class
		public void RequestStateChangeForAllEntities<T>() where T : State
		{
			var query = GetStateEntityQuery();
			var entities = query.ToEntityArray(Allocator.Temp);
			foreach (Entity entity in entities)
			{
				RequestStateChange<T>(entity);
			}
		}

		protected override void OnCreate()
        {
            base.OnCreate();

			//Requires

			//Registers
			RegisterState<EnemyAIWaitState>();
			RegisterState<EnemyAIMoveState>();
			RegisterState<EnemyAIShootState>();
		}
    }
}