using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using System.Collections.Generic;
using Galaga.GalagaStates;
namespace GalagaTests {
    public class TestStateTransformer {

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();
        }

        [Test]
        public void TestGameRunningString() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_RUNNING"),
                GameStateType.GameRunning);
        }
        
        [Test]
        public void TestGamePausedString() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_PAUSED"),
                GameStateType.GamePaused);
        }

        [Test]
        public void TestMainMenuString() {
            Assert.AreEqual(StateTransformer.TransformStringToState("MAIN_MENU"),
                GameStateType.MainMenu);
        }
        
        [Test]
        public void TestGameRunningState() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GameRunning),
                "GAME_RUNNING");
        }
        
        [Test]
        public void TestGamePausedState() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GamePaused),
                "GAME_PAUSED");
        }
        
        [Test]
        public void TestMainMenuState() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.MainMenu), 
                "MAIN_MENU");
        }
    }
}