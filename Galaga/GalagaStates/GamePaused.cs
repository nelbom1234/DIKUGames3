using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.EventBus;
using DIKUArcade.Utilities; 
using DIKUArcade.Math;
using System.Collections.Generic;
namespace Galaga.GalagaStates {
    
    public class GamePaused : IGameState {

        private static GamePaused instance = null;
        private Entity backGroundImage;
        private List<Text> menuButtons;
        private int activeMenuButton;
        private int activeMenuButtons;
        private int maxMenuButtons;
        
        public GamePaused() {
            activeMenuButton = 0;
            activeMenuButtons = 2;
            maxMenuButtons = 2;
            InitializeGameState();
        }
        
        /// <summary>
        /// The game loop can be structured differently depending on what the
        /// current game state needs.
        /// </summary>
        public void GameLoop() {

        }
        /// <summary>
        /// Use this method to initialize all the GameState's variables.
        /// Call this method at the end of the constuctor.
        /// </summary>
        public void InitializeGameState() {
            backGroundImage = new Entity(new StationaryShape(0.0f,0.0f,1.0f, 1.0f),
                new Image(Path.Combine("..","Galaga","Assets","Images","TitleImage.png")));
            menuButtons = new List<Text>(2);    
            menuButtons.Add(new Text("CONTNUE", new Vec2F(0.5f, 0.4f),new Vec2F(0.2f, 0.2f)));
            menuButtons[0].SetColor(new Vec3F(1.0f,0.0f,0.0f));
            menuButtons.Add(new Text("MAIN MENU", new Vec2F(0.5f, 0.3f), new Vec2F(0.2f, 0.2f)));
            menuButtons[1].SetColor(new Vec3F(1.0f,1.0f,1.0f));
        }
        /// <summary>
        /// Update all variables that are being used by this GameState.
        /// </summary>
        public void UpdateGameLogic() {

        }
        /// <summary>
        /// Render all entities in this GameState
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();
            foreach (Text button in menuButtons) {
                button.RenderText();
            }
        }
        /// <summary>
        /// Each state can react to key events, delegated from the host StateMachine.
        /// </summary>
        /// <param name="keyAction">Either "KEY_PRESS" or "KEY_RELEASE".</param>
        /// <param name="keyValue">The string key value (see DIKUArcade.Input.KeyTransformer
        /// for details).</param>
        public void HandleKeyEvent(string keyValue, string keyAction){
            if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                    case "KEY_UP":
                        //set color for current active button back to white
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,1.0f,1.0f));
                        activeMenuButton -= 1;
                        if (activeMenuButton < 0) {
                            activeMenuButton = 0;
                        }
                        //set new active button to red
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,0.0f,0.0f));
                        break;
                    case "KEY_DOWN":
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,1.0f,1.0f));
                        activeMenuButton += 1;
                        if (activeMenuButton >= activeMenuButtons) {
                            activeMenuButton = activeMenuButtons-1;
                        }
                        menuButtons[activeMenuButton].SetColor(new Vec3F(1.0f,0.0f,0.0f));
                        break;
                    case "KEY_ENTER":
                        switch(activeMenuButton) {
                            case 0:
                                GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_RUNNING", ""));

                                break;
                            case 1:
                                GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "MAIN_MENU",""));
                                break;

                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static GamePaused GetInstance(){
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
    }
}