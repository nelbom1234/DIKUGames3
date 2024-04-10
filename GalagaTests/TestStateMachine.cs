using NUnit.Framework;
using Galaga;
using Galaga.GalagaStates;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using System.Collections.Generic;
namespace GalagaTests {
    [TestFixture]
    public class TestStateMachine {
        private StateMachine stateMachine;

        public TestStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent, GameEventType.InputEvent});
        }

        [SetUp]
        public void InitiateStateMachine() {
            stateMachine = new StateMachine();
        }
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        
        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING",""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
    }
}