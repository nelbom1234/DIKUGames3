using DIKUArcade.Entities;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga.Squadron {
    public class Squadron3 : ISquadron {
        public EntityContainer<Enemy> Enemies{ get; }

        public int MaxEnemies {get;}

        public Squadron3() {
            MaxEnemies = 8;
            Enemies = new EntityContainer<Enemy>(MaxEnemies);
        }
        //method for easier making of enemies in CreateEnemies
         private void CreateEnemy(List<Image> enemyStrides, List<Image> alternativeEnemyStrides, float PosX, float PosY) {
             Enemies.AddEntity(new Enemy(
                     new DynamicShape(new Vec2F(PosX, PosY), new Vec2F(0.1f, 0.1f)),
                     new ImageStride(80, enemyStrides), new ImageStride(40, alternativeEnemyStrides)));
        }
        //X ∈ (− ∞, 0.1 ] and X ∈ [ 0.9, ∞ )
        public void CreateEnemies(List<Image> enemyStrides, List<Image> alternativeEnemyStrides) {
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.3f, 0.9f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.4f, 0.9f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.4f, 0.8f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.5f, 0.9f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.5f, 0.8f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.6f, 0.9f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.5f, 0.7f);
            CreateEnemy(enemyStrides, alternativeEnemyStrides, 0.4f, 0.7f);
        }
    }
}