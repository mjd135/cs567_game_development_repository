using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs567_assn3
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        UserControlledSprite player;

        List<Sprite> spriteList = new List<Sprite>();

        public SpriteManager(Game game)
            :base(game)
        {

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/SamusRunning"),
                Vector2.Zero, new Point(90, 90), 30, new Point (0,0),
                new Point(4,3), new Vector2(6, 6),"Running",100, 10, 0);

            spriteList.Add(
                new ChasingSprite(Game.Content.Load<Texture2D>(@"Images/WolfRunning"),
                new Vector2(250, 250), new Point(111, 56), 10, new Point(0, 0),
                new Point(6,1), Vector2.One, "Boss5", this, 6, 0));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);

            foreach(Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                
                if (s.CollisionRect.Intersects(player.CollisionRect))
                    ((Game1)Game).PlayCue(s.cueName);
                    //Game.Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            player.Draw(gameTime, spriteBatch);
            foreach(Sprite s in spriteList)
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
