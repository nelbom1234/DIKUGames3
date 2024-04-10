using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Events;
using DIKUArcade.Utilities;
using DIKUArcade.Math;
using System.Collections.Generic; 
using Breakout.Utilities;
using DIKUArcade.Input;
using Breakout.DisplayTexts;

namespace Breakout.BreakoutStates {
    
    public class GameOver : IGameState {

        private static GameOver instance = null;
        private Entity backGroundImage;
        private List<Text> menuButtons;
        private int activeMenuButton;
        private int activeMenuButtons;
        public bool shouldExit{get; private set;}
        private bool hasWon;
        private Score score;
        private Text gameStatus;
        public GameOver() {
            activeMenuButtons = 2;
            hasWon = false;
            gameStatus = new Text("GAME LOST", new Vec2F(0.3f, 0.25f), new Vec2F(0.5f, 0.5f));
            gameStatus.SetColor(new Vec3F(1.0f,1.0f,1.0f));
            ResetState();
        }
        
        /// <summary>
        /// Use this method to initialize all the GameState's variables.
        /// Call this method at the end of the constuctor.
        /// </summary>
        public void ResetState() {
            activeMenuButton = 0;
            backGroundImage = new Entity(new StationaryShape(0.0f,0.0f,1.0f, 1.0f),
                new Image(FileCollector.FilePath(Path.Combine("Assets","Images","BreakoutTitleScreen.png"))));
            menuButtons = new List<Text>(2);
            menuButtons.Add( new Text("MAIN MENU", new Vec2F(0.42f, 0.4f),new Vec2F(0.2f, 0.2f)));
            menuButtons[0].SetColor(new Vec3F(1.0f,0.0f,0.0f));
            menuButtons.Add( new Text("QUIT", new Vec2F(0.46f, 0.3f), new Vec2F(0.2f, 0.2f)));
            menuButtons[1].SetColor(new Vec3F(1.0f,1.0f,1.0f));
            score = GameRunning.GetInstance().score;
            shouldExit = false;
            if (hasWon) {
                gameStatus.SetText("GAME WON");
            }
            else {
                gameStatus.SetText("GAME LOST");
            }
        }

        public void SetGameStatus(bool result) {
            hasWon = result;
        } 

        /// <summary>
        /// Update all variables that are being used by this GameState.
        /// </summary>
        public void UpdateState() {

        }
        /// <summary>
        /// Render all entities in this GameState
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (Text button in menuButtons) {
                button.RenderText();
            }
            score.RenderScore();
            gameStatus.RenderText();
        }
        /// <summary>
        /// Each state can react to key events, delegated from the host StateMachine.
        /// </summary>
        /// <param name="keyAction">Either "KEY_PRESS" or "KEY_RELEASE".</param>
        /// <param name="keyValue">The string key value (see DIKUArcade.Input.KeyTransformer
        /// for details).</param>
        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            GameEvent gameEvent = new GameEvent();
            if (action == KeyboardAction.KeyRelease) {
                switch (key) {
                    case KeyboardKey.Up:
                        //set color for current active button back to white
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,1.0f,1.0f));
                        activeMenuButton -= 1;
                        if (activeMenuButton < 0) {
                            activeMenuButton = 0;
                        }
                        //set new active button to red
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,0.0f,0.0f));
                        break;
                    case KeyboardKey.Down:
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,1.0f,1.0f));
                        activeMenuButton += 1;
                        if (activeMenuButton >= activeMenuButtons) {
                            activeMenuButton = activeMenuButtons-1;
                        }
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,0.0f,0.0f));
                        break;
                    case KeyboardKey.Enter:
                        switch(activeMenuButton) {
                            case 0:
                                gameEvent.EventType = GameEventType.GameStateEvent;
                                gameEvent.Message = "CHANGE_STATE";
                                gameEvent.StringArg1 = "MAIN_MENU";
                                BreakoutBus.GetBus().RegisterEvent(gameEvent);
                                break;

                            case 1:
                                gameEvent.EventType = GameEventType.GameStateEvent;
                                gameEvent.Message = "CHANGE_STATE";
                                gameEvent.StringArg1 = "EXIT";
                                BreakoutBus.GetBus().RegisterEvent(gameEvent);
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static GameOver GetInstance() {
            return GameOver.instance ?? (GameOver.instance = new GameOver());
        }
    }
}