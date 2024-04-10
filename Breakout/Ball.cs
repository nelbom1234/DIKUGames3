using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using System.Collections.Generic;
using System;

namespace Breakout {
    public class Ball : Entity {
        public bool hasLaunched;
        public bool doubleSpeed;
        public bool halfSpeed;

        public DynamicShape shape{get; private set;}
        public Ball(DynamicShape shape, IBaseImage image) : base(shape, image){
            this.shape = shape;
            hasLaunched = false;
            doubleSpeed = false;
            halfSpeed = false;
        }

        public void Launch() {
            if (!hasLaunched) {    
                hasLaunched = true;
                var rand = new Random();
                shape.Direction = new Vec2F(((float)rand.Next(-5,6))/1000f, 0.01f);
            }
        }

        public void DoubleSpeed(bool val) {
            if (doubleSpeed != val) {
                doubleSpeed = val;
                if (val) {
                    shape.Direction = shape.Direction*2.0f;
                    
                }
                else {
                    shape.Direction = shape.Direction/2.0f;
                }
            }
        }

        public void HalfSpeed(bool val) {
            if (halfSpeed != val) {
                halfSpeed = val;
                if (val) {
                    shape.Direction = shape.Direction/2.0f;
                }
                else {
                    shape.Direction = shape.Direction*2.0f;
                }
            }
        }
        public void Move() {
            shape.Move();
            if (shape.Position.X+shape.Extent.X > 1.0f) {
                shape.Direction.X = -shape.Direction.X;
            }
            else if (shape.Position.X < 0.0f) {
                shape.Direction.X = -shape.Direction.X;
            }
            if (shape.Position.Y+shape.Extent.Y > 1.0f) {
                shape.Direction.Y = -shape.Direction.Y;
            }
            if (shape.Position.Y < 0.0f) {
                this.DeleteEntity();
            }
        }
    }
}