using NUnit.Framework;
using Breakout;
using Breakout.BreakoutStates;
using DIKUArcade.Entities;
using DIKUArcade.Events;
using System.Collections.Generic;
namespace BreakoutTests.StateTests {
    [TestFixture]
    public class TestStateMachine {
        private StateMachine stateMachine;

        public TestStateMachine() {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            BreakoutBus.ResetBus();
            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, GameEventType.GameStateEvent, GameEventType.StatusEvent,
                 GameEventType.TimedEvent});
            
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
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.GameStateEvent;
            gameEvent.To = stateMachine;
            gameEvent.Message = "CHANGE_STATE";
            gameEvent.StringArg1 = "GAME_PAUSED";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventGameRunning() {
                GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.GameStateEvent;
            gameEvent.To = stateMachine;
            gameEvent.Message = "CHANGE_STATE";
            gameEvent.StringArg1 = "GAME_RUNNING";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        [Test]
        public void TestEventGameWon() {
             GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.GameStateEvent;
            gameEvent.To = stateMachine;
            gameEvent.Message = "CHANGE_STATE";
            gameEvent.StringArg1 = "GAME_WON";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameOver>());
        }

        [Test]
        public void TestEventGameLost() {
             GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.GameStateEvent;
            gameEvent.To = stateMachine;
            gameEvent.Message = "CHANGE_STATE";
            gameEvent.StringArg1 = "GAME_LOST";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
            BreakoutBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameOver>());
        }
    }
}