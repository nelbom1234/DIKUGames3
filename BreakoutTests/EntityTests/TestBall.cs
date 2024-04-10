using NUnit.Framework;
using Breakout;
using Breakout.LevelCreation;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using Breakout.Utilities;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using System.Collections.Generic;
using System.IO;

namespace BreakoutTests.EntityTests {
    public class TestBall {

        private Ball ball;

        public TestBall() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            ball = new Ball(
                new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F(0.021f, 0.021f)),
                new Image(FileCollector.FilePath(Path.Combine("Assets","Images","ball.png"))));
            
        }

        [Test]
        public void TestLaunchX() {
            ball.Launch();
            Assert.AreNotEqual(ball.shape.Direction.X, 0.0f);
        }

        [Test]
        public void TestLaunchY() {
            ball.Launch();
            Assert.AreEqual(ball.shape.Direction.Y, 0.01f);
        }

        [Test]
        public void TestDoubleSpeedOn() {
            ball.Launch();
            ball.DoubleSpeed(true);
            Assert.AreEqual(ball.shape.Direction.Y, 0.02f);
        }
        [Test]
        public void TestDoubleSpeedOff() {
            ball.Launch();
            ball.DoubleSpeed(true);
            ball.DoubleSpeed(false);
            Assert.AreEqual(ball.shape.Direction.Y, 0.01f);
        }
        [Test]
        public void TestDoubleSpeedCap() {
            ball.Launch();
            ball.DoubleSpeed(true);
            ball.DoubleSpeed(true);
            Assert.AreEqual(ball.shape.Direction.Y, 0.02f);
        }
        [Test]
        public void TestHalfSpeedOn() {
            ball.Launch();
            ball.HalfSpeed(true);
            Assert.AreEqual(ball.shape.Direction.Y, 0.005f);
        }
        [Test]
        public void TestHalfSpeedOff() {
            ball.Launch();
            ball.HalfSpeed(true);
            ball.HalfSpeed(false);
            Assert.AreEqual(ball.shape.Direction.Y, 0.01f);
        }
        [Test]
        public void TestHalfSpeedCap() {
            ball.Launch();
            ball.HalfSpeed(true);
            ball.HalfSpeed(true);
            Assert.AreEqual(ball.shape.Direction.Y, 0.005f);
        }
        [Test]
        public void TestMoveX() {
            ball.shape.Direction = new Vec2F(0.1f, 0.0f);
            ball.Move();
            Assert.AreEqual(ball.shape.Position.X, 0.1f);
        }
        [Test]
        public void TestMoveY() {
            ball.shape.Direction = new Vec2F(0.0f, 0.1f);
            ball.Move();
            Assert.AreEqual(ball.shape.Position.Y, 0.1f);
        }
        [Test]
        public void TestRightBorder() {
            ball.shape.Position = new Vec2F(1.0f, 0.5f);
            ball.shape.Direction = new Vec2F(0.1f, 0.0f);
            ball.Move();
            Assert.AreEqual(ball.shape.Direction.X, -0.1f);
        }
        [Test]
        public void TestLeftBorder() {
            ball.shape.Position = new Vec2F(0.0f, 0.5f);
            ball.shape.Direction = new Vec2F(-0.1f, 0.0f);
            ball.Move();
            Assert.AreEqual(ball.shape.Direction.X, 0.1f);
        }
        [Test]
        public void TestUpperBorder() {
            ball.shape.Position = new Vec2F(0.5f, 1.0f);
            ball.shape.Direction = new Vec2F(0.0f, 0.1f);
            ball.Move();
            Assert.AreEqual(ball.shape.Direction.Y, -0.1f);
        }
        [Test]
        public void TestLowerBorder() {
            ball.shape.Position = new Vec2F(0.5f, 0.0f);
            ball.shape.Direction = new Vec2F(0.0f, -0.1f);
            ball.Move();
            Assert.IsTrue(ball.IsDeleted());
        }
    }
}