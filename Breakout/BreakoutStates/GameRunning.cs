using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.State;
using Breakout.Utilities;
using DIKUArcade.Input;
using Breakout;
using Breakout.LevelCreation;
using Breakout.DisplayTexts;

namespace Breakout.BreakoutStates {
    
    public class GameRunning : IGameState {

        private static GameRunning instance = null;
        private Player player;
        private LevelLoader levelLoader;
        private EntityContainer<Block> blocks;
        private EntityContainer<Ball> balls;
        private EntityContainer<PowerUp> powerUps;
        private int currLevel;
        private int blocksLeft;
        public Score score{get; private set;}
        private Lives lives;
        private Timer timer;
        
        public GameRunning() {
            levelLoader = new LevelLoader();
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets", "Levels","level1.txt")));
            currLevel = 1;
            blocks = levelLoader.blocks;
            blocksLeft = blocks.CountEntities();

            player = new Player(
                new DynamicShape(new Vec2F(0.5f, 0.1f), new Vec2F(0.15f, 0.01f)),
                new Image(FileCollector.FilePath(
                    Path.Combine("Assets","Images","player.png"))));

            balls = new EntityContainer<Ball>();
            addBall();
            powerUps = new EntityContainer<PowerUp>();

            BreakoutBus.GetBus().Subscribe(GameEventType.InputEvent, player);            
            //setting game over

            score = new Score(new Vec2F(0.1f, -0.2f), new Vec2F(0.3f, 0.3f));
            lives = new Lives(new Vec2F(0.8f,-0.2f), new Vec2F(0.3f, 0.3f));
            timer = new Timer(new Vec2F(0.8f, 0.6f), new Vec2F(0.3f, 0.3f), levelLoader.levelTime);
        }

        public void Reset() {
            GameRunning.instance = new GameRunning();
        }

        public void addBall() {
            Ball ball = new Ball(
                new DynamicShape(new Vec2F(0.55f, 0.1f), new Vec2F(0.021f, 0.021f)),
                new Image(FileCollector.FilePath(
                    Path.Combine("Assets","Images","ball.png"))));
            balls.AddEntity(ball);
        }

        public void GameWon() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.GameStateEvent;
            gameEvent.Message = "CHANGE_STATE";
            gameEvent.StringArg1 = "GAME_WON";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
        }

        private void loseLife() {
            GameEvent gameEvent = new GameEvent();
            gameEvent.EventType = GameEventType.StatusEvent;
            gameEvent.Message = "REMOVE_LIFE";
            BreakoutBus.GetBus().RegisterEvent(gameEvent);
            addBall();
        }

        //true if want wide, false if want normal
        public void Wide(bool val) {
            if (val) {
                if (player.shape.Extent.X != 0.3f) {
                    player.shape.Extent.X = 0.3f;
                    player.shape.Position.X -= 0.075f;
                }
            }
            else {
                if (player.shape.Extent.X != 0.15f) {
                    player.shape.Extent.X = 0.15f;
                    player.shape.Position.X += 0.075f;
                }
            }
        }

        public void DoubleSpeed(bool val) {
            balls.Iterate( ball => {
                ball.DoubleSpeed(val);
            });
        }

        public void HalfSpeed(bool val) {
            balls.Iterate( ball => {
                ball.HalfSpeed(val);
            });
        }

