using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_midterm
{
    internal class ChasingSprite : Sprite
    {
        private SpriteManager spriteManager;

        public ChasingSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, string cueName,
            SpriteManager spriteManager, int numFrames, int frame)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, cueName, numFrames, frame)
        {
            this.spriteManager = spriteManager;
        }

        public override Vector2 Direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds, SoundEffect soundEffect)
        {
            Vector2 player = spriteManager.GetPlayerPosition();

            float speedVal = Math.Max(
                Math.Abs(speed.X), Math.Abs(speed.Y));

            if (player.X < position.X)
                position.X -= speedVal;
            else if (player.X > position.X)
                position.X += speedVal;

            if (player.Y < position.Y)
                position.Y -= speedVal;
            else if (player.Y > position.Y)
                position.Y += speedVal;

            base.Update(gameTime, clientBounds, soundEffect);
        }
    }
}
