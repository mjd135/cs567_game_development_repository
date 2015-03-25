using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cs567_midterm
{
    internal class EnemyWeapon
    {
        private Vector2 position;
        private Texture2D sprite;

        private float updateCounter;
        private int updateRate;
        private int moveRate;
        public bool isAlive;

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public EnemyWeapon(Texture2D graphic, float x, float y, int moveRate)
        {
            position = new Vector2(x, y);
            sprite = graphic;
            updateCounter = 0;
            updateRate = 60;
            this.moveRate = moveRate;
            isAlive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public void Update(float elapsedTime)
        {
            updateCounter -= elapsedTime;
            if (updateCounter > 0)
                return;

            updateCounter += 1000f / updateRate;

            position.X -= moveRate;
        }
    }
}