        private void IterateBalls() {
            if (balls.CountEntities() == 0) {
                loseLife();
            }
            else {
                balls.Iterate(ball => {
                if (!ball.hasLaunched) {
                    ball.shape.Position.X = player.shape.Position.X+player.shape.Extent.X/2;
                }
                else {
                    ball.Move();
                    if (CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),player.shape).Collision) {
                        ball.shape.Direction.Y = -ball.shape.Direction.Y;
                        float playerMiddle = player.shape.Position.X+player.shape.Extent.X/2;
                        float ballRelativePosition = ball.shape.Position.X - playerMiddle;
                        ball.shape.Direction.X += ballRelativePosition/6.0f;
                        if (ball.shape.Direction.X > 0.03f) {
                            ball.shape.Direction.X = 0.03f;
                        }
                        else if (ball.shape.Direction.X < -0.03f) {
                            ball.shape.Direction.X = -0.03f;
                        }
                    }
                    blocks.Iterate(block => {
                        if (CollisionDetection.Aabb(ball.Shape.AsDynamicShape(),block.shape).Collision) {
                            block.Hit();
                            if (ball.shape.Position.X >= block.shape.Position.X+block.shape.Extent.X) {
                                ball.shape.AsDynamicShape().Direction.X = -ball.shape.Direction.X;
                            }
                            else if (ball.shape.Position.X+ball.shape.Extent.X < block.shape.Position.X) {
                                ball.shape.AsDynamicShape().Direction.X = -ball.shape.Direction.X;
                            }
                            else ball.shape.AsDynamicShape().Direction.Y = -ball.shape.Direction.Y;
                            if (block.GetType().Name == "HardenedBlock" && block.hp <= 3) {
                                block.Damage();
                            }
                            if (block.GetType().Name != "UnbreakableBlock" && block.hp < 1) {
                                if (block.GetType().Name == "PowerUpBlock") {
                                    powerUps.AddEntity(((PowerUpBlock)block).powerUp);
                                }
                                block.DeleteEntity();
                                blocksLeft--;
                                GameEvent gameEvent = new GameEvent();
                                gameEvent.EventType = GameEventType.StatusEvent;
                                gameEvent.Message = "ADD_POINTS";
                                gameEvent.IntArg1 = block.value;
                                BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            }
                            if (blocksLeft == 0) {
                                changeLevel();
                            }
                        }
                        
                    });
                }
                });
            }
        }
        public void IteratePowerUps() {
            powerUps.Iterate(powerUp => {
                powerUp.Move();
                if (CollisionDetection.Aabb(powerUp.Shape.AsDynamicShape(),player.shape).Collision) {
                    //activate powerup
                    switch(powerUp.powerUp) {
                        case 0:
                            //extra life
                            GameEvent gameEvent = new GameEvent();
                            gameEvent.EventType = GameEventType.StatusEvent;
                            gameEvent.Message = "ADD_LIFE";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case 1:
                            //extra ball
                            addBall();
                            break;
                        case 2:
                            //Wide
                            Wide(true);

                            BreakoutBus.GetBus().RegisterTimedEvent(
                                new GameEvent {
                                    EventType = GameEventType.TimedEvent, 
                                    Message = "POWERUP",
                                    StringArg1 = "WIDE"},
                                TimePeriod.NewSeconds(15.0));
                            break;
                        case 3:
                            //double speed
                            DoubleSpeed(true);
                            BreakoutBus.GetBus().RegisterTimedEvent(
                                new GameEvent {
                                    EventType = GameEventType.TimedEvent, 
                                    Message = "POWERUP",
                                    StringArg1 = "DOUBLE_SPEED"},
                                TimePeriod.NewSeconds(10.0));
                            break;
                        case 4:
                            //half speed
                            HalfSpeed(true);
                            BreakoutBus.GetBus().RegisterTimedEvent(
                                new GameEvent {
                                    EventType = GameEventType.TimedEvent, 
                                    Message = "POWERUP",
                                    StringArg1 = "HALF_SPEED"},
                                TimePeriod.NewSeconds(10.0));
                            break;
                    }
                    //delete powerup
                    powerUp.DeleteEntity();
                }
            });
        }
        public void newLevel(string level) {
            blocks.ClearContainer();
            levelLoader.LoadLevel(FileCollector.FilePath(
                Path.Combine("Assets","Levels", level)));
            blocksLeft = blocks.CountEntities();
            //remove unbreakable blocks from the blocks we need to remove
            //Not the prettiest way to do it, but it gets the job done
            blocks.Iterate(block =>  {
                if (block.GetType().Name == "UnbreakableBlock") {
                    blocksLeft--;
                }
            });
            balls.ClearContainer();
            addBall();
        }
        public void changeLevel() {
            switch(currLevel) {
                case 1: 
                    newLevel("level2.txt");
                    currLevel = 2;
                    break;
                case 2: 
                    newLevel("Level3.txt");
                    currLevel = 3;
                    break;
                case 3: 
                    newLevel("level4.txt");
                    currLevel = 4;
                    break;
                case 4:
                    GameWon();
                    break;
            }
        }

        /// <summary>
        /// Use this method to initialize all the GameState's variables.
        /// Call this method at the end of the constuctor.
        /// </summary>
        public void ResetState() {

        }
        /// <summary>
        /// Update all variables that are being used by this GameState.
        /// </summary>
        public void UpdateState() {
            player.Move();
            IterateBalls();
            IteratePowerUps();
            if (timer.timer > 0) {
            timer.decrementTime();
            }
        }
        /// <summary>
        /// Render all entities in this GameState
        /// </summary>
        public void RenderState() {
                player.Render();
                blocks.RenderEntities();
                balls.RenderEntities();
                score.RenderScore();
                lives.RenderLives();
                powerUps.RenderEntities();

                //feels like this could be made better by having decrement
                //time and render timer together since they use same if-statement
                //however one renders and one updates, so I'll keep them seperated
                //to have them in their respective methods
                if (timer.timer > 0) {
                    timer.RenderTimer();
                }
        }
        /// <summary>
        /// Each state can react to key events, delegated from the host StateMachine.
        /// </summary>
        /// <param name="keyAction">Either "KEY_PRESS" or "KEY_RELEASE".</param>
        /// <param name="keyValue">The string key value (see DIKUArcade.Input.KeyTransformer
        /// for details).</param>
        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key){
            GameEvent gameEvent = new GameEvent();
            switch(action) {
                case KeyboardAction.KeyRelease:
                    switch(key) {
                        case KeyboardKey.Escape:
                            gameEvent.EventType = GameEventType.GameStateEvent;
                            gameEvent.Message = "CHANGE_STATE";
                            gameEvent.StringArg1 = "GAME_PAUSED";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.A:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_RELEASE";
                            gameEvent.StringArg1 = "KEY_A";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.D: 
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_RELEASE";
                            gameEvent.StringArg1 = "KEY_D";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.Left:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_RELEASE";
                            gameEvent.StringArg1 = "KEY_LEFT";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.Right: 
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_RELEASE";
                            gameEvent.StringArg1 = "KEY_RIGHT";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        //For testing purposes
                        case KeyboardKey.H:
                            blocks.Iterate(block => { 
                                block.Hit();
                            });
                            break;
                        case KeyboardKey.Space:
                                balls.Iterate(ball => {
                                    if (!ball.hasLaunched) {
                                        ball.Launch();
                                    }
                                });
                            break;
                    default:
                        break;
                    }
                    break;
                case KeyboardAction.KeyPress:
                    switch(key) {
                        case KeyboardKey.A:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_PRESS";
                            gameEvent.StringArg1 = "KEY_A";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.D:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_PRESS";
                            gameEvent.StringArg1 = "KEY_D";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.Left:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_PRESS";
                            gameEvent.StringArg1 = "KEY_LEFT";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        case KeyboardKey.Right:
                            gameEvent.EventType = GameEventType.InputEvent;
                            gameEvent.To = player;
                            gameEvent.Message = "KEY_PRESS";
                            gameEvent.StringArg1 = "KEY_RIGHT";
                            BreakoutBus.GetBus().RegisterEvent(gameEvent);
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }


        public static GameRunning GetInstance(){
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
    }
}