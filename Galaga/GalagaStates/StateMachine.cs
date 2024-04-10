using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga.GalagaStates { 
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState {get; private set;}

        public bool shouldExit{get; private set;}

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
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

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
            else if (gameEvent.Message == "CHANGE_STATE") {
                switch(gameEvent.Parameter1) {
                    case "GAME_PAUSED":
                        SwitchState(GameStateType.GamePaused);
                        break;
                    case "GAME_RUNNING":
                        SwitchState(GameStateType.GameRunning);
                        break;
                    case "MAIN_MENU":
                        SwitchState(GameStateType.MainMenu);
                        GameRunning.GetInstance().Reset();
                        break;
                    case "EXIT":
                        shouldExit = true;
                        break;
                    case "GAME_OVER":
                        GameRunning.GetInstance().GameOver();
                        break;

                }
            }
        }
    }
}