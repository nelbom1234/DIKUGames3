using DIKUArcade.Math;
using DIKUArcade.Graphics;
namespace Galaga{   
    public class Score {
        public int score{get; private set;}
        private Text display;
        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text("score: " + score.ToString(), position, extent);
            display.SetColor(255,255,255,255);
        }              

        public void AddPoint() {
            score += 1;
            display.SetText("score: "+ score.ToString());
        }
        public void RenderScore() {
            display.RenderText();
        }
    }
}