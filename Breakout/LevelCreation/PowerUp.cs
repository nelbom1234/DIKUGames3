using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.IO;
using System;
using Breakout.Utilities;

namespace Breakout.LevelCreation {
    public class PowerUp : Entity{

        public int powerUp;

        //Not a great solution to have image in constructor since we change it to something else,
        //however since isDeleted is set to private rather than protected I do not have access
        //to it from here and therefore could never set it to false, which will create
        //issues when we later make an entityContainer for powerups and try to delete powerups from it
        public PowerUp(Shape shape, IBaseImage image) : base(shape, image) {
            Shape.AsDynamicShape().Direction.Y = -0.01f;
            powerUp = new Random().Next(5);
            switch(powerUp) {
                case 0:
                    //extra life
                    Image = new Image(FileCollector.FilePath(
                        Path.Combine("Assets","Images","LifePickUp.png")));
                    break;
                case 1:
                    //extra ball
                    Image = new Image(FileCollector.FilePath(
                        Path.Combine("Assets","Images","ExtraBallPowerUp.png")));
                    break;
                case 2:
                    //Wide
                    Image = new Image(FileCollector.FilePath(
                        Path.Combine("Assets","Images","WidePowerUp.png")));
                    break;
                case 3:
                    //double speed
                    Image = new Image(FileCollector.FilePath(
                        Path.Combine("Assets","Images","DoubleSpeedPowerUp.png")));
                    break;
                case 4:
                    //half speed
                    Image = new Image(FileCollector.FilePath(
                        Path.Combine("Assets","Images","HalfSpeedPowerUp.png")));
                    break;
            }

        }

        public void Move() {
            Shape.Move();
            if (Shape.Position.Y < 0.0f) {
                this.DeleteEntity();
            }
        }
    }
}