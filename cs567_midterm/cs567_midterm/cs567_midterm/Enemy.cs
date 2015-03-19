using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cs567_midterm
{
    internal class Enemy
    {
        private Texture2D sprite;
        private Vector2 position;
        private Point spriteFirstFramePosition;
        private Point spriteCurrentFramePosition;
        private Point spriteFrameSize;
        private Point spriteSheetSize;
        private int spriteFrames;
        private int spriteCurrentFrame = 0;
        private float spriteScale;

        private float updateCounter;
        private int updateRate;
        private int moveRate;

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Enemy(Texture2D graphic, float x, float y, Point firstFrame, Point currentFrame, Point frameSize, Point sheetSize, int frames, float scale)
        {
            sprite = graphic;
            position = new Vector2(x, y);
            spriteFirstFramePosition = firstFrame;
            spriteCurrentFramePosition = currentFrame;
            spriteFrameSize = frameSize;
            spriteSheetSize = sheetSize;
            spriteFrames = frames;
            spriteScale = scale;
            moveRate = 1;
            updateCounter = 0;
            updateRate = 60;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position,
                new Rectangle(spriteFirstFramePosition.X + spriteCurrentFramePosition.X * spriteFrameSize.X,
                spriteFirstFramePosition.Y + spriteCurrentFramePosition.Y * spriteFrameSize.Y, spriteFrameSize.X, spriteFrameSize.Y),
                Color.White, 0, Vector2.Zero, spriteScale, SpriteEffects.FlipHorizontally, 0f);
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