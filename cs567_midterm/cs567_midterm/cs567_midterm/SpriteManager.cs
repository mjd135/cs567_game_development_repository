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
    internal class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        
        private UserControlledSprite player;
        private SoundEffect soundEffect;

        private List<Sprite> spriteList = new List<Sprite>();

        private float delay;
        private float time;
        private bool gameOver = false;

        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 2.0f;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);



            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/samus"),
                new Vector2(100,300), new Point(48, 49), 30, new Point(240, 650), new Point(0, 0),
                new Point(4, 3), new Vector2(6, 6), "Victory Against Metroid", 100, 10, 0);

            spriteList.Add(
                new PowerBeam(Game.Content.Load<Texture2D>(@"Images/powerBeam"),
                new Vector2(230, 210), new Point(23, 21), 5, new Point (0,0), new Point(0, 0),
                new Point(1, 1), Vector2.One, null, this, 1, 0));

            //spriteList.Add(
            //    new ChasingSprite(Game.Content.Load<Texture2D>(@"Images/WolfRunning"),
            //    new Vector2(250, 250), new Point(111, 56), 10, new Point(0, 0),
            //    new Point(6, 1), Vector2.One, "Boss5", this, 6, 0));

            //soundEffect = Game.Content.Load<SoundEffect>(@"Audio\Running");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds, soundEffect);

            for (int i = 0; i < spriteList.Count; i++)
            {
                Sprite s = spriteList[i];
                s.Update(gameTime, Game.Window.ClientBounds, null);

                if (s.CollisionRect.Intersects(player.CollisionRect))
                {
                    ((Game1)Game).PlayCue(s.cueName);
                    spriteList.RemoveAt(i);
                    i--;
                }
                if (spriteList.Count == 0)
                    gameOver = true;
            }
            if (gameOver == true)
            {
                delay = 1f;
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > delay)
                {
                    ((Game1)Game).PlayCue(player.cueName);
                    gameOver = false;
                }
            }





            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            player.Draw(gameTime, spriteBatch);
            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }
    }
}
