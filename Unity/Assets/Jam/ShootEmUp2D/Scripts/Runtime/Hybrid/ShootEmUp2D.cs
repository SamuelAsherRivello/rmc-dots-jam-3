using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RMC.Audio;
using RMC.DOTS.Samples.Games.ShootEmUp2D.StateMachines.EnemyStateMachine;
using RMC.DOTS.Systems.GameState;
using RMC.DOTS.Systems.Scoring;
using RMC.DOTS.Utilities;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo.
    ///
    /// Responsibilities include to wire together the ECS areas, and the GameObject areas like UI
    /// </summary>
    public class ShootEmUp2D : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        /// <summary>
        /// This is used to freeze the rendering of the game.
        /// An alternative would be to have each of my custom systems
        /// check the GameStateSystem.IsGamePaused property.
        /// </summary>
        public bool IsEnabledSimulationSystemGroup
        {
            set
            {
                var simulationSystemGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
                simulationSystemGroup.Enabled = value;
            }
        }
        
        
        //  Fields ----------------------------------------
        [Header("Scene References")]
        [SerializeField] 
        private Common _common;

        [SerializeField] 
        private SubScene _subScene;

        [SerializeField] 
        private List<Background> _backgrounds;
        
        [Header("Settings")]
        [Tooltip("True, to show debug logs")]
        [SerializeField] 
        private bool IsDebugLog = false;

        [Tooltip("True, to show faster UI")]
        [SerializeField] 
        private bool IsDebugWaves = false;

        private GameStateSystem _gameStateSystem;
        private PlayerMoveSystem _playerMoveSystem;
        private World _ecsWorld;
        private ScoringSystem _scoringSystem;
        private int _enemyKillsThisRoundCurrent;
        private int _enemyKillsThisRoundMax;

        //  Unity Methods  --------------------------------
        protected async void Start()
        {
            // ECS
            _ecsWorld = await DOTSUtility.GetWorldAsync(_subScene);

            // Game State
            _gameStateSystem = _ecsWorld.GetExistingSystemManaged<GameStateSystem>();
            _gameStateSystem.OnIsGameOverChanged += GameStateSystem_OnIsGameOverChanged;
            _gameStateSystem.OnIsGamePausedChanged += GameStateSystem_OnIsGamePausedChanged;
            _gameStateSystem.OnGameStateChanged += GameStateSystem_OnGameStateChanged;

            // Player Input/Movement
            _playerMoveSystem = _ecsWorld.GetExistingSystemManaged<PlayerMoveSystem>();

            // Enemy Killed
            // WasHitSystem wasHitSystem = _ecsWorld.GetExistingSystemManaged<WasHitSystem>();
            // wasHitSystem.OnWasHit += WasHitSystem_OnWasHit;
            
            // Scoring
            _scoringSystem = _ecsWorld.GetExistingSystemManaged<ScoringSystem>();
            _scoringSystem.OnScoringComponentChanged += ScoresEventSystem_OnScoresChanged;
            
            // UI
            _common.MainUI.OnRestartRequest.AddListener(MainUI_OnRestartRequest);
            _common.MainUI.OnRestartConfirm.AddListener(MainUI_OnRestartConfirm);
            _common.MainUI.OnRestartCancel.AddListener(MainUI_OnRestartCancel);

            _common.MainUI.OnSwitchWeaponOne.AddListener(MainUI_OnSwitchWeaponOne);
            _common.MainUI.OnSwitchWeaponTwo.AddListener(MainUI_OnSwitchWeaponTwo);
            _common.MainUI.OnSwitchWeaponThree.AddListener(MainUI_OnSwitchWeaponThree);

            // Populate UI
            RefreshWaveProgressLabel();
            ScoresEventSystem_OnScoresChanged(default(ScoringComponent));
            _common.MainUI.StatusLabel.text = $"WASD/Arrows to move\nSpacebar to shoot";
            _common.MainUI.RestartButton.text = "Restart";
            _common.MainUI.WaveTitleLabel.text = "";
            await InitializeAsync();
        }

  

        protected void OnDestroy()
        {
            DOTSUtility.DisposeAllWorlds();
        }


        //  Methods ---------------------------------------
        private async Task InitializeAsync()
        {
            await DOTSUtility.IsWorldReadyAsync(_subScene);


            SetupStateMachines();


			if (_gameStateSystem != null)
            {
                _gameStateSystem.GameState = GameState.Initializing;
            }
        }

		private void SetupStateMachines()
		{
			//Set initial state for all enemies
			EnemyAIStateMachine enemyStateMachine = _ecsWorld.CreateSystemManaged<EnemyAIStateMachine>();
            enemyStateMachine.RequestStateChangeForAllEntities<EnemyAIMoveState>();
		}

		private void RefreshWaveProgressLabel()
        {
            _common.MainUI.WaveProgressLabel.text = $"Enemies {_enemyKillsThisRoundCurrent} / {_enemyKillsThisRoundMax}";
        }


        //  Event Handlers --------------------------------
        private async void GameStateSystem_OnGameStateChanged(GameState gameState)
        {
            if (IsDebugLog)
            {
                Debug.Log($"OnGameStateChanged() gameState = {gameState}");
            }
            
            switch (gameState)
            {
                case GameState.Initializing:
                    _gameStateSystem.GameState = GameState.Initialized;
                    break;
                case GameState.Initialized:
                    _gameStateSystem.GameState = GameState.GameStarting;
                    break;
  
                case GameState.GameStarting: 
                    
                    _gameStateSystem.IsGamePaused = false;
                    _gameStateSystem.IsGameOver = false;
                    
                    _gameStateSystem.RoundData = new RoundData
                    {
                        RoundCurrent = 0,
                        RoundMax = 3
                    };
                    
                    _gameStateSystem.GameState = GameState.GameStarted;
                    break;
                case GameState.GameStarted: 
                    _gameStateSystem.GameState = GameState.RoundStarting;
                    break;
                case GameState.RoundStarting: 
                    _gameStateSystem.RoundData = new RoundData
                    {
                        RoundCurrent = _gameStateSystem.RoundData.RoundCurrent + 1,
                        RoundMax = _gameStateSystem.RoundData.RoundMax
                    };
                    
                    // Increase difficulty
                    _enemyKillsThisRoundCurrent = 0;
                    _enemyKillsThisRoundMax = 1 * _gameStateSystem.RoundData.RoundCurrent;
                    RefreshWaveProgressLabel();
                    
                    // Faster waves when debugging
                    float waveDelay = IsDebugWaves ? 0.1f : 1f;
                    int delayBeforeTextLine1 = (int)(300 * waveDelay);
                    int delayBeforeTextLine2 = (int)(500 * waveDelay);
                    int delayAfterText = (int)(2000 * waveDelay);
                    
                    try
                    {
                        // Show wave line 1
                        await Task.Delay(delayBeforeTextLine1, destroyCancellationToken);
                        _common.MainUI.WaveTitleLabel.text = $"Wave {_gameStateSystem.RoundData.RoundCurrent} " +
                                                        $"of {_gameStateSystem.RoundData.RoundMax}";
                        
                        // Show wave line 2
                        await Task.Delay(delayBeforeTextLine2, destroyCancellationToken);
                        _common.MainUI.WaveTitleLabel.text = $"Wave {_gameStateSystem.RoundData.RoundCurrent} " +
                                                        $"of {_gameStateSystem.RoundData.RoundMax}\n"+
                                                        $"Kill {_enemyKillsThisRoundMax} Enemies!";
                        
                        // Clear wave lines
                        await Task.Delay(delayAfterText, destroyCancellationToken);
                        _common.MainUI.WaveTitleLabel.text = "";
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    
                    _gameStateSystem.GameState = GameState.RoundStarted;
                    break;
                case GameState.RoundStarted: 
                    // The core game is played here...
                    break;
                case GameState.GameEnding: 
                    _gameStateSystem.GameState = GameState.GameEnded;
                    break;
                case GameState.GameEnded: 
                    _gameStateSystem.IsGameOver = true;
                    break;
            }
        }
        
        private void GameStateSystem_OnIsGameOverChanged(bool isGameOver)
        {
            if (isGameOver)
            {
                // Show victory
                _common.MainUI.StatusLabel.text = "You Win!";

                // Freeze game
                IsEnabledSimulationSystemGroup = false;
            }
        }


        private void GameStateSystem_OnIsGamePausedChanged(bool isGamePaused)
        {
            IsEnabledSimulationSystemGroup = !isGamePaused;
            
            _backgrounds.ForEach(x => x.IsPaused = isGamePaused);
        }
        
        
        private void WasHitSystem_OnWasHit(Type t, bool wasDestroyed)
        {
            // if (t == typeof(EnemyTag) && wasDestroyed)
            // {
            //     _enemyKillsThisRoundCurrent++;
            // }
            //
            // if (_enemyKillsThisRoundCurrent >= _enemyKillsThisRoundMax)
            // {
            //     _gameStateSystem.GameState = GameState.RoundStarting;
            // }
            //
            RefreshWaveProgressLabel();
        }


        private void ScoresEventSystem_OnScoresChanged(ScoringComponent scoringComponent)
        {
            var gemsCurrent = 0;
            var gemsMax = 0;
            
            if (!scoringComponent.Equals(default(ScoringComponent)))
            {
                gemsCurrent = scoringComponent.ScoreComponent01.ScoreCurrent;
                gemsMax = scoringComponent.ScoreComponent01.ScoreMax;
            }

            _common.MainUI.ScoreLabel.text =
                     $"Pickups: {gemsCurrent}"; //Don't show "/gemsMax" anymore?
        }
        
        
        private void MainUI_OnRestartRequest()
        {
            AudioManager.Instance.PlayAudioClip("Click01");

            _gameStateSystem.IsGamePaused = true;
        }


        private void MainUI_OnRestartCancel()
        {
            AudioManager.Instance.PlayAudioClip("Click02");

            //only unpause if the game is not over
            if (!_gameStateSystem.IsGameOver)
            {
                _gameStateSystem.IsGamePaused = false;
            }
        }

        
        private async void MainUI_OnRestartConfirm()
        {
            AudioManager.Instance.PlayAudioClip("Click01");

            _gameStateSystem.IsGamePaused = false;

            await DOTSUtility.ReloadWorldAsync(_subScene);
        }

        private void MainUI_OnSwitchWeaponOne()
        {
            _playerMoveSystem.CurrentActiveWeaponSlot = 0;
        }
        
        private void MainUI_OnSwitchWeaponTwo()
        {
            _playerMoveSystem.CurrentActiveWeaponSlot = 1;
        }

        private void MainUI_OnSwitchWeaponThree()
        {
            _playerMoveSystem.CurrentActiveWeaponSlot = 2;
        }
    }
}