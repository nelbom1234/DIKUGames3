using NUnit.Framework;
using Breakout;
using Breakout.LevelCreation;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using System.Collections.Generic;
using Breakout.Utilities;
using System.IO;

namespace BreakoutTests.EntityTests {
    public class TestPowerUp {
        private PowerUp powerUp;

        public TestPowerUp() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            powerUp = new PowerUp(new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(0.07f, 0.07f)), 
            new Image(FileCollector.FilePath(Path.Combine("Assets","Images","LifePickUp.png"))));
        }

        [Test]
        public void TestMove() {
            powerUp.Shape.Position = new Vec2F(0.5f,0.5f);
            powerUp.Shape.AsDynamicShape().Direction = new Vec2F(0.0f, -0.1f);
            powerUp.Move();
            Assert.AreEqual(powerUp.Shape.Position.Y, 0.4f);
        }
        [Test]
        public void TestLowerborder() {
            powerUp.Shape.Position = new Vec2F(0.5f, 0.0f);
            powerUp.Shape.AsDynamicShape().Direction = new Vec2F(0.0f, -0.1f);
            powerUp.Move();
            Assert.IsTrue(powerUp.IsDeleted());
        }
    }
}
        