using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;

namespace cs567_assn5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private SoundEffect soundEffect;
        private Cue trackCue;
        private Song themeSong;

        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 1.0f;

        private Texture2D backGround0;
        private Texture2D backGround1;
        private Texture2D backGround2;
        private float backGroundScale = 2.0f;
        private float backGroundScale1 = 2.5f;
        private float layer0Scroll = .25f;
        private float layer1Scroll = 1.0f;
        private float layer2Scroll = 3.0f;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;

        World world;

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backGround0 = Content.Load<Texture2D>(@"Images/Background");
            backGround1 = Content.Load<Texture2D>(@"Images/Bridge");
            backGround2 = Content.Load<Texture2D>(@"Images/Walkway");

            //audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            //waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            //themeSong = Content.Load<Song>(@"Audio\Theme");
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

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
                cameraPosition.X -= cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X += cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                cameraPosition.Y -= cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                cameraPosition.Y += cameraSpeed;


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


            spriteBatch.Draw(backGround0, new Vector2(cameraPosition.X, 0),
                new Rectangle((int)Math.Round(cameraPosition.X * layer0Scroll / backGroundScale1),
                    0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, backGroundScale1,
                    SpriteEffects.None, 0);

            //second layer
            spriteBatch.Draw(backGround1, new Vector2(cameraPosition.X, 180),
                new Rectangle((int)Math.Round(cameraPosition.X * layer1Scroll / backGroundScale),
                    0, 1000, 130), Color.White, 0.0f, Vector2.Zero, backGroundScale,
                    SpriteEffects.None, 0);

            //first layer
            spriteBatch.Draw(backGround2, new Vector2(cameraPosition.X, 250),
                new Rectangle((int)Math.Round(cameraPosition.X * layer2Scroll / backGroundScale),
                    0, 800, 130), Color.White, 0.0f, Vector2.Zero, backGroundScale,
                    SpriteEffects.None, 0);


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
