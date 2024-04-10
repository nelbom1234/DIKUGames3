using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace Breakout.DisplayTexts {
    public class Lives : IGameEventProcessor {
        public int lives{get;private set;}
        private Text display;
        public Lives(Vec2F position, Vec2F extent) {
            lives = 3;
            display = new Text("lives: " + lives.ToString(), position, extent);
            display.SetColor(255,255,255,255);

            BreakoutBus.GetBus().Subscribe(GameEventType.StatusEvent, this);
        }
        private void removeLife() {
            lives--;
            display.SetText("lives: "+ lives.ToString());
            if (lives <= 0) {
                GameEvent gameEvent = new GameEvent();
                gameEvent.EventType = GameEventType.GameStateEvent;
                gameEvent.Message = "CHANGE_STATE";
                gameEvent.StringArg1 = "GAME_LOST";
                BreakoutBus.GetBus().RegisterEvent(gameEvent);
            }
        }

        private void addLife() {
            lives++;
            if (lives > 5) {
                lives = 5;
            }
            display.SetText("lives: "+lives.ToString());
        }

        public void RenderLives() {
            display.RenderText();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch(gameEvent.Message) {
                case "REMOVE_LIFE":
                    removeLife();
                    break;
                case "ADD_LIFE":
                    addLife();
                    break;
                default:
                    break;

            }
        }
    }
}