using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cs567_assn3
{
    class ChasingSprite : Sprite
    {
        SpriteManager spriteManager;

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

        public override void Update(GameTime gameTime, Rectangle clientBounds)
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

            base.Update(gameTime, clientBounds);
        }
    }
}
