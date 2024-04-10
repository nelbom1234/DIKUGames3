using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace Breakout.DisplayTexts {
    public class Timer {
        public int timer{get;private set;}
        private Text display;
        public Timer(Vec2F position, Vec2F extent, int time) {
            display = new Text("Time: " + 0.ToString(), position, extent);
            setTimer(time);
            display.SetColor(255,255,255,255);
        }

        public void setTimer(int time) {
            if (time <= 0) {
                timer = -1;
            }
            //game runs at 30 max fps. If the fps is lower then
            //the general game will also run slower and therefore
            //the timer should compensate by running slower as well
            //so a player does not get needlessly punished by lag
            else {
                timer = (time)*30;
                display.SetText("Time: "+((timer/30)+1).ToString());
            }
        }

        public void decrementTime() {
            timer--;
            display.SetText("Time: "+ ((timer/30)+1).ToString());
            GameEvent gameEvent = new GameEvent();
            if (timer == 0) {
                gameEvent.EventType = GameEventType.GameStateEvent;
                gameEvent.Message = "CHANGE_STATE";
                gameEvent.StringArg1 = "GAME_LOST";
                BreakoutBus.GetBus().RegisterEvent(gameEvent);
            }
        }

        public void RenderTimer() {
            display.RenderText();
        }

    }
}