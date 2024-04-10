using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using System.Collections.Generic;
namespace GalagaTests {
    public class TestPlayer {

        private Player player;

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

            player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("..","Galaga","Assets", "Images", "Player.png"))); 
        }

        [Test]
        public void TestKeyPressLeft() {
            player.KeyPress("KEY_LEFT");
            player.UpdateDirection();
            Assert.AreEqual(-0.01f, player.shape.Direction.X);
        }
        
        [Test]
        public void TestKeyPressRight() {
            player.KeyPress("KEY_RIGHT");
            player.UpdateDirection();
            Assert.AreEqual(0.01f, player.shape.Direction.X);
        }

        [Test]
        public void TestKeyReleaseLeft() {
            player.KeyPress("KEY_LEFT");
            player.UpdateDirection();
            player.KeyRelease("KEY_LEFT");
            player.UpdateDirection();
            Assert.AreEqual(0.0f, player.shape.Direction.X);
        }

        [Test]
        public void TestKeyReleaseRight() {
            player.KeyPress("KEY_RIGHT");
            player.UpdateDirection();
            player.KeyRelease("KEY_RIGHT");
            player.UpdateDirection();
            Assert.AreEqual(0.0f, player.shape.Direction.X);
        }
        
        
        [Test]
        public void TestMove() {
            player.shape.Direction.X = 0.1f;
            player.Move();
            Assert.AreEqual(0.55f, player.shape.Position.X);
        }

        [Test]
        public void TestMoveRightBorder() {
            player.shape.Direction.X = 1.0f;
            player.Move();
            Assert.AreEqual(0.9f, player.shape.Position.X);
        }

        [Test]
        public void TestMoveLeftBorder() {
            player.shape.Direction.X = -1.0f;
            player.Move();
            Assert.AreEqual(0.0f, player.shape.Position.X);
        }
    }
}