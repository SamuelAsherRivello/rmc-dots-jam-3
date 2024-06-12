using Unity.Entities;
using Unity.Mathematics;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    struct EnemyAIComponent : IComponentData
    {
        public float3 MoveOffset;
        public float MoveSpeed;
		public float WaitDurationInSeconds;

        //Set at runtime
        public float3 _TargetPosition;
	}
}