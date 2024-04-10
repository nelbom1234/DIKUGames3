using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace GalagaTests {
    public class TestScore {

        private Score score;

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

            score = new Score(new Vec2F(0.5f, 0.5f), new Vec2F(0.1f, 0.1f));
        }

        [Test]
        public void TestAddPoint() {
            score.AddPoint();
            score.AddPoint();
            score.AddPoint();
            Assert.AreEqual(3, score.score);
        }
    }
}