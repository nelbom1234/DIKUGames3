using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;
using Galaga.Squadron;
using Galaga.MovementStrategy; 
using DIKUArcade.State;
namespace Galaga.GalagaStates {
    
    public class GameRunning : IGameState {

        private static GameRunning instance = null;
        private Player player;
        private EntityContainer<Enemy> enemies;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;
        private List<Image> enemyStridesRed;
        private IMovementStrategy currMovementStrategy;
        private Score score;
        private float DifficultyMultiplier;
        private int enemiesLeft;
        private List<Image> images;
        private Squadron1 squad1;
        private Squadron2 squad2;
        private Squadron3 squad3;
        private MovementNoMove NoMove;
        private MovementDown MoveDown;
        private MovementZigZagDown ZigZag;
        private bool isGameOver;
        
        public GameRunning() {
            player = new Player(new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("..","Galaga","Assets", "Images", "Player.png")));
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent,player);            
            
            // Playershots instanciated
            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("..","Galaga","Assets", "Images", "BulletRed2.png"));
            
            // Creating animations and images
            images = ImageStride.CreateStrides(4, Path.Combine("..","Galaga","Assets", "Images", "BlueMonster.png"));
            const int numEnemies = 8;
            enemyStridesRed = ImageStride.CreateStrides(2,
                Path.Combine("..","Galaga","Assets", "Images", "RedMonster.png"));
            enemyExplosions = new AnimationContainer(numEnemies);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("..","Galaga","Assets", "Images", "Explosion.png"));
            
            // Instanciating enemy squadrons
            squad1 = new Squadron1();
            squad2 = new Squadron2();
            squad3 = new Squadron3();
            squad3.CreateEnemies(images, enemyStridesRed);
            enemies = squad3.Enemies;
            DifficultyMultiplier = 1.0f;
            enemiesLeft = numEnemies;

            // Instanciating Movement Strategies
            NoMove = new MovementNoMove();
            MoveDown = new MovementDown();
            ZigZag = new MovementZigZagDown();
            currMovementStrategy = NoMove;                

            //instanciate score
            score = new Score(new Vec2F(0.1f,-0.2f),new Vec2F(0.3f,0.3f));

            //setting game over
            isGameOver = false;
        }

        public void Reset() {
            GameRunning.instance = new GameRunning();
        }

        public void GameOver() {
            isGameOver = true;
        }

        private void NewRound() {
            DifficultyMultiplier += 0.2f;
            switch(new System.Random().Next(3))  {
                case 0:
                    squad1.CreateEnemies(images, enemyStridesRed);
                    enemies = squad1.Enemies;
                    break;
                case 1:
                    squad2.CreateEnemies(images, enemyStridesRed);
                    enemies = squad2.Enemies;
                    break;
                case 2:
                    squad3.CreateEnemies(images, enemyStridesRed);
                    enemies = squad3.Enemies;
                    break;
            }
            enemiesLeft = 8;
            switch(new System.Random().Next(2)) {
                case 0:
                    currMovementStrategy = MoveDown;
                    MoveDown.GetDiffMult(DifficultyMultiplier);
                    break;
                    
                case 1:
                    currMovementStrategy = ZigZag;
                    ZigZag.GetDiffMult(DifficultyMultiplier);
                    break;
            }

        }

        private void IterateShots() {
            playerShots.Iterate(shot => {
            // Moves the shot's shape
                shot.Shape.Move(0.0f,0.1f);
                if (shot.Shape.Position.Y > 1.0f/* guard against window borders */ ) {
                // Deletes shot
                    shot.DeleteEntity();
                } 
                else {
                    // If collision btw shot and enemy -> delete both
                    enemies.Iterate(enemy => {  
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),enemy.Shape).Collision) {
                            shot.DeleteEntity();
                            enemy.Hit();                            
                            if (enemy.hitPoints < 1) {
                                enemy.DeleteEntity(); 
                                score.AddPoint();
                                AddExplosion(enemy.Shape.Position, enemy.Shape.Extent);
                                enemiesLeft--;
                                if (enemiesLeft < 1) {
                                    NewRound();
                                }
                            }
                        }
                    });
                }
            });
        }

        public void AddExplosion(Vec2F position, Vec2F extent) {
        // Adds explosion to the AnimationContainer 
            enemyExplosions.AddAnimation(new StationaryShape(position, extent), EXPLOSION_LENGTH_MS, 
                new ImageStride(EXPLOSION_LENGTH_MS/8, explosionStrides));
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

        }
        /// <summary>
        /// Update all variables that are being used by this GameState.
        /// </summary>
        public void UpdateGameLogic() {
            player.Move();
            currMovementStrategy.MoveEnemies(enemies);
        }
        /// <summary>
        /// Render all entities in this GameState
        /// </summary>
        public void RenderState() {
            if (!isGameOver) {    
                player.Render();
                enemies.RenderEntities();
                IterateShots();
                playerShots.RenderEntities();
                enemyExplosions.RenderAnimations();
            }
            score.RenderScore();
        }
        /// <summary>
        /// Each state can react to key events, delegated from the host StateMachine.
        /// </summary>
        /// <param name="keyAction">Either "KEY_PRESS" or "KEY_RELEASE".</param>
        /// <param name="keyValue">The string key value (see DIKUArcade.Input.KeyTransformer
        /// for details).</param>
        public void HandleKeyEvent(string keyValue, string keyAction){
            switch(keyAction) {
                case "KEY_RELEASE":
                    switch(keyValue) {
                        case "KEY_SPACE":
                            playerShots.AddEntity(new PlayerShot(player.GetPosition(), playerShotImage));
                            break;
                        case "KEY_ESCAPE":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE", //<- message
                                    "GAME_PAUSED", //<- parameter1
                                    ""));
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