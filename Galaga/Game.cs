using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using Galaga.GalagaStates;
using System.Collections.Generic;

namespace Galaga {
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private StateMachine stateMachine;
        
        public Game() {
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent, GameEventType.InputEvent});
            window = new Window("Galaga", 500, 500);
            window.RegisterEventBus(GalagaBus.GetBus());

            gameTimer = new GameTimer(30, 30);
            stateMachine = new StateMachine();
        }


        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            }
        
        public void Run() {
            while (window.IsRunning()) {;
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    window.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                    if (stateMachine.shouldExit) {
                        window.CloseWindow();
                    }
                }
                // updating game logic here
                if (gameTimer.ShouldRender()) {
                    window.Clear();
                    // rendering game entities here
                    stateMachine.ActiveState.RenderState();
                    window.SwapBuffers();
                        
                }
            }
            if (gameTimer.ShouldReset()) {
                // This update happens once every second
                window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates}, { gameTimer.CapturedFrames})";
            }
        }
    }
}