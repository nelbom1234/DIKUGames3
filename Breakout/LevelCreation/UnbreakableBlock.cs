using DIKUArcade.Entities;
using DIKUArcade.Graphics;



namespace Breakout.LevelCreation {
    public class UnbreakableBlock : Block{

        public UnbreakableBlock(StationaryShape shape, IBaseImage image, IBaseImage damaged, char identifier) : base(shape, image, damaged, identifier) {
        }
    }
}