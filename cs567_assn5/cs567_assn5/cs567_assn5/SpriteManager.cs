using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace cs567_assn5
{
    internal class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        enum Type { Player, Enemy };
        private SpriteBatch spriteBatch;

        private UserControlledSprite player;
        Texture2D texture;
        public Vector2 PlayerPosition { get; set; }
        public Body PlayerBody { get; set; }
        public Rectangle PlayerRectangle { get; set; }
        public Vector2 PlayerScale { get; set; }

        private SoundEffect soundEffect;

        private List<Sprite> spriteList = new List<Sprite>();

        private float delay;
        private float time;
        private bool gameOver = false;

        World world;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            world = new World(new Vector2(0, 9.8f));

            PlayerRectangle = new Rectangle(0, 0, 90, 90);
            PlayerScale = new Vector2(1.0f, 1.0f);
            PlayerBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(PlayerRectangle.Width * PlayerScale.X), ConvertUnits.ToSimUnits(PlayerRectangle.Height * PlayerScale.Y), 1);
            PlayerPosition = new Vector2(300, 100);
            PlayerBody.Position = new Vector2(ConvertUnits.ToSimUnits(PlayerPosition.X), ConvertUnits.ToSimUnits(PlayerPosition.Y));
            PlayerBody.BodyType = BodyType.Dynamic;
            //PlayerBody.OnCollision += OnCollision; //registers the OnCollision method to the OnCollision delegate 
            PlayerBody.UserData = Type.Player;
            PlayerBody.Friction = 0.0f;

            //player = new UserControlledSprite(
            //    Game.Content.Load<Texture2D>(@"Images/SamusRunning"),
            //    Vector2.Zero, new Point(90, 90), 30, new Point(0, 0),
            //    new Point(4, 3), new Vector2(1, 1), "Victory Against Metroid", 100, 10, 0);
            texture = Game.Content.Load<Texture2D>(@"Images/SamusRunning");
            //player = new UserControlledSprite(
            //    Game.Content.Load<Texture2D>(@"Images/SamusRunning"),
            //    Vector2.Zero, new Point(90, 90), 30, new Point(0, 0),
            //    new Point(4, 3), new Vector2(1, 1), "Victory Against Metroid", 100, 10, 0);

            //spriteList.Add(
            //    new ChasingSprite(Game.Content.Load<Texture2D>(@"Images/WolfRunning"),
            //    new Vector2(250, 250), new Point(111, 56), 10, new Point(0, 0),
            //    new Point(6, 1), Vector2.One, "Boss5", this, 6, 0));

            //soundEffect = Game.Content.Load<SoundEffect>(@"Audio\Running");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //player.Update(gameTime, Game.Window.ClientBounds, soundEffect);

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
            spriteBatch.Draw(texture, PlayerPosition, PlayerRectangle, Color.White, PlayerBody.Rotation, new Vector2(PlayerRectangle.Width / 2f, PlayerRectangle.Height / 2f), PlayerScale, SpriteEffects.None, 1f);
           // player.Draw(gameTime, spriteBatch);
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

        //public override bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        //{
        //    if (!((fixtureA.Body.UserData is Type) &&
        //        (fixtureB.Body.UserData is Type))) //verify UserData is a Type for both fixtures
        //        return true;
        //    Type colliderType = (Type)(fixtureA.Body.UserData);
        //    Type collidedType = (Type)(fixtureB.Body.UserData);
        //    if (colliderType == Type.Player) //just checking for collisions by a puck
        //        return false;

        //    return true;
        //}
    }
}