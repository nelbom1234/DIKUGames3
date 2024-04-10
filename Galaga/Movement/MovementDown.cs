using DIKUArcade.Entities;
using DIKUArcade.EventBus;


namespace Galaga.MovementStrategy {
    public class MovementDown : IMovementStrategy {
        
        private float DifficultyMultiplier = 1.0f;
        
        public void MoveEnemy(Enemy enemy) {
            if (!enemy.isEnraged) {
                enemy.shape.Direction.Y = -0.001f*DifficultyMultiplier;
            }
            else enemy.shape.Direction.Y = -0.001f*DifficultyMultiplier*1.5f;
            enemy.shape.Move();
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
            enemies.Iterate(enemy => { 
                MoveEnemy(enemy);
            }); 
        }

        public void GetDiffMult(float diff) {
            DifficultyMultiplier = diff;
        }


    }
}