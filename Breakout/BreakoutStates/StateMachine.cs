using DIKUArcade.Events;
using DIKUArcade.State;
using DIKUArcade.Input;
using Breakout.Utilities;
using System.Collections.Generic;
using DIKUArcade.Timers;

namespace Breakout.BreakoutStates { 
    public class StateMachine : IGameEventProcessor {
        public IGameState ActiveState {get; private set;}

        public bool shouldExit{get; private set;}

        public StateMachine() {
            BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            BreakoutBus.GetBus().Subscribe(GameEventType.TimedEvent, this);
            ActiveState = MainMenu.GetInstance();
            shouldExit = false;
        }

        private void SwitchState(GameStateType stateType) {
            switch(stateType) {
                case GameStateType.GameRunning:
                    ActiveState = GameRunning.GetInstance();
                    break;
                case GameStateType.GamePaused:
                    ActiveState = GamePaused.GetInstance();
                    break;
                case GameStateType.MainMenu:
                    ActiveState = MainMenu.GetInstance();
                    break;
                case GameStateType.GameWon:
                    ActiveState = GameOver.GetInstance();
                    break;
                case GameStateType.GameLost:
                    ActiveState = GameOver.GetInstance();
                    break;
            }
        }


        public void KeyPress(string key) {
            if (ActiveState == GameRunning.GetInstance()) {
                switch(key) {
                    case "KEY_ESCAPE":
                        SwitchState(GameStateType.GamePaused);
                        break;
                    default:
                        break;
                }
            }
        }

        public void KeyRelease(string key) {
            switch(key) {
                default:
                    break;
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.InputEvent) {
                KeyboardAction action = KeyConverter.ConvertActionString(gameEvent.Message);
                KeyboardKey key = KeyConverter.ConvertKeyString(gameEvent.StringArg1);
                ActiveState.HandleKeyEvent(action, key);
            }
            else if (gameEvent.Message == "POWERUP") {
                switch(gameEvent.StringArg1) {
                    case "WIDE":
                        GameRunning.GetInstance().Wide(false);
                        break;
                    case "DOUBLE_SPEED":
                        GameRunning.GetInstance().DoubleSpeed(false);
                        break;
                    case "HALF_SPEED":
                        GameRunning.GetInstance().HalfSpeed(false);
                        break;
                }
            }
            else if (gameEvent.Message == "CHANGE_STATE") {
                switch(gameEvent.StringArg1) {
                    case "GAME_PAUSED":
                        SwitchState(GameStateType.GamePaused);
                        StaticTimer.PauseTimer();
                        break;
                    case "GAME_RUNNING":
                        SwitchState(GameStateType.GameRunning);
                        StaticTimer.ResumeTimer();
                        break;
                    case "MAIN_MENU":
                        SwitchState(GameStateType.MainMenu);
                        GameRunning.GetInstance().Reset();
                        break;
                    case "EXIT":
                        shouldExit = true;
                        break;
                    case "GAME_WON":
                        SwitchState(GameStateType.GameWon);
                        GameOver.GetInstance().SetGameStatus(true);
                        GameOver.GetInstance().ResetState();
                        break;
                    case "GAME_LOST":
                        SwitchState(GameStateType.GameLost);
                        GameOver.GetInstance().SetGameStatus(false);
                        GameOver.GetInstance().ResetState();
                        break;
                }
            }
        }
    }
}