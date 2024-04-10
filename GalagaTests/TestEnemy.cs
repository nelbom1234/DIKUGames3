using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using System.Collections.Generic;
namespace GalagaTests
{
    public class TestEnemy  {

        private Enemy enemy;
        
        private IBaseImage enemyStridesRed;


        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();


            var images = ImageStride.CreateStrides(4, 
                Path.Combine("..","Galaga","Assets", "Images", "BlueMonster.png"));
            enemyStridesRed = new ImageStride(40, ImageStride.CreateStrides(2,
                Path.Combine("..","Galaga","Assets", "Images", "RedMonster.png")));
            enemy = new Enemy(new DynamicShape(new Vec2F(0.5f, 0.5f),new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, images), enemyStridesRed);
        }

        [Test]
        public void TestSubHP() {
            enemy.subHP(1);
            Assert.AreEqual(3,enemy.hitPoints);
        }

        [Test]
        public void TestEnrage() {
            enemy.Enrage();
            Assert.AreEqual(enemy.Image, enemyStridesRed);

        }
    }
}