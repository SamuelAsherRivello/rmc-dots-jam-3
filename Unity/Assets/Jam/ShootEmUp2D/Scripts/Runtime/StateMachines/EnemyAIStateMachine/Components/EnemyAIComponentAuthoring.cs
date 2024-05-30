using RMC.DOTS.Systems.StateMachine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    public class EnemyAIComponentAuthoring : MonoBehaviour
    {
        [Header("Wait")]
        public float WaitDurationInSeconds = 1;

        [Header("Move")]
        public Vector3 MoveOffset = new float3(0, 0.5f, 0);
        public float MoveSpeed = 50;

		public class EnemyAIComponentAuthoringBaker : 
            Baker<EnemyAIComponentAuthoring>
        {
            public override void Bake(EnemyAIComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent<StateID>(entity);
                AddComponent<EnemyAIComponent>(entity, new EnemyAIComponent
				{
                    MoveOffset = authoring.MoveOffset,
					MoveSpeed = authoring.MoveSpeed,

					WaitDurationInSeconds = authoring.WaitDurationInSeconds,

				});
            }
        }
    }
}