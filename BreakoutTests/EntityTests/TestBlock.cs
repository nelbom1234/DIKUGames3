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
    public class TestBlock {

        private Block block;

        public TestBlock() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            block = new Block(
                new StationaryShape(new Vec2F(0.0f,0.0f),new Vec2F(0.1f,0.1f)),
                new Image(FileCollector.FilePath(Path.Combine("Assets","Images","grey-block.png"))),
                new Image(FileCollector.FilePath(Path.Combine("Assets","Images","grey-block-damaged.png"))),
                '@');
        }

        [Test]
        public void TestHP() {
            Assert.AreEqual(3, block.hp);
        }

        [Test]
        public void TestValue() {
            Assert.AreEqual(1, block.value);
        }

        [Test]
        public void TestHit() {
            block.Hit();
            Assert.AreEqual(2, block.hp);
        }
    }
}