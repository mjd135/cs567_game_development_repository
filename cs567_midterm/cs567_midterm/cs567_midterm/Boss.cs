using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cs567_midterm
{
    internal class Boss
    {
        private Texture2D sprite;
        private Vector2 position;
        private Point spriteFirstFramePosition;
        private Point spriteCurrentFramePosition;
        private Point spriteFrameSize;
        private Point spriteSheetSize;
        private int spriteFrames;
        private int spriteCurrentFrame = 6;
        private float spriteScale;
        private int millisecondsPerFrame = 100;
        private float spriteTimeSinceLastFrame;
        private float updateCounter;
        private int updateRate;
        public int bossLife;
        public bool isAlive;

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)position.X + 20, (int)position.Y, spriteFrameSize.X, spriteFrameSize.Y);
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Boss(Texture2D graphic, float x, float y, Point firstFrame, Point currentFrame, Point frameSize, Point sheetSize, int frames, float scale)
        {
            sprite = graphic;
            position = new Vector2(x, y);
            spriteFirstFramePosition = firstFrame;
            spriteCurrentFramePosition = currentFrame;
            spriteFrameSize = frameSize;
            spriteSheetSize = sheetSize;
            spriteFrames = frames;
            spriteScale = scale;

            updateCounter = 0;
            updateRate = 60;
            bossLife = 30;
            isAlive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position,
                new Rectangle(spriteFirstFramePosition.X + spriteCurrentFramePosition.X * spriteFrameSize.X,
                spriteFirstFramePosition.Y + spriteCurrentFramePosition.Y * spriteFrameSize.Y, spriteFrameSize.X, spriteFrameSize.Y),
                Color.White, 0, Vector2.Zero, spriteScale, SpriteEffects.None, 0f);
        }

        public void Update(float elapsedTime)
        {
            updateCounter -= elapsedTime;
            if (updateCounter > 0)
                return;

            updateCounter += 1000f / updateRate;

            spriteTimeSinceLastFrame += elapsedTime;
            if (spriteTimeSinceLastFrame > millisecondsPerFrame)
            {
                spriteCurrentFrame--;
                spriteCurrentFramePosition.X--;
                spriteTimeSinceLastFrame = 0;
                if (spriteCurrentFrame == 0)
                {
                    spriteCurrentFramePosition.X = 6;
                    spriteCurrentFramePosition.Y = 0;
                    spriteCurrentFrame = 6;
                }
                else if (spriteCurrentFramePosition.X >= spriteSheetSize.X)
                {
                    spriteCurrentFramePosition.X = 6;
                    spriteCurrentFrame--;
                    ++spriteCurrentFramePosition.Y;
                    if (spriteCurrentFramePosition.Y >= spriteSheetSize.Y)
                    {
                        spriteCurrentFramePosition.Y = 6;
                        spriteCurrentFrame = 6;
                    }
                }
            }
        }
    }
}