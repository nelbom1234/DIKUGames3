using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Graphics;
using Breakout.LevelCreation;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.Collections.Generic;
using Breakout.Utilities;
using Breakout.BreakoutStates;

namespace Breakout {
    public class Game : DIKUGame {
        
        private StateMachine stateMachine;

        
         public Game(WindowArgs windowArgs) : base(windowArgs) {
            window.SetKeyEventHandler(KeyHandler);

            BreakoutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, GameEventType.GameStateEvent, 
                GameEventType.StatusEvent, GameEventType.TimedEvent});

            stateMachine  = new StateMachine();
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            //Console.WriteLine($"TestKeyEvents.KeyHandler({action}, {key})");
            stateMachine.ActiveState.HandleKeyEvent(action, key);
            if (action == KeyboardAction.KeyRelease) {
                switch (key) {
                    case KeyboardKey.Num_1:
                        window.SetClearColor(128, 52, 43);
                        break;
                    case KeyboardKey.Num_2:
                        window.SetClearColor(28, 108, 218);
                        break;
                }
            } 
        }

        public override void Render()
        {
            stateMachine.ActiveState.RenderState();
            }

        public override void Update()
        {
            BreakoutBus.GetBus().ProcessEvents();
            stateMachine.ActiveState.UpdateState();
            if (stateMachine.shouldExit) {
                window.CloseWindow();
            }
        }    
    }
}