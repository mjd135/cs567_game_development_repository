using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_midterm
{
    internal abstract class Sprite
    {
        //Sprite Draw
        private Texture2D textureImage;

        protected Point frameSize;
        private Point currentFrame;
        private Point sheetSize;
        private int numFrames;
        private int frame;

        //Collision
        private int collisionOffset;

        //Framerate
        private int timeSinceLastFrame = 0;

        private int millisecondsPerFrame;
        private const int defaultMillisecondsPerFrame = 16;

        //Movement
        protected Vector2 speed;

        protected Vector2 position;

        //Abstract definition of direction property
        public abstract Vector2 Direction
        {
            get;
        }

        //Collision Cue Name
        public string cueName { get; set; }

        //Constructors
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            string cueName, int numFrames, int frame)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, cueName, numFrames, frame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, string cueName, int numFrames, int frame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.cueName = cueName;
            this.numFrames = numFrames;
            this.frame = frame;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds, SoundEffect soundEffect)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                frame++;
                currentFrame.X++;
                timeSinceLastFrame = 0;
                if (frame >= numFrames)
                {
                    currentFrame.X = 0;
                    currentFrame.Y = 0;
                    frame = 1;
                }
                else if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    frame++;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                        frame = 0;
                    }
                }
            }
        }

        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public Vector2 GetPosition
        {
            get { return position; }
        }
    }
}
