using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
namespace Galaga {
    public class Player : IGameEventProcessor<object> {
        // Add private fields
        private Entity entity;
        public DynamicShape shape {get; private set;}
        private float moveLeft{get; set;}
        private float moveRight{get; set;}
        private const float MOVEMENT_SPEED = 0.01f;
        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
            moveLeft = 0.0f;
            moveRight = 0.0f;
        }

        public void KeyPress(string key) {
            // switch on key string and set the player's move direction
            switch(key) {
                case "KEY_LEFT":
                    SetMoveLeft(true);
                    break;
                case "KEY_RIGHT":
                    SetMoveRight(true);
                    break;
            }
        }

        public void KeyRelease(string key) {
            // switch on key string and disable the player's move direction
            switch(key) {
                case "KEY_LEFT":
                    SetMoveLeft(false);
                    break;
                case "KEY_RIGHT":
                    SetMoveRight(false);
                    break;
            }
        }

        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.InputEvent)
                switch (gameEvent.Parameter1) {
                    case "KEY_PRESS":
                        KeyPress(gameEvent.Message);
                        break;
                    case "KEY_RELEASE":
                        KeyRelease(gameEvent.Message);
                        break;
                    default:
                        break;
                }
        }

        public void Render() {
        // render the player entity
            entity.RenderEntity();
        } 

        public void Move() {
        // move the shape and guard against the window borders
            shape.Move();
            if (shape.Position.X > 0.9f) {
                shape.Position.X = 0.9f;
            }
            
            else if (shape.Position.X < 0.0f) {
                shape.Position.X = 0.0f;
            }
        }

        private void SetMoveLeft(bool val) {
        // set moveLeft appropriately and call UpdateMovement()
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            }
            else {
                moveLeft = 0.0f;
            }
            UpdateDirection();
        }
        
        public Vec2F GetPosition(){
            return shape.Position;
        }
        
        private void SetMoveRight(bool val) {
        // set moveRight appropriately and call UpdateMovement()
            if (val){
                moveRight = MOVEMENT_SPEED;
            }
            else {moveRight = 0.0f;
            }
            UpdateDirection();
        }

        public void UpdateDirection() {
            shape.Direction.X = moveLeft + moveRight;
        }
    }
}
