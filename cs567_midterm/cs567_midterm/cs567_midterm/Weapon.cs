using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_midterm
{
    class Weapon
    {
        Vector2 position;
        Texture2D sprite;

        float updateCounter;
        int updateRate;
        int moveRate;
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
        public Weapon(Texture2D graphic, float x, float y)
        {
            position = new Vector2(x, y);
            sprite = graphic;
            updateCounter = 0;
            updateRate = 60;
            moveRate = 20;
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

            position.X += moveRate;
        }
    }
}
