using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using NUnit.Framework;
using Breakout.DisplayTexts;
using DIKUArcade.GUI;
using Breakout.Utilities;
using System.IO;

namespace BreakoutTests.DisplayTests {
    public class TestLives {
        private Lives lives;

        public TestLives() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            lives = new Lives(new Vec2F(0.0f, 0.0f), new Vec2F(0.0f, 0.0f));
        }

        [Test]
        public void TestRemoveLife() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.Message = "REMOVE_LIFE";
            lives.ProcessEvent(gameEvent);
            Assert.AreEqual(2, lives.lives);
        }
        [Test]
        public void TestAddLife() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.Message = "ADD_LIFE";
            lives.ProcessEvent(gameEvent);
            Assert.AreEqual(4, lives.lives);
        }
        [Test]
        public void TestAddLifeCap() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.Message = "ADD_LIFE";
            lives.ProcessEvent(gameEvent);
            lives.ProcessEvent(gameEvent);
            lives.ProcessEvent(gameEvent);
            Assert.AreEqual(5, lives.lives);

        }
    }
}