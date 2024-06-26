﻿using RMC.Audio.Data.Types;
using RMC.DOTS.SystemGroups;
using RMC.DOTS.Systems.Audio;
using RMC.DOTS.Systems.GameState;
using RMC.DOTS.Systems.Input;
using RMC.DOTS.Systems.Player;
using Unity.Entities;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    [UpdateInGroup(typeof(UnpauseablePresentationSystemGroup))]
    public partial struct PlayerShootSystem : ISystem
    {
        //TODO: Make a better way to 'have any system increment a counter' (Put in RandomComponent?)
        private int _playerShootPitchCounter;
        
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
                     in SystemAPI.Query<ShootAspect>().WithAll<PlayerTag>())
            {
                if (!playerShootAspect.TryShoot(ref ecb, SystemAPI.Time))
                    continue;
                

                // Play sound
                float pitch = ShootEmUp2DConstants.PlayerShootPitches[++_playerShootPitchCounter % ShootEmUp2DConstants.PlayerShootPitches.Length];
                var audioEntity = ecb.CreateEntity();
                ecb.AddComponent<AudioComponent>(audioEntity, new AudioComponent
                (
                    ShootEmUp2DConstants.GunShot01,
                    AudioConstants.VolumeDefault,
                    pitch
                ));
            }
        }
    }
}
