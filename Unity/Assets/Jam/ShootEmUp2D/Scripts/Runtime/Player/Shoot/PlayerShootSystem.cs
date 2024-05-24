﻿using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.GameState;
using RMC.DOTS.Systems.Input;
using RMC.DOTS.Systems.PhysicsVelocityImpulse;
using RMC.DOTS.Systems.Player;
using Unity.Entities;
using Unity.Transforms;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    /// <summary>
    /// This system moves the player in 3D space.
    /// </summary>
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial struct PlayerShootSystem : ISystem
    {
        private int _tempPitchCount;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputComponent>();
            state.RequireForUpdate<PlayerShootSystemAuthoring.PlayerShootSystemIsEnabledTag>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<GameStateComponent>();
        }
		
        public void OnUpdate(ref SystemState state)
        {
            // Check GameStateComponent
            GameStateComponent gameStateComponent = SystemAPI.GetSingleton<GameStateComponent>();
            if (gameStateComponent.GameState != GameState.RoundStarted)
            {
                return;
            }
            

            // Get the ArrowKeys for Look from the InputComponent. 
            var inputComponent = SystemAPI.GetSingleton<InputComponent>();
            bool isPressedAction1 = inputComponent.IsPressedAction1;
            bool isPressedAction2 = inputComponent.IsPressedAction2;
            bool isShooting = isPressedAction1 || isPressedAction2;
            if (!isShooting)
            {
                return;
            }
            
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().
                CreateCommandBuffer(state.WorldUnmanaged);
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var playerShootAspect 
                     in SystemAPI.Query<PlayerShootAspect>().WithAll<PlayerTag>())
            {
                // Check if the player can shoot based on the bullet fire rate
                if (playerShootAspect.CanShoot(deltaTime))
                {
                    // Instantiate the entity
                    var instanceEntity = ecb.Instantiate(playerShootAspect.BulletPrefab);
                    
                    // Move entity to initial position
                    ecb.SetComponent<LocalTransform>
                    (
                        instanceEntity,
                        LocalTransform.FromPosition(playerShootAspect.Position + playerShootAspect.Up * 1.5f)
                    );

                    
                    // Push entity once
                    var bulletForce = playerShootAspect.Up * playerShootAspect.BulletSpeed;
                    ecb.AddComponent<PhysicsVelocityImpulseComponent>
                        (
                            instanceEntity,
                            PhysicsVelocityImpulseComponent.FromForce(bulletForce)
                        );

                    
                    float[] pitches = {0.5f, 1, 1.5f};
                    float pitch = pitches[++_tempPitchCount % 3];
                    
                    // Play sound
                    var audioEntity = ecb.CreateEntity();
                    ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
                    (
                        "Click01",
                        1,
                        pitch
                    ));
                    
                    // Update shoot cooldown
                    playerShootAspect.ResetShootCooldown(playerShootAspect.BulletFireRate);
                }
            }
        }
    }
}
