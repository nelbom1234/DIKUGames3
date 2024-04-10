using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using System.Collections.Generic;
using Galaga.Squadron;
namespace GalagaTests {
    public class TestSquadron {

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
        }

        [Test]
        public void TestSquadron1() {
            var Squadron = new Squadron1();
            Squadron.CreateEnemies(images, enemyStridesRed);
            enemies = Squadron.Enemies;
            Assert.AreEqual(8, enemies.CountEntities());
        }

        [Test]
        public void TestSquadron2() {
            var Squadron = new Squadron2();
            Squadron.CreateEnemies(images, enemyStridesRed);
            enemies = Squadron.Enemies;
            Assert.AreEqual(8, enemies.CountEntities());
        }

        [Test]
        public void TestSquadron3() {
            var Squadron = new Squadron3();
            Squadron.CreateEnemies(images, enemyStridesRed);
            enemies = Squadron.Enemies;
            Assert.AreEqual(8, enemies.CountEntities());
        }
    }
}