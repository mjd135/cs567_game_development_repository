using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace cs567_assn3
{
    internal class AutomatedSprite : Sprite
    {
        public override Vector2 Direction
        {
            get { return speed; }
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, string collisionCueName, int numFrames, int frame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, collisionCueName, numFrames, frame)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, string collisionCueName, int numFrames, int frame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, collisionCueName, numFrames, frame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds, SoundEffect soundEffect)
        {
            position += Direction;

            base.Update(gameTime, clientBounds, soundEffect);
        }
    }
}