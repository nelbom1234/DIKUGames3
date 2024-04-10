using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using Breakout.Utilities;



namespace Breakout.LevelCreation {
    public class PowerUpBlock : Block{

        public PowerUp powerUp;


        public PowerUpBlock(StationaryShape shape, IBaseImage image, IBaseImage damaged, char identifier) : base(shape, image, damaged, identifier) {
            powerUp = new PowerUp(new DynamicShape(Shape.Position, new Vec2F(0.07f, 0.07f)), 
            new Image(FileCollector.FilePath(
                Path.Combine("Assets","Images","LifePickUp.png"))));
        }

    }
}