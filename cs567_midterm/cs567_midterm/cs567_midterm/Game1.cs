using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace cs567_midterm
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region sound

        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private SoundEffect soundEffect;
        private Cue trackCue;
        private Song themeSong;
        private bool songStart = false;

        #endregion sound

        #region background

        private Texture2D backGround;
        private Texture2D walkway;
        private float backGroundScale = 2.5f;
        private float backGroundScroll = .25f;
        public Vector2 walkwayScale = new Vector2(1f, 1f);
        public Vector2 walkwayPosition = new Vector2(0, 300);

        #endregion background

        #region samus

        private Texture2D samusTexture;
        private Player player;

        #endregion samus

        #region enemy

        private Enemy spacePirate;
        private Enemy metroid;
        private Texture2D pirateTexture;
        private Texture2D metroidTexture;

        #endregion enemy

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //private SpriteManager spriteManager;

        #region framerate and camerea

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;
        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 2.0f;

        #endregion framerate and camerea

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
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            spacePirate = new Enemy(pirateTexture, 300, 200, new Point(447, 2), new Point(0, 0), new Point(46, 66), new Point(1, 8), 8, 2.5f);
            metroid = new Enemy(metroidTexture, 300, 100, new Point(285, 4), new Point(0, 0), new Point(41, 47), new Point(1, 2), 2, 1.0f);
            player = new Player(samusTexture, 100, 220, new Point(240, 650), new Point(0, 0), new Point(48, 49), new Point(4, 3), 10, 3.0f);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            themeSong = Content.Load<Song>(@"Audio\Theme");
            backGround = Content.Load<Texture2D>(@"Images/Background");
            walkway = Content.Load<Texture2D>(@"Images/bridge");
            samusTexture = Content.Load<Texture2D>(@"Images/samus");
            pirateTexture = Content.Load<Texture2D>(@"Images/space pirates");
            metroidTexture = Content.Load<Texture2D>(@"Images/metroids");
            MediaPlayer.IsRepeating = true;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
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

            // TODO: Add your update logic here
            player.Update(gameTime, cameraPosition);
            spacePirate.Update();
            metroid.Update();

            if (!songStart)
            {
                MediaPlayer.Play(themeSong);
                songStart = true;
            }

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                cameraPosition.X += cameraSpeed;
                player.Update(gameTime, cameraPosition);
            }

            //KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.Update(gameTime, cameraPosition);

                cameraPosition.X -= cameraSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X += cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Space))
                player.Update(gameTime, cameraPosition);
            //if (keyboardState.IsKeyDown(Keys.Down))
            //cameraPosition.Y += cameraSpeed;

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
            spriteBatch.Begin(SpriteSortMode.Immediate,
             BlendState.AlphaBlend,
             SamplerState.PointWrap,
             DepthStencilState.Default,
             RasterizerState.CullCounterClockwise,
             null,
             screenMatrix);

            spriteBatch.Draw(backGround, new Vector2(cameraPosition.X, 0),
                new Rectangle((int)Math.Round(cameraPosition.X * backGroundScroll / backGroundScale),
                    0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, backGroundScale,
                    SpriteEffects.None, 0);
            spriteBatch.Draw(walkway, new Vector2(cameraPosition.X, walkwayPosition.Y / walkwayScale.Y),
                new Rectangle((int)Math.Round(cameraPosition.X * 1.0 / backGroundScale),
                    0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, walkwayScale,
                    SpriteEffects.None, 0);

            player.Draw(spriteBatch);
            spacePirate.Draw(spriteBatch);
            metroid.Draw(spriteBatch);

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }
    }
}