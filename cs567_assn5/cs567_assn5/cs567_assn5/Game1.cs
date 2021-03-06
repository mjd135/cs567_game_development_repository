using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace cs567_assn5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private enum Type { walkway, Player, Enemy, PowerBeam };
        private bool moving;
        private bool jump;
        private bool shoot = false;
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private SoundEffect soundEffect;
        private Cue trackCue;
        private Song themeSong;



        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 2.0f;

        private Texture2D backGround;
        private Texture2D walkway;
        private float backGroundScale = 2.0f;
        private float backGroundScale1 = 2.5f;
        private float layer0Scroll = .25f;
        private float layer2Scroll = 3.0f;

        public Vector2 walkwayPosition { get; set; }

        public Body walkwayBody { get; set; }

        public Rectangle walkwayRectangle { get; set; }

        public Vector2 walkwayScale { get; set; }

        private Texture2D powerBeam;
        private Texture2D pirate;
        private Texture2D samus;

        public Vector2 PlayerPosition { get; set; }

        public Body PlayerBody { get; set; }

        public Rectangle PlayerRectangle { get; set; }

        public Vector2 PlayerScale { get; set; }

        public Vector2 EnemyPosition { get; set; }

        public Body EnemyBody { get; set; }

        public Rectangle EnemyRectangle { get; set; }

        public Vector2 EnemyScale { get; set; }

        public Vector2 PowerBeamPosition { get; set; }

        public Body PowerBeamBody { get; set; }

        public Rectangle PowerBeamRectangle { get; set; }

        public Vector2 PowerBeamScale { get; set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private World world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        ///

        private Vertices GetBounds()
        {
            float width = ConvertUnits.ToSimUnits(this.GraphicsDevice.Viewport.Width);
            float height = ConvertUnits.ToSimUnits(this.GraphicsDevice.Viewport.Height);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return bounds;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //spriteManager = new SpriteManager(this);
            //Components.Add(spriteManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            world = new World(new Vector2(0, 9.4f));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            samus = Content.Load<Texture2D>(@"Images/SamusRunning");
            pirate = Content.Load<Texture2D>(@"Images/spacePirate");
            powerBeam = Content.Load<Texture2D>(@"Images/powerBeam");
            //var bounds = GetBounds();
            //var boundary = BodyFactory.CreateLoopShape(world, bounds);

            PlayerRectangle = new Rectangle(0, 0, 90, 78);
            PlayerScale = new Vector2(1.0f, 1.0f);
            PlayerBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(PlayerRectangle.Width * PlayerScale.X), ConvertUnits.ToSimUnits(PlayerRectangle.Height * PlayerScale.Y), 1);
            PlayerPosition = new Vector2(300, 100);
            PlayerBody.Position = new Vector2(ConvertUnits.ToSimUnits(PlayerPosition.X), ConvertUnits.ToSimUnits(PlayerPosition.Y));
            PlayerBody.BodyType = BodyType.Dynamic;
            PlayerBody.OnCollision += OnCollision; //registers the OnCollision method to the OnCollision delegate
            PlayerBody.UserData = Type.Player;
            PlayerBody.Friction = 1.0f;

            EnemyRectangle = new Rectangle(0, 0, 41, 52);
            EnemyScale = new Vector2(1.5f, 1.5f);
            EnemyBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(EnemyRectangle.Width * EnemyScale.X), ConvertUnits.ToSimUnits(EnemyRectangle.Height * EnemyScale.Y), 1);
            EnemyPosition = new Vector2(700, 100);
            EnemyBody.Position = new Vector2(ConvertUnits.ToSimUnits(EnemyPosition.X), ConvertUnits.ToSimUnits(EnemyPosition.Y));
            EnemyBody.BodyType = BodyType.Dynamic;
            EnemyBody.OnCollision += OnCollision; //registers the OnCollision method to the OnCollision delegate
            EnemyBody.UserData = Type.Enemy;
            EnemyBody.Friction = 1.0f;

            PowerBeamRectangle = new Rectangle(8, 5, 14, 14);
            PowerBeamScale = new Vector2(1.5f, 1.5f);
            PowerBeamBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(PowerBeamRectangle.Width * PowerBeamScale.X), ConvertUnits.ToSimUnits(PowerBeamRectangle.Height * PowerBeamScale.Y), 1);
            PowerBeamPosition = new Vector2(360, 300);
            PowerBeamBody.Position = new Vector2(ConvertUnits.ToSimUnits(PowerBeamPosition.X), ConvertUnits.ToSimUnits(PowerBeamPosition.Y));
            PowerBeamBody.BodyType = BodyType.Dynamic;
            PowerBeamBody.OnCollision += OnCollision; //registers the OnCollision method to the OnCollision delegate
            PowerBeamBody.UserData = Type.PowerBeam;
            PowerBeamBody.Friction = 0.0f;
            PowerBeamBody.Awake = false;
            PowerBeamBody.IgnoreGravity = true;

            backGround = Content.Load<Texture2D>(@"Images/Background");

            walkway = Content.Load<Texture2D>(@"Images/Walkway");

            walkwayScale = new Vector2(2.0f, 2.0f);
            walkwayRectangle = new Rectangle(17, 36, 779, 92);
            walkwayBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(walkwayRectangle.Width * walkwayScale.X), ConvertUnits.ToSimUnits(walkwayRectangle.Height * walkwayScale.Y), 1);
            walkwayPosition = new Vector2(250, 450);
            walkwayBody.Position = new Vector2(ConvertUnits.ToSimUnits(walkwayPosition.X), ConvertUnits.ToSimUnits(walkwayPosition.Y));
            walkwayBody.BodyType = BodyType.Static;
            walkwayBody.OnCollision += OnCollision;
            walkwayBody.UserData = Type.walkway;

            ;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        ///

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                //PlayerBody.ApplyLinearImpulse(new Vector2(-0.15f, 0));
                cameraPosition.X -= cameraSpeed;

                
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                PlayerBody.ApplyLinearImpulse(new Vector2(0.15f, 0));
                cameraPosition.X += cameraSpeed;
                
            }
            if (keyboardState.IsKeyDown(Keys.Up))
                jump = true;

            if (keyboardState.IsKeyDown(Keys.Down))
                cameraPosition.Y = cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                shoot = true;
            }

            if (shoot == true)
            {
                PowerBeamPosition = new Vector2(ConvertUnits.ToDisplayUnits(PowerBeamBody.Position.X),
                    ConvertUnits.ToDisplayUnits(PowerBeamBody.Position.Y));
                PowerBeamBody.Awake = true;
                PowerBeamBody.ApplyForce(new Vector2(1.0f, 0f));
            }

            if (jump == true)
            {
                if (keyboardState.IsKeyUp(Keys.Up))
                {
                    PlayerBody.ApplyLinearImpulse(new Vector2(0, -3.5f));
                    jump = false;
                }
            }



            PlayerPosition = new Vector2(ConvertUnits.ToDisplayUnits(PlayerBody.Position.X),
               ConvertUnits.ToDisplayUnits(PlayerBody.Position.Y));
            EnemyPosition = new Vector2(ConvertUnits.ToDisplayUnits(EnemyBody.Position.X),
                ConvertUnits.ToDisplayUnits(EnemyBody.Position.Y));

            walkwayPosition = new Vector2(ConvertUnits.ToDisplayUnits(walkwayBody.Position.X),
                ConvertUnits.ToDisplayUnits(walkwayBody.Position.Y));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix screenMatrix = Matrix.CreateTranslation(new Vector3(-cameraPosition, 0));
            // spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            spriteBatch.Begin(SpriteSortMode.Immediate,
             BlendState.AlphaBlend,
             SamplerState.PointWrap,
             DepthStencilState.Default,
             RasterizerState.CullCounterClockwise,
             null,
             screenMatrix);

            spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0),
                new Rectangle((int)Math.Round(cameraPosition.X * layer0Scroll / backGroundScale1),
                    0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, backGroundScale1,
                    SpriteEffects.None, 0);

            //first layer

            spriteBatch.Draw(walkway, walkwayPosition, walkwayRectangle, Color.White,
                walkwayBody.Rotation, new Vector2(walkwayRectangle.Width / 2f, walkwayRectangle.Height / 2f),
                walkwayScale, SpriteEffects.None, 0f);

            spriteBatch.Draw(samus, PlayerPosition, PlayerRectangle, Color.White, PlayerBody.Rotation, new Vector2(PlayerRectangle.Width / 2f, PlayerRectangle.Height / 2f), PlayerScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(pirate, EnemyPosition, EnemyRectangle, Color.White, EnemyBody.Rotation, new Vector2(EnemyRectangle.Width / 2f, EnemyRectangle.Height / 2f), EnemyScale, SpriteEffects.None, 0f);

            if (shoot == true)
            {
                spriteBatch.Draw(powerBeam, PowerBeamPosition, PowerBeamRectangle, Color.White, PowerBeamBody.Rotation, new Vector2(PowerBeamRectangle.Width / 2f, PowerBeamRectangle.Height / 2f), PowerBeamScale, SpriteEffects.None, 0f);
            }

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!((fixtureA.Body.UserData is Type) &&
                (fixtureB.Body.UserData is Type)))
                return true;
            Type colliderType = (Type)(fixtureA.Body.UserData);
            Type collidedType = (Type)(fixtureB.Body.UserData);
            if (colliderType == Type.Player)
                switch (collidedType)
                {
                    case Type.walkway:
                        return true;

                    case Type.Enemy:
                        return false;

                    default:
                        return true;
                }
            //return true;

            if (colliderType == Type.Enemy)
                switch (collidedType)
                {
                    case Type.PowerBeam:
                        shoot = false;
                        EnemyBody.ApplyForce(new Vector2(15.0f, 0f));

                        return false;
                }
            return true;
        }


    }
}