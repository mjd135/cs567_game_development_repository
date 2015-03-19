using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cs567_midterm
{
    internal class Player
    {
        private Texture2D sprite;
        private Vector2 position;
        private Point spriteFirstFramePosition;
        private Point spriteCurrentFramePosition;
        private Point spriteFrameSize;
        private Point spriteSheetSize;
        private Vector2 origin;
        private Vector2 jumpHeight = new Vector2(0, 100);
        private int spriteFrames;
        private int spriteCurrentFrame = 0;
        private float spriteScale;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;
        private State spriteCurrentState = State.Walking;
        private Vector2 spriteStartingPosition = Vector2.Zero;
        private KeyboardState previousKeyboardState;
        private Vector2 spriteDirection = Vector2.Zero;
        private const int MOVE_UP = -1;
        private const int MOVE_DOWN = 1;
        private Vector2 jumpSpeed = Vector2.Zero;
        private const int spriteSpeed = 100;

        private enum State
        {
            Walking,
            Jumping
        }

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
            origin = position;
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
            position += spriteDirection * jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();

            UpdateJump(keyboardState);

            previousKeyboardState = keyboardState;

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

        private void UpdateJump(KeyboardState keyboardState)
        {
            if (spriteCurrentState == State.Walking)
            {
                if (keyboardState.IsKeyDown(Keys.Space) == true && previousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }

            if (spriteCurrentState == State.Jumping)
            {
                if (position.Y < 50)
                {
                    spriteDirection.Y = MOVE_DOWN;
                }

                if (position.Y > origin.Y)
                {
                    position.Y = origin.Y;

                    spriteCurrentState = State.Walking;

                    spriteDirection = Vector2.Zero;
                }
            }
        }

        private void Jump()
        {
            if (spriteCurrentState != State.Jumping)
            {
                spriteCurrentState = State.Jumping;

                origin = position;

                spriteDirection.Y = MOVE_UP;

                jumpSpeed = new Vector2(spriteSpeed, spriteSpeed);
            }
        }
    }
}