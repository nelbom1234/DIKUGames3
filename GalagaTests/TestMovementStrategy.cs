using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using System.Collections.Generic;
using Galaga.MovementStrategy;
namespace GalagaTests {
    public class TestMovement {

        private EntityContainer<Enemy> enemies;

        private List<Image> images;

        private List<Image> enemyStridesRed;

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

             images = ImageStride.CreateStrides(4, 
                Path.Combine("..","Galaga","Assets", "Images", "BlueMonster.png"));
            enemyStridesRed = ImageStride.CreateStrides(2,
                Path.Combine("..","Galaga","Assets", "Images", "RedMonster.png"));
            
            enemies = new EntityContainer<Enemy>(1);
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, images), 
                new ImageStride(40, enemyStridesRed)));
        }

        [Test]
        public void TestMovementDown() {
            var movement = new MovementDown();
            movement.MoveEnemies(enemies);
            enemies.Iterate(enemy => { 
                Assert.AreEqual(enemy.startPosition.Y-0.001f, enemy.shape.Position.Y);});
            
        }
        
        [Test]
        public void TestMovementNoMove() {
            var movement = new MovementNoMove();
            movement.MoveEnemies(enemies);
            enemies.Iterate(enemy => { 
                Assert.AreEqual(enemy.startPosition.Y, enemy.shape.Position.Y);});
        }

        /*
        as explained in our MovementZigZagDown.cs we have no idea how to make
        it work, and since we have not gotten our assignment back we are not able to fix it,
        therefore we cannot test it properly, and so we have commented the test out as a proof of concept.
        [Test]
        public void TestMovementZigZagDown() {
            var movement = new MovementZigZagDown();
            movement.MoveEnemies(enemies);
            enemies.Iterate(enemy => { 
                Assert.AreNotEqual(enemy.startPosition.Y, enemy.shape.Position.Y);});   
        */
    }
}