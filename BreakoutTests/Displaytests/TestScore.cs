using DIKUArcade.Math;
using DIKUArcade.Graphics;
using DIKUArcade.Events;
using NUnit.Framework;
using Breakout.DisplayTexts;
using DIKUArcade.GUI;
using Breakout.Utilities;
using System.IO;

namespace BreakoutTests.DisplayTests {
    public class TestScore {
        private Score score;

        public TestScore() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            score = new Score(new Vec2F(0.0f, 0.0f), new Vec2F(0.0f, 0.0f));
        }

        [Test]
        public void TestAddPoint() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.Message = "ADD_POINTS";
            gameEvent.IntArg1 = 2;
            score.ProcessEvent(gameEvent);
            Assert.AreEqual(2, score.score);

        }
    }
}