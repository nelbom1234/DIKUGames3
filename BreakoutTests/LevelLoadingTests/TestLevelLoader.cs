using NUnit.Framework;
using Breakout;
using Breakout.LevelCreation;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.GUI;
using System.Collections.Generic;
using Breakout.Utilities;
using System.IO;

namespace BreakoutTests.LevelLoadingTests {
    public class TestLevelLoader {
        private LevelLoader levelLoader;
        
        public TestLevelLoader() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            levelLoader = new LevelLoader();
            

        }

        [Test]
        public void TestAddBlock() {
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","level1.txt")));
            levelLoader.AddBlock('#', new Vec2F(0.0f,0.0f));
            //76 blocks in level 1 so adding another one would mean 77 total
            Assert.AreEqual(77, levelLoader.blocks.CountEntities());
        }

        [Test]
        public void TestAddMetaData() {
            LevelLoader levelLoader2 = new LevelLoader();
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","level1.txt")));
            levelLoader2.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","level2.txt")));
            Assert.AreNotEqual(levelLoader.levelName, levelLoader2.levelName);            
        }

        [Test]
        public void TestEmptyFile() {
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","empty.txt")));
            Assert.IsTrue(true);
        }

        [Test]
        public void TestInvalidFile() {
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","wrongFile.png")));
            Assert.IsTrue(true);
        }

        [Test]
        public void TestDataStructuresLevel() {
            levelLoader.LoadLevel(FileCollector.FilePath(Path.Combine("Assets","Levels","level1.txt")));
            List<string> expectedLevel = new List<string> {
                "------------",
                "------------",
                "-qqqqqqqqqq-",
                "-qqqqqqqqqq-",
                "-111----111-",
                "-111-##-111-",
                "-111-22-111-",
                "-111-##-111-",
                "-111----111-",
                "-qqqqqqqqqq-",
                "-qqqqqqqqqq-",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",
                "------------",};
            CollectionAssert.AreEqual(expectedLevel, levelLoader.level);
        }

        [Test]
        public void TestDataStructuresMeta() {
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","level1.txt")));
            List<string> expectedMeta = new List<string> {
                "Name: LEVEL 1",
                "Time: 300",
                "PowerUp: 2"
            };
            CollectionAssert.AreEqual(expectedMeta, levelLoader.metadata);
        }
        [Test]
        public void TestDataStructuresLegend() {
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels","level1.txt")));
            List<string> expectedLegend = new List<string> {
                "#) teal-block.png",
                "1) blue-block.png",
                "2) green-block.png",
                "q) darkgreen-block.png"
            };
            CollectionAssert.AreEqual(expectedLegend, levelLoader.legend);
        }
    }
}