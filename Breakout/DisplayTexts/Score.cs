using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace Breakout.DisplayTexts {
    public class Score : IGameEventProcessor {
        public int score{get;private set;}
        private Text display;
        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text("score: " + score.ToString(), position, extent);
            display.SetColor(255,255,255,255);

            BreakoutBus.GetBus().Subscribe(GameEventType.StatusEvent, this);
        }
        private void AddPoint(int points) {
            score += points;
            display.SetText("score: "+ score.ToString());
        }

        public void RenderScore() {
            display.RenderText();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch(gameEvent.Message) {
                case "ADD_POINTS":
                    AddPoint(gameEvent.IntArg1);
                    break;
                default:
                    break;

            }
        }
    }
}