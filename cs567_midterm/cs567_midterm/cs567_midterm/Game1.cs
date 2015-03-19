using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

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

        #region samus

        private Texture2D samusTexture;
        private Player player;

        #endregion samus

        #region enemy

        private List<Enemy> enemies;
        private Texture2D pirateTexture;
        private Texture2D metroidTexture;
        private float enemyGenerationCounter;
        private float enemyGenerationRate;

        #endregion enemy

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //private SpriteManager spriteManager;

        #region framerate and camerea

        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 2.0f;

        #endregion framerate and camerea

        private Display display;
        private Random rand;

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
            display = new Display(this);

            enemyGenerationRate = .5f;
            rand = new Random();
            enemies = new List<Enemy>();

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
            HandleEnemies((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            player.Update(gameTime, cameraPosition);

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

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.Update(gameTime, cameraPosition);
                cameraPosition.X -= cameraSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
                cameraPosition.X += cameraSpeed;
            if (keyboardState.IsKeyDown(Keys.Space))
                player.Update(gameTime, cameraPosition);

            base.Update(gameTime);
        }

        private void HandleEnemies(float elapsedTime)
        {
            foreach (Enemy currentEnemy in enemies)
            {
                currentEnemy.Update(elapsedTime);
            }

            CheckForEnemiesOffScreen();

            enemyGenerationCounter -= elapsedTime;

            if (enemyGenerationCounter > 0)
                return;

            enemyGenerationCounter += 1000f / enemyGenerationRate;

            CreateNewEnemy();
        }

        private void CreateNewEnemy()
        {
            Enemy enemyTemp;
            int randomOffset;

            randomOffset = rand.Next(10, 250);

            enemyTemp = new Enemy(pirateTexture, cameraPosition.X + 800, 200, new Point(447, 2), new Point(0, 0), new Point(46, 66), new Point(8, 1), 8, 2.5f);
            enemies.Add(enemyTemp);
            enemyTemp = new Enemy(metroidTexture, cameraPosition.X + 800, randomOffset, new Point(285, 4), new Point(0, 0), new Point(41, 47), new Point(2, 1), 2, 1.0f);
            enemies.Add(enemyTemp);
        }

        private void CheckForEnemiesOffScreen()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].Position.X < cameraPosition.X - 70)
                    enemies.RemoveAt(i);
            }
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

            display.DisplayBackGround(spriteBatch, cameraPosition);

            player.Draw(spriteBatch);

            foreach (Enemy currentEnemy in enemies)
            {
                currentEnemy.Draw(spriteBatch);
            }

            display.DisplayScore(spriteBatch, cameraPosition);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }
    }
}