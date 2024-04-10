using DIKUArcade.Entities;
using System;
using DIKUArcade.Math;
using DIKUArcade.EventBus;

namespace Galaga.MovementStrategy {
    public class MovementZigZagDown : IMovementStrategy {
        private float DifficultyMultiplier = 1.0f;
        private float a = 0.05f;
        private float s = 0.0003f;
        private float p = 0.045f;
        /* We really don't know why this doesn't work. We feel like we did everything by the book
        and it still won't work. We had Pedram have a look at it and he also has no idea, so for
        now we will leave it as it is and we'd appreciate if you could tell us if you have
        any idea what is wrong */
        public void MoveEnemy(Enemy enemy) {
            float startX = enemy.startPosition.X;
            float startY = enemy.startPosition.Y;
            enemy.shape.Position.X = (startX + a * ((float)Math.Sin((2*Math.PI*(startY-enemy.Shape.Position.Y))/p)))*1.0005f;
            if (!enemy.isEnraged) {
                enemy.shape.Position.Y -= s*DifficultyMultiplier;
            }
            else enemy.shape.Position.Y -= s*DifficultyMultiplier*1.5f;
            if (enemy.shape.Position.Y < 0.1f) {
                GalagaBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_OVER",""));
            }
        } 

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate(MoveEnemy);
        }
        public void GetDiffMult(float diff) {
            DifficultyMultiplier = diff;
        }
    }
}