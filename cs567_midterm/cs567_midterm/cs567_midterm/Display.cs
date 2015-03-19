using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace cs567_midterm
{
    internal class Display
    {
        private Texture2D backGround;
        private Texture2D walkway;
        private float backGroundScale = 2.5f;
        private float backGroundScroll = .25f;
        public Vector2 walkwayScale = new Vector2(1f, 1f);
        public Vector2 walkwayPosition = new Vector2(0, 300);
        private Game1 game;
        private SpriteFont score;

        public Display(Game1 _game)
        {
            game = _game;
            LoadContent();
        }

        public void LoadContent()
        {
            backGround = game.Content.Load<Texture2D>(@"Images/Background");
            walkway = game.Content.Load<Texture2D>(@"Images/bridge");
            score = game.Content.Load<SpriteFont>("SpriteFontMain");
        }

        public void DisplayBackGround(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0),
                new Rectangle((int)Math.Round(cameraPosition.X * backGroundScroll / backGroundScale),
                    0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, backGroundScale,
                    SpriteEffects.None, 0);
            spriteBatch.Draw(walkway, new Vector2(cameraPosition.X, walkwayPosition.Y / walkwayScale.Y),
                new Rectangle((int)Math.Round(cameraPosition.X * 1.0 / backGroundScale),
                    0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, walkwayScale,
                    SpriteEffects.None, 0);
        }

        public void DisplayScore(SpriteBatch spriteBatch, Vector2 cameraPosition, int totalScore)
        {
            string playerScore;
            playerScore = "Player Score:  " + totalScore;
            spriteBatch.DrawString(score, playerScore, new Vector2(cameraPosition.X, 10), Color.White);
        }
    }
}