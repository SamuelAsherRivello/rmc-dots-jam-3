using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Demos.StateMachine.Full
{
    public class MyMovementScaleState : MyMovementBaseState
    {
        public override void OnUpdate(Entity entity)
        {
            base.OnUpdate(entity);
            var deltaTime = StateMachineSystemBase.World.Time.DeltaTime;

            // Check Component
            if (!EntityManager.HasComponent<LocalTransform>(entity) ||
                !EntityManager.HasComponent<MyMovementDataComponent>(entity))
            {
                return;
            }

            // Get Component
            MyMovementDataComponent myMovementDataComponent = 
                EntityManager.GetComponentData<MyMovementDataComponent>(entity);

            LocalTransform localTransform = 
                EntityManager.GetComponentData<LocalTransform>(entity);

            // Consider Transition
            if (StateElapsedTimeInSeconds >= myMovementDataComponent.ScaleDurationInSeconds)
            {
                RequestStateChangePerTransitions(entity);
            }
            else
            {
                // Update Component
                localTransform.Scale = localTransform.Scale + (myMovementDataComponent.ScaleDelta * deltaTime);

				// Set Component
				EntityManager.SetComponentData(entity, localTransform);
            }
        }
    }
}