using DIKUArcade.Entities;
using DIKUArcade.Graphics;



namespace Breakout.LevelCreation {
    public class HardenedBlock : Block{

        public HardenedBlock(StationaryShape shape, IBaseImage image, IBaseImage damaged, char identifier) : base(shape, image, damaged, identifier) {
            hp = 6;
            value = 2;
        }
    }
}