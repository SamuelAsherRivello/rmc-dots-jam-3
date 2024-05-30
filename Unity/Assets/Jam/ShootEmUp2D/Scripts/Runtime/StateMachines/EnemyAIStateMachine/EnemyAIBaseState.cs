using RMC.DOTS.Systems.StateMachine;
using UnityEngine;
using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine
{
    public class EnemyAIBaseState : StateMachineSystemBase.State
    {
        protected float StateElapsedTimeInSeconds { get; private set; }
        
        protected virtual void RequestStateChangePerTransitions(Entity entity)
        {
            //Toggle
            if (IsInState<EnemyAIWaitState>(entity))
            {
                RequestStateChange<EnemyAIMoveState>(entity);
            }
            else if (IsInState<EnemyAIMoveState>(entity))
            {
                RequestStateChange<EnemyAIShootState>(entity);

            }
            else if (IsInState<EnemyAIShootState>(entity))
            {
                RequestStateChange<EnemyAIWaitState>(entity);
            }
            else
            {
                Debug.LogError("Unknown State");
            }
        }
        
        public override void OnEnter(Entity entity)
        {
            base.OnEnter(entity);
            StateElapsedTimeInSeconds = 0;
            //Debug.LogFormat("{0}.OnEnter() {1}\n\n", entity.ToString(), GetType().Name);
        }
        
        public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);
            StateElapsedTimeInSeconds += World.Time.DeltaTime;
            //Debug.LogFormat("{0}.OnUpdate() {1}\n\n", entity.ToString(), GetType().Name);
        }

        public override void OnExit(Entity entity)
        {
            base.OnExit(entity);
            //Debug.LogFormat("{0}.OnExit() {1}\n\n", entity.ToString(), GetType().Name);
        }
    }
}