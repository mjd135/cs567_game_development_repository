using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_midterm
{
    internal class UserControlledSprite : Sprite
    {
        private MouseState prevMouseState;
        private float time;
        private float delay;
        public const float MaxMovement = 5.0f;

        public override Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    
                    inputDirection.X -= 1;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                return inputDirection * speed;
            }
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFramePosition, Point currentFrame, Point sheetSize,
            Vector2 speed, string cueName, int numFrames, int frame)
            : base(textureImage, position, frameSize, collisionOffset, currentFramePosition, currentFrame,
                sheetSize, speed, cueName, numFrames, frame)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFramePosition, Point currentFrame, Point sheetSize,
            Vector2 speed, string cueName, int millisecondsPerFrame, int numFrames, int frame)
            : base(textureImage, position, frameSize, collisionOffset, currentFramePosition, currentFrame,
            sheetSize, speed, millisecondsPerFrame, cueName, numFrames, frame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds, SoundEffect soundEffect)
        {
            position += Direction;







            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds, soundEffect);
        }
    }
}
