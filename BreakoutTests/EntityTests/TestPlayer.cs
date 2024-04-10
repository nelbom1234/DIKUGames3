using NUnit.Framework;
using Breakout;
using Breakout.LevelCreation;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.GUI;
using DIKUArcade.Graphics;
using System.Collections.Generic;
using DIKUArcade.Events;
using Breakout.Utilities;
using System.IO;

namespace BreakoutTests.EntityTests {
    public class TestPlayer {

        private Player player;

        public TestPlayer(){
            BreakoutBus.ResetBus();
            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, GameEventType.GameStateEvent, GameEventType.StatusEvent,
                 GameEventType.TimedEvent});
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.01f)),
                new Image(FileCollector.FilePath(Path.Combine("Assets","Images","player.png"))));
        }

        [Test]
        public void TestLeftKey() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.InputEvent;
            gameEvent.To = player;
            gameEvent.Message = "KEY_PRESS";
            gameEvent.StringArg1 = "KEY_LEFT";
            var oldXPosition = player.GetPosition().Copy().X;
            player.ProcessEvent(gameEvent);
            player.Move();
            Assert.Less(player.GetPosition().X, oldXPosition);
        }

        [Test]
        public void TestAKey() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.InputEvent;
            gameEvent.To = player;
            gameEvent.Message = "KEY_PRESS";
            gameEvent.StringArg1 = "KEY_A";
            var oldXPosition = player.GetPosition().Copy().X;
            player.ProcessEvent(gameEvent);
            player.Move();
            Assert.Less(player.GetPosition().X, oldXPosition);
        }

        [Test]
        public void TestRightKey() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.InputEvent;
            gameEvent.To = player;
            gameEvent.Message = "KEY_PRESS";
            gameEvent.StringArg1 = "KEY_RIGHT";
            var oldXPosition = player.GetPosition().Copy().X;
            player.ProcessEvent(gameEvent);
            player.Move();
            Assert.Greater(player.GetPosition().X, oldXPosition);
        }

        [Test]
        public void TestDKey() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.InputEvent;
            gameEvent.To = player;
            gameEvent.Message = "KEY_PRESS";
            gameEvent.StringArg1 = "KEY_D";
            var oldXPosition = player.GetPosition().Copy().X;
            player.ProcessEvent(gameEvent);
            player.Move();
            Assert.Greater(player.GetPosition().X, oldXPosition);
        }
        [Test]
        public void TestRightBoundary() {
            player.shape.Position.X = 1.0f;
            player.Move();
            Assert.AreEqual(player.GetPosition().X, 0.9f);
        }

        [Test]
        public void TestLeftBoundary() {
            player.shape.Position.X = -0.1f;
            player.Move();
            Assert.AreEqual(player.GetPosition().X, 0.0f);
        }

        [Test]
        public void TestUpperBoundary() {
            player.shape.Position.Y = 0.6f;
            player.Move();
            Assert.AreEqual(player.GetPosition().Y, 0.5f);
        }

        [Test]
        public void TestLowerBoundary() {
            player.shape.Position.Y = -0.1f;
            player.Move();
            Assert.AreEqual(player.GetPosition().Y, 0.0f);
        }
    }
}
