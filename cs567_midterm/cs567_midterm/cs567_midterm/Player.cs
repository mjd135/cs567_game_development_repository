using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_midterm
{
    class Player
    {
        Texture2D sprite;
        Vector2 position;
        Point spriteFirstFramePosition;
        Point spriteCurrentFramePosition;
        Point spriteFrameSize;
        Point spriteSheetSize;
        int spriteFrames;
        int spriteCurrentFrame = 0;
        float spriteScale;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;
        

        public Player(Texture2D graphic, float x, float y, Point firstFrame, Point currentFrame, Point frameSize, Point sheetSize, int frames, float scale)
        {
            sprite = graphic;
            position = new Vector2(x, y);
            spriteFirstFramePosition = firstFrame;
            spriteCurrentFramePosition = currentFrame;
            spriteFrameSize = frameSize;
            spriteSheetSize = sheetSize;
            spriteFrames = frames;
            spriteScale = scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position,
                new Rectangle(spriteFirstFramePosition.X + spriteCurrentFramePosition.X * spriteFrameSize.X,
                spriteFirstFramePosition.Y + spriteCurrentFramePosition.Y * spriteFrameSize.Y, spriteFrameSize.X, spriteFrameSize.Y),
                Color.White, 0, Vector2.Zero, spriteScale, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime, Vector2 cameraPosition)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            position.X = cameraPosition.X + 100;
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    spriteCurrentFrame++;
                    spriteCurrentFramePosition.X++;
                    timeSinceLastFrame = 0;
                    if (spriteCurrentFrame >= spriteFrames)
                    {
                        spriteCurrentFramePosition.X = 0;
                        spriteCurrentFramePosition.Y = 0;
                        spriteCurrentFrame = 1;
                    }
                    else if (spriteCurrentFramePosition.X >= spriteSheetSize.X)
                    {
                        spriteCurrentFramePosition.X = 0;
                        spriteCurrentFrame++;
                        ++spriteCurrentFramePosition.Y;
                        if (spriteCurrentFramePosition.Y >= spriteSheetSize.Y)
                        {
                            spriteCurrentFramePosition.Y = 0;
                            spriteCurrentFrame = 0;
                        }
                    }
                }

            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
              
            }

            
        }

    }
}
