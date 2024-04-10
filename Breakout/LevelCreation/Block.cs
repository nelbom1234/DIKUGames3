using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace Breakout.LevelCreation {
    public class Block : Entity {

        public StationaryShape shape;
        public IBaseImage damaged;

        public int hp{get; protected set;}
        public int value{get; protected set;}
        public char identifier{get; protected set;}

        public Block(StationaryShape shape, IBaseImage image, IBaseImage damaged, char identifier) : base(shape, image) {
            this.shape = shape;
            this.damaged = damaged;
            hp = 3;
            this.value = 1;
            this.identifier = identifier;
        }

        public void Damage() {
            Image = damaged;
        }

        public void Hit() {
            hp -= 1;
        }
    }
}