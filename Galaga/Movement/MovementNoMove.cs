using DIKUArcade.Entities;


namespace Galaga.MovementStrategy {
    public class MovementNoMove : IMovementStrategy {
        
        public void MoveEnemy(Enemy enemy) {
            enemy.shape.Direction.X = 0.0f;
            enemy.shape.Direction.Y = 0.0f;
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate(enemy => { 
                MoveEnemy(enemy);     
            }); 
        }
    }
}