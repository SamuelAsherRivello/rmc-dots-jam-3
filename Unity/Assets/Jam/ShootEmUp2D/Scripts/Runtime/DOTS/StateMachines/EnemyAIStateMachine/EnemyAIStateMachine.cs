using RMC.DOTS.Systems.StateMachine;
using Unity.Collections;
using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    partial class EnemyAIStateMachine : StateMachineSystem<EnemyAIComponent>
    {


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