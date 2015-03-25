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
        #region game variables

        private enum GameState { Menu, Playing, Paused, Dead, Boss };

        private float currentPos;
        private Display display;
        private Random rand;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameState currentState;
        private int enemiesDefeated;
        public int totalScore;
        private SpriteFont titleFont;
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        #endregion game variables

        #region sound

        private SoundEffect running;
        private SoundEffect playerShoot;
        private SoundEffect enemyDie;
        private SoundEffect enemyShoot;
        private Song themeSong;
        private bool songStart = false;

        #endregion sound

        #region samus

        private Texture2D samusTexture;
        private Player player;

        #endregion samus

        #region powerBeam

        private Texture2D powerBeam;
        private List<Weapon> powerBeamWeapon;
        private float fireCounter;
        private const float FIRE_RATE = 3.0f;

        #endregion powerBeam

        #region enemy

        private Boss boss;
        private int bossLife;
        private List<Enemy> enemies;
        private List<Boss> bosses;
        private Texture2D pirateTexture;
        private Texture2D metroidTexture;
        private float enemyGenerationCounter;
        private float enemyGenerationRate;
        private Texture2D bossTexture;

        #endregion enemy

        #region enemy weapon

        private Texture2D enemyWeaponTexture;
        private List<EnemyWeapon> enemyWeapon;
        private float enemyFireCounter;
        private const float ENEMY_FIRE_RATE = .5f;

        #endregion enemy weapon

        #region framerate and camerea

        private Vector2 cameraPosition = Vector2.Zero;
        private const float cameraSpeed = 3.5f;

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
            display = new Display(this);
            currentState = GameState.Menu;
            enemyGenerationRate = .5f;
            rand = new Random();
            enemies = new List<Enemy>();
            player = new Player(samusTexture, 100, 220, new Point(240, 650), new Point(0, 0), new Point(48, 49), new Point(4, 3), 10, 3.0f);
            powerBeamWeapon = new List<Weapon>();
            enemyWeapon = new List<EnemyWeapon>();
            bosses = new List<Boss>();
            enemyFireCounter = 0;
            fireCounter = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            themeSong = Content.Load<Song>(@"Audio/Theme");
            samusTexture = Content.Load<Texture2D>(@"Images/samus");
            powerBeam = Content.Load<Texture2D>(@"Images/powerBeam");
            pirateTexture = Content.Load<Texture2D>(@"Images/space pirates");
            metroidTexture = Content.Load<Texture2D>(@"Images/metroids");
            titleFont = Content.Load<SpriteFont>(@"MenuPausedDead");
            running = Content.Load<SoundEffect>(@"Audio/Running");
            playerShoot = Content.Load<SoundEffect>(@"Audio/SuperMissile");
            enemyDie = Content.Load<SoundEffect>(@"Audio/Boss5");
            enemyShoot = Content.Load<SoundEffect>(@"Audio/Enemy6");
            enemyWeaponTexture = Content.Load<Texture2D>(@"Images/enemyWeapon");
            bossTexture = Content.Load<Texture2D>(@"Images/SA-X");
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
            currentPos = player.Position.X;
            if (!songStart)
            {
                MediaPlayer.Play(themeSong);
                songStart = true;
            }

            if (enemiesDefeated == 10)
            {
                enemiesDefeated = 0;
                currentState = GameState.Boss;
            }

            currentKeyboardState = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.Menu:
                    Update_Menu(gameTime);
                    break;

                case GameState.Playing:
                    Update_Playing(gameTime);
                    break;

                case GameState.Paused:
                    Update_Paused(gameTime);
                    break;

                case GameState.Dead:
                    Update_Dead(gameTime);
                    break;

                case GameState.Boss:
                    Update_Boss(gameTime);
                    break;
            }

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        private void Update_Boss(GameTime gameTime)
        {
            if (CheckForPlayerCollision() == true)
            {
                currentState = GameState.Dead;
                return;
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies.RemoveAt(i);
            }

            HandleBoss((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            HandlePowerBeam((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            HandleEnemyWeapon((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            fireCounter -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            enemyFireCounter -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            CheckForCollisions();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            player.Update(gameTime, cameraPosition, running);

            KeyboardState keyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                currentState = GameState.Paused;

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                cameraPosition.X += cameraSpeed;
                player.Update(gameTime, cameraPosition, running);
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.Update(gameTime, cameraPosition, running);
                cameraPosition.X -= cameraSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
                player.Update(gameTime, cameraPosition, running);

            if (keyboardState.IsKeyDown(Keys.F) && (fireCounter <= 0))
            {
                FirePowerBeam();
                fireCounter = 1000f / FIRE_RATE;
            }

            foreach (Boss b in bosses)
            {
                if (enemyFireCounter <= 0)
                {
                    FireBossWeapon();
                    enemyFireCounter = 1000f / ENEMY_FIRE_RATE;
                }
            }
        }

        private void Update_Playing(GameTime gameTime)
        {
            if (CheckForPlayerCollision() == true)
            {
                currentState = GameState.Dead;
                return;
            }
            HandleEnemies((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            HandlePowerBeam((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            HandleEnemyWeapon((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            fireCounter -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            enemyFireCounter -= ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            CheckForCollisions();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            player.Update(gameTime, cameraPosition, running);

            KeyboardState keyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                currentState = GameState.Paused;

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                cameraPosition.X += cameraSpeed;
                player.Update(gameTime, cameraPosition, running);
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.Update(gameTime, cameraPosition, running);
                cameraPosition.X -= cameraSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
                player.Update(gameTime, cameraPosition, running);

            if (keyboardState.IsKeyDown(Keys.F) && (fireCounter <= 0))
            {
                FirePowerBeam();
                fireCounter = 1000f / FIRE_RATE;
            }

            foreach (Enemy e in enemies)
            {
                if (enemyFireCounter <= 0)
                {
                    FireEnemyWeapon();
                    enemyFireCounter = 1000f / ENEMY_FIRE_RATE;
                }
            }
        }

        private void FireBossWeapon()
        {
            foreach (Boss b in bosses)
            {
                EnemyWeapon newEnemyWeapon;
                newEnemyWeapon = new EnemyWeapon(enemyWeaponTexture, b.Position.X - 10, b.Position.Y + 120 - enemyWeaponTexture.Height / 2, 10);
                enemyWeapon.Add(newEnemyWeapon);
                enemyShoot.Play();
            }
        }

        private void HandleBossWeapon(float elapsedTime)
        {
            foreach (EnemyWeapon w in enemyWeapon)
            {
                w.Update(elapsedTime);
            }

            CheckForEnemyWeaponOffScreen();
        }

        private void FireEnemyWeapon()
        {
            foreach (Enemy e in enemies)
            {
                if (e.enemyType == 1 && e.Position.X - cameraPosition.X <= (690))
                {
                    EnemyWeapon newEnemyWeapon;
                    newEnemyWeapon = new EnemyWeapon(enemyWeaponTexture, e.Position.X, e.Position.Y + 75 - enemyWeaponTexture.Height / 2, 5);
                    enemyWeapon.Add(newEnemyWeapon);
                    enemyShoot.Play();
                }
            }
        }

        private void HandleEnemyWeapon(float elapsedTime)
        {
            foreach (EnemyWeapon w in enemyWeapon)
            {
                w.Update(elapsedTime);
            }

            CheckForEnemyWeaponOffScreen();
        }

        private void CheckForEnemyWeaponOffScreen()
        {
            for (int i = enemyWeapon.Count - 1; i >= 0; i--)
            {
                if (enemyWeapon[i].Position.X < player.Position.X - 100)
                {
                    enemyWeapon.RemoveAt(i);
                }
            }
        }

        private void Update_Menu(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                currentState = GameState.Playing;
                ResetGame();
            }
        }

        private void ResetGame()
        {
            enemies.Clear();
            powerBeamWeapon.Clear();
            enemyWeapon.Clear();
            bosses.Clear();
            totalScore = 0;
        }

        private void Update_Dead(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                currentState = GameState.Menu;
        }

        private void Update_Paused(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                currentState = GameState.Menu;

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
                currentState = GameState.Playing;
        }

        private bool CheckForPlayerCollision()
        {
            Rectangle playerBounds;
            playerBounds = new Rectangle((int)player.Position.X, (int)player.Position.Y, 48 * 2, 49);

            foreach (Enemy e in enemies)
            {
                if (e.Bounds.Intersects(playerBounds))
                    return true;
            }

            foreach (EnemyWeapon ew in enemyWeapon)
            {
                if (ew.Bounds.Intersects(playerBounds))
                    return true;
            }

            return false;
        }

        private void CheckForCollisions()
        {
            Rectangle powerBeamRectangle;
            Rectangle enemyRectangle;
            Rectangle bossRectangle;

            foreach (Weapon w in powerBeamWeapon)
            {
                powerBeamRectangle = w.Bounds;

                foreach (Enemy e in enemies)
                {
                    enemyRectangle = e.Bounds;
                    if (enemyRectangle.Intersects(powerBeamRectangle))
                    {
                        e.isAlive = false;
                        w.isAlive = false;
                        enemyDie.Play();
                        enemiesDefeated++;
                        totalScore++;
                        break;
                    }
                }

                foreach (Boss b in bosses)
                {
                    bossRectangle = b.Bounds;
                    if (bossRectangle.Intersects(powerBeamRectangle))
                    {
                        b.bossLife--;
                        if (b.bossLife == 0)
                        {
                            b.isAlive = false;
                            w.isAlive = false;
                            totalScore = totalScore + 10;
                            currentState = GameState.Playing;
                        }
                    }
                }
            }

            for (int w = powerBeamWeapon.Count - 1; w >= 0; w--)
            {
                if (powerBeamWeapon[w].isAlive == false)
                    powerBeamWeapon.RemoveAt(w);
            }

            for (int e = enemies.Count - 1; e >= 0; e--)
            {
                if (enemies[e].isAlive == false)
                    enemies.RemoveAt(e);
            }

            for (int b = bosses.Count - 1; b >= 0; b--)
            {
                if (bosses[b].isAlive == false)
                    bosses.RemoveAt(b);
            }
        }

        private void HandlePowerBeam(float elapsedTime)
        {
            foreach (Weapon w in powerBeamWeapon)
            {
                w.Update(elapsedTime);
            }

            CheckForPowerBeamOffScreen();
        }

        private void CheckForPowerBeamOffScreen()
        {
            for (int i = powerBeamWeapon.Count - 1; i >= 0; i--)
            {
                if (powerBeamWeapon[i].Position.X > player.Position.X + 500)
                {
                    powerBeamWeapon.RemoveAt(i);
                }
            }
        }

        private void FirePowerBeam()
        {
            Weapon newPowerBeam;
            newPowerBeam = new Weapon(powerBeam, player.Position.X + 41 * 3, player.Position.Y + 15 * 3 - powerBeam.Height / 2);
            powerBeamWeapon.Add(newPowerBeam);
            playerShoot.Play();
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

            enemyTemp = new Enemy(pirateTexture, cameraPosition.X + 800, 200, new Point(447, 2), new Point(0, 0), new Point(46, 66), new Point(8, 1), 8, 2.5f, 1);
            enemies.Add(enemyTemp);
            enemyTemp = new Enemy(metroidTexture, cameraPosition.X + 800, randomOffset, new Point(285, 4), new Point(0, 0), new Point(41, 47), new Point(2, 1), 2, 1.0f, 2);
            enemies.Add(enemyTemp);
        }

        private void HandleBoss(float elapsedTime)
        {
            foreach (Boss currentBoss in bosses)
            {
                currentBoss.Update(elapsedTime);
            }

            if (bosses.Count < 1)
                CreateNewBoss();
        }

        private void CreateNewBoss()
        {
            Boss bossTemp;

            bossTemp = new Boss(bossTexture, currentPos + 500, 150, new Point(0, 0), new Point(6, 0), new Point(104, 106), new Point(6, 1), 6, 2f);

            bosses.Add(bossTemp);
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

            switch (currentState)
            {
                case GameState.Playing:
                    Draw_Playing(gameTime);
                    break;

                case GameState.Menu:
                    Draw_Menu(gameTime);
                    break;

                case GameState.Dead:
                    Draw_Dead(gameTime);
                    break;

                case GameState.Paused:
                    Draw_Paused(gameTime);
                    break;

                case GameState.Boss:
                    Draw_Boss(gameTime);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void Draw_Menu(GameTime gameTime)
        {
            display.DisplayBackGround(spriteBatch, cameraPosition);

            spriteBatch.DrawString(titleFont, "My Metroid Shooter", new Vector2(cameraPosition.X + 100, 100), Color.White);
            spriteBatch.DrawString(titleFont, "Press Enter to play", new Vector2(cameraPosition.X + 100, 150), Color.White);
            spriteBatch.DrawString(titleFont, "Press ESC to quit", new Vector2(cameraPosition.X + 100, 200), Color.White);
        }

        private void Draw_Paused(GameTime gameTime)
        {
            Draw_Playing(gameTime);
            spriteBatch.DrawString(titleFont, "Game is Paused", new Vector2(cameraPosition.X + 100, 50), Color.White);
            spriteBatch.DrawString(titleFont, "Press Enter to Unpause", new Vector2(cameraPosition.X + 100, 100), Color.White);
            spriteBatch.DrawString(titleFont, "Press ESC for the menu", new Vector2(cameraPosition.X + 100, 150), Color.White);
        }

        private void Draw_Dead(GameTime gameTime)
        {
            display.DisplayBackGround(spriteBatch, cameraPosition);
            spriteBatch.DrawString(titleFont, "You Were Defeated!", new Vector2(cameraPosition.X + 100, 100), Color.White);
            spriteBatch.DrawString(titleFont, "Press Enter for the menu", new Vector2(cameraPosition.X + 100, 150), Color.White);
        }

        private void Draw_Playing(GameTime gameTime)
        {
            display.DisplayBackGround(spriteBatch, cameraPosition);

            player.Draw(spriteBatch);

            foreach (Weapon w in powerBeamWeapon)
            {
                w.Draw(spriteBatch);
            }
            foreach (EnemyWeapon ew in enemyWeapon)
            {
                ew.Draw(spriteBatch);
            }
            foreach (Enemy currentEnemy in enemies)
            {
                currentEnemy.Draw(spriteBatch);
            }

            display.DisplayScore(spriteBatch, cameraPosition, totalScore);
        }

        private void Draw_Boss(GameTime gametime)
        {
            display.DisplayBackGround(spriteBatch, cameraPosition);

            player.Draw(spriteBatch);

            foreach (Boss b in bosses)
            {
                b.Draw(spriteBatch);
                bossLife = b.bossLife;
            }
            foreach (EnemyWeapon ew in enemyWeapon)
            {
                ew.Draw(spriteBatch);
            }
            foreach (Weapon w in powerBeamWeapon)
            {
                w.Draw(spriteBatch);
            }

            display.DisplayBossLife(spriteBatch, cameraPosition, bossLife);
        }
    }
}