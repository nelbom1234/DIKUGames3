using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
namespace Galaga {
    public class Enemy : Entity {
        
        public IBaseImage enrage;

        public DynamicShape shape {get; private set;}

        public Vec2F startPosition{get; private set;}
        public bool isEnraged{get; private set;}

        public int hitPoints {get; private set;}
        public Enemy(DynamicShape shape, IBaseImage image, IBaseImage enraged) : base(shape, image) {
            hitPoints = 4;
            enrage = enraged;
            this.shape = shape;
            startPosition = shape.Position.Copy();
            isEnraged = false;
        }

        public void subHP(int val) {
            hitPoints -= val;
        }

        public void Enrage() {
            Image = enrage;
            isEnraged = true;
        }

        public void Hit() {
            this.subHP(1);
            if (hitPoints < 2) {
                    Enrage();
            }
        }
    }
}