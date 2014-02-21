#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Fonts;
#endregion

//TODO: set up GameOver() for 3+ players
//TODO: Controller designation is not fully functional for 3+ players in update()

namespace Mascape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Player good1;
        //Player evil1;
        static int mapWidth = 15;
        static int mapHeight = 10;
        Wall[, ,] walls = new Wall[2, mapWidth, mapHeight];
        Wall[, ,] borders = new Wall[2, mapWidth, mapHeight];
        Random rand = new Random();
        int mapFullness;
        int randomExit;
        List<Trap> traps = new List<Trap>();
        TextField stats;
        List<Player> goodPlayers = new List<Player>();
        List<Player> evilPlayers = new List<Player>();

        public Game1()
            : base()
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
            graphics.IsFullScreen = true;
            IsMouseVisible = false;
            mapFullness = 3;
            //New Player Creation Specs:
            //(Attribute hp, float speed, Attribute destroy, Attribute build, Attribute bomb, float dmg, float heal, String team)
            Player good1 = new Player(new Attribute(150), 100F, new Attribute(1.5), new Attribute(3.0), new Attribute(3.0), 10.0F, 10.0F, "good");
            Player evil1 = new Player(new Attribute(150), 100F, new Attribute(2.0), new Attribute(3.0), new Attribute(3.5), 50.0F, 5.0F, "evil");
            goodPlayers.Add(good1);
            evilPlayers.Add(evil1);
            randomExit = rand.Next(mapHeight);
            stats = new TextField("Stats", new Vector2(1200.0F, 50.0F), Color.Black);

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

            // TODO: use this.Content to load your game content here
            SpawnWalls();
            foreach (Player p in goodPlayers)
            {
                p.Image = Content.Load<Texture2D>("player");
                p.Rectangle = new Rectangle(-1, -1, p.Image.Width, p.Image.Height);
                p.Vector = new Vector2((float)1.5 * borders[0, 0, 0].Image.Width, (float)rand.Next(borders[1, 0, 0].Image.Height + (int)(0.5 * borders[1, 0, 0].Image.Height), borders[1, 0, 0].Image.Height * mapHeight + (int)(0.5 * borders[0, 0, 0].Image.Height)));


            }
            foreach (Player p in evilPlayers)
            {
                p.Image = Content.Load<Texture2D>("enemy");
                p.Rectangle = new Rectangle(-1, -1, p.Image.Width, p.Image.Height);
                p.Vector = new Vector2((float)(1 + (.5 * mapWidth)) * borders[0, 0, 0].Image.Width, (float)rand.Next(borders[1, 0, 0].Image.Height + (int)(0.5 * borders[1, 0, 0].Image.Height), borders[1, 0, 0].Image.Height * mapHeight + (int)(0.5 * borders[0, 0, 0].Image.Height)));

            }
            stats.loadContent(Content);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Game Status
            stats.Message = "\n\nRed Guy States:\n" + (int)goodPlayers[0].HitPoints.Current + " / " + (int)goodPlayers[0].HitPoints.Maximum + " Hit Points\n" + (int)goodPlayers[0].DestroyCooldown.Current + " / " + (int)goodPlayers[0].DestroyCooldown.Maximum + " Destroy Cooldown\n" + (int)goodPlayers[0].BuildCooldown.Current + " / " + (int)goodPlayers[0].BuildCooldown.Maximum + " Build Cooldown\n" + (int)goodPlayers[0].BombCooldown.Current + " / " + (int)goodPlayers[0].BombCooldown.Maximum + " Bomb Cooldown\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nBlue Guy Status:\n" + (int)evilPlayers[0].HitPoints.Current + " / " + (int)evilPlayers[0].HitPoints.Maximum + " Hit Points\n" + (int)evilPlayers[0].DestroyCooldown.Current + " / " + (int)evilPlayers[0].DestroyCooldown.Maximum + " Destroy Cooldown\n" + (int)evilPlayers[0].BuildCooldown.Current + " / " + (int)evilPlayers[0].BuildCooldown.Maximum + " Build Cooldown\n" + (int)evilPlayers[0].BombCooldown.Current + " / " + (int)evilPlayers[0].BombCooldown.Maximum + " Bomb Cooldown";

            //Player 1 (good1)
            foreach (Player p in goodPlayers)
            {
                DecrementCooldown(p, gameTime);
                if (p.Human == true)
                {
                    if (GamePad.GetState(PlayerIndex.One).IsConnected)
                    {
                        GamePadState playerController = GamePad.GetState(PlayerIndex.One);
                        GamePadAction(p, playerController, gameTime);
                    }
                    else
                    {
                        KeyboardAction(p, gameTime);

                    }
                }
            }


            //Player 2 (evil1)
            foreach (Player p in evilPlayers)
            {
                DecrementCooldown(p, gameTime);
                if (p.Human == true)
                {
                    if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        GamePadState enemyController = GamePad.GetState(PlayerIndex.Two);
                        GamePadAction(p, enemyController, gameTime);
                    }
                    else
                    {
                        KeyboardAction(p, gameTime);
                    }
                }
            }

            //Damage
            foreach (Player p in goodPlayers)
            {
                CheckCollisions(p, gameTime);
            }
            foreach (Player p in evilPlayers)
            {
                CheckCollisions(p, gameTime);
            }

            //Health Status
            foreach (Player p in goodPlayers)
            {
                ResizePlayer(p);
            }
            foreach (Player p in evilPlayers)
            {
                ResizePlayer(p);
            }

            //Check Game Status
            GameOver();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawWalls();
            DrawTraps();
            foreach (Player p in goodPlayers)
            {
                if (p.HitPoints.Current > 0)
                {
                    spriteBatch.Draw(p.Image, p.Rectangle, Color.White);
                }
            }
            foreach (Player p in evilPlayers)
            {
                if (p.HitPoints.Current > 0)
                {
                    spriteBatch.Draw(p.Image, p.Rectangle, Color.White);
                }
            }
            stats.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }



        /// <summary>
        ///         Build a wall
        ///Add a build time?
        ///Remove the build cooldown?
        /// </summary>
        /// <param name="player">Current Player</param>
        void BuildClosestWall(Rectangle player)
        {
            Wall closestWall = walls[0, 0, 0];
            double shortestDistance = GetDistanceToWall(player, closestWall.Rectangle);


            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (shortestDistance > GetDistanceToWall(player, walls[i, j, k].Rectangle))
                        {
                            shortestDistance = GetDistanceToWall(player, walls[i, j, k].Rectangle);
                            closestWall = walls[i, j, k];
                        }
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (closestWall.Equals(walls[i, j, k]))
                        {
                            walls[i, j, k].Active = true;
                        }

                    }
                }
            }

        }


        /// <summary>
        ///Player destroys a wall
        ///Add a Destroy time?
        ///Remove the cooldown?
        /// </summary>
        /// <param name="player">current player</param>
        void DestroyClosestWall(Rectangle player)
        {
            Wall closestWall = walls[0, 0, 0];
            double shortestDistance = GetDistanceToWall(player, closestWall.Rectangle);


            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (shortestDistance > GetDistanceToWall(player, walls[i, j, k].Rectangle))
                        {
                            shortestDistance = GetDistanceToWall(player, walls[i, j, k].Rectangle);
                            closestWall = walls[i, j, k];
                        }
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (closestWall.Equals(walls[i, j, k]))
                        {
                            walls[i, j, k].Active = false;
                        }

                    }
                }
            }
        }


        /// <summary>
        ///Calculates distance between player and wall
        /// </summary>
        /// <param name="player">current player</param>
        /// <param name="wall">nearby wall</param>
        /// <returns>distance between player and wall</returns>
        static double GetDistanceToWall(Rectangle player, Rectangle wall)
        {
            return Math.Sqrt(Math.Pow(GetCenterRec(player).X - GetCenterRec(wall).X, 2) + Math.Pow(GetCenterRec(player).Y - GetCenterRec(wall).Y, 2));

        }

        //Returns center point of a Rectangle object
        /// <summary>
        ///Returns center point of a Rectangle object
        /// </summary>
        /// <param name="r">rectangle for center</param>
        /// <returns>center point of a rectangle</returns>
        static Point GetCenterRec(Rectangle r)
        {
            return new Point(r.Left + r.Width / 2,
                             r.Top + r.Height / 2);
        }


        /// <summary>
        ///Create the map
        ///Randomly Generated
        ///Needs better algorithm
        /// </summary>
        void SpawnWalls()
        {
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        walls[i, j, k] = new Wall();
                        if (i == 0)
                        {
                            walls[i, j, k].Orientation = "horizontal";
                        }
                        else
                        {
                            walls[i, j, k].Orientation = "vertical";
                        }
                        if (i == 0 && k != 0)
                        {
                            walls[i, j, k].Image = Content.Load<Texture2D>("horizontalwall");
                            walls[i, j, k].Rectangle = new Rectangle((j + 1) * walls[i, j, k].Image.Width, (k + 1) * walls[i, j, k].Image.Width, walls[i, j, k].Image.Width, walls[i, j, k].Image.Height);

                        }
                        else if (j != 0)
                        {
                            walls[i, j, k].Image = Content.Load<Texture2D>("verticalwall");
                            walls[i, j, k].Rectangle = new Rectangle((j + 1) * walls[i, j, k].Image.Height, (k + 1) * walls[i, j, k].Image.Height, walls[i, j, k].Image.Width, walls[i, j, k].Image.Height);

                        }
                        if (!(rand.Next(mapFullness) == 0))
                        {
                            walls[i, j, k].Active = true;
                        }
                        else
                        {
                            walls[i, j, k].Active = false;
                        }
                    }
                }

            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        borders[i, j, k] = new Wall();
                        if (i == 0)
                        {
                            borders[i, j, k].Orientation = "horizontal";
                        }
                        else
                        {
                            borders[i, j, k].Orientation = "vertical";
                        }

                        borders[i, j, k].Active = false;
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (i == 0 && (k == 0 || k == mapHeight - 1))
                        {
                            borders[i, j, k].Image = Content.Load<Texture2D>("horizontalborder");
                            borders[i, j, k].Active = true;
                            if (k == 0)
                            {
                                borders[i, j, k].Rectangle = new Rectangle((j + 1) * borders[i, j, k].Image.Width, (k + 1) * borders[i, j, k].Image.Width, borders[i, j, k].Image.Width, borders[i, j, k].Image.Height);
                            }
                            else
                            {
                                borders[i, j, k].Rectangle = new Rectangle((j + 1) * borders[i, j, k].Image.Width, (k + 2) * borders[i, j, k].Image.Width, borders[i, j, k].Image.Width, borders[i, j, k].Image.Height);

                            }
                        }
                        else if (i == 1 && (j == 0 || j == mapWidth - 1))
                        {
                            borders[i, j, k].Image = Content.Load<Texture2D>("verticalborder");
                            borders[i, j, k].Active = true;
                            borders[1, mapWidth - 1, randomExit].Active = false;
                            if (j == 0)
                            {
                                borders[i, j, k].Rectangle = new Rectangle((j + 1) * borders[i, j, k].Image.Height, (k + 1) * borders[i, j, k].Image.Height, borders[i, j, k].Image.Width, borders[i, j, k].Image.Height);
                            }
                            else
                            {
                                borders[i, j, k].Rectangle = new Rectangle((j + 2) * borders[i, j, k].Image.Height, (k + 1) * borders[i, j, k].Image.Height, borders[i, j, k].Image.Width, borders[i, j, k].Image.Height);

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        ///Player control via gamepad 
        /// </summary>
        /// <param name="player">current player</param>
        /// <param name="controller">controller they are using</param>
        /// <param name="gameTime">gametime</param>
        void GamePadAction(Player player, GamePadState controller, GameTime gameTime)
        {

            if (controller.ThumbSticks.Left.X < 0 && player.Mobile == true && player.Mobile == true)
            {
                float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(x, player.Vector.Y);
            }
            if (controller.ThumbSticks.Left.X > 0 && player.Mobile == true && player.Mobile == true)
            {
                float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(x, player.Vector.Y);
            }
            if (controller.ThumbSticks.Left.Y < 0 && player.Mobile == true && player.Mobile == true)
            {
                float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(player.Vector.X, y);
            }
            if (controller.ThumbSticks.Left.Y > 0 && player.Mobile == true && player.Mobile == true)
            {
                float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(player.Vector.X, y);
            }

            if (controller.Buttons.A == ButtonState.Pressed && player.DestroyCooldown.Current <= 0)
            {
                player.DestroyCooldown.Current = player.DestroyCooldown.Maximum;
                DestroyClosestWall(player.Rectangle);
            }
            if (controller.Buttons.B == ButtonState.Pressed && player.BuildCooldown.Current <= 0)
            {
                player.BuildCooldown.Current = player.BuildCooldown.Maximum;
                BuildClosestWall(player.Rectangle);
            }
            if (controller.Buttons.X == ButtonState.Pressed && player.BombCooldown.Current <= 0)
            {
                player.BombCooldown.Current = player.BombCooldown.Maximum;
                UseBomb(player);
            }
            if (controller.Buttons.Y == ButtonState.Pressed)
            {
                Heal(player, gameTime);
            }
            else
            {
                player.Mobile = true;
            }



            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    for (int k = 0; k < mapHeight; k++)
                    {
                        if (player.Rectangle.Intersects(walls[i, j, k].Rectangle) && walls[i, j, k].Active == true)
                        {
                            if (walls[i, j, k].Orientation.Equals("horizontal"))
                            {
                                if (player.Rectangle.Y > walls[i, j, k].Rectangle.Y)
                                {
                                    float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                                else
                                {
                                    float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                            }
                            else
                            {
                                if (player.Rectangle.X > walls[i, j, k].Rectangle.X)
                                {
                                    float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                                else
                                {
                                    float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                            }

                        }
                    }
                }
            }





            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    for (int k = 0; k < mapHeight; k++)
                    {
                        if (player.Rectangle.Intersects(borders[i, j, k].Rectangle) && borders[i, j, k].Active == true)
                        {
                            if (borders[i, j, k].Orientation.Equals("horizontal"))
                            {
                                if (player.Rectangle.Y > borders[i, j, k].Rectangle.Y)
                                {
                                    float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                                else
                                {
                                    float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                            }
                            else
                            {
                                if (player.Rectangle.X > borders[i, j, k].Rectangle.X)
                                {
                                    float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                                else
                                {
                                    float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                            }
                        }
                    }
                }

            }
            Rectangle r = player.Rectangle;
            r.X = (int)player.Vector.X;
            r.Y = (int)player.Vector.Y;
            player.Rectangle = r;
        }


        /// <summary>
        ///Player control via keyboard
        /// </summary>
        /// <param name="player">current player</param>
        /// <param name="gameTime">gametime</param>
        void KeyboardAction(Player player, GameTime gameTime)
        {

            if ((Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) && player.Mobile == true)
            {
                float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(x, player.Vector.Y);
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)) && player.Mobile == true)
            {
                float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(x, player.Vector.Y);
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)) && player.Mobile == true)
            {
                float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(player.Vector.X, y);
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)) && player.Mobile == true)
            {
                float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Vector = new Vector2(player.Vector.X, y);
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.PageUp) || Keyboard.GetState().IsKeyDown(Keys.D1)) && player.DestroyCooldown.Current <= 0)
            {
                player.DestroyCooldown.Current = player.DestroyCooldown.Maximum;
                DestroyClosestWall(player.Rectangle);
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.PageDown) || Keyboard.GetState().IsKeyDown(Keys.D2)) && player.BuildCooldown.Current <= 0)
            {
                player.BuildCooldown.Current = player.BuildCooldown.Maximum;
                BuildClosestWall(player.Rectangle);
            }
            if ((Keyboard.GetState().IsKeyDown(Keys.RightShift) || Keyboard.GetState().IsKeyDown(Keys.D3)) && player.BombCooldown.Current <= 0)
            {
                player.BombCooldown.Current = player.BombCooldown.Maximum;
                UseBomb(player);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                Heal(player, gameTime);
            }
            else
            {
                player.Mobile = true;
            }



            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    for (int k = 0; k < mapHeight; k++)
                    {
                        if (player.Rectangle.Intersects(walls[i, j, k].Rectangle) && walls[i, j, k].Active == true)
                        {
                            if (walls[i, j, k].Orientation.Equals("horizontal"))
                            {
                                if (player.Rectangle.Y > walls[i, j, k].Rectangle.Y)
                                {
                                    float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                                else
                                {
                                    float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                            }
                            else
                            {
                                if (player.Rectangle.X > walls[i, j, k].Rectangle.X)
                                {
                                    float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                                else
                                {
                                    float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                            }

                        }
                    }
                }
            }





            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    for (int k = 0; k < mapHeight; k++)
                    {
                        if (player.Rectangle.Intersects(borders[i, j, k].Rectangle) && borders[i, j, k].Active == true)
                        {
                            if (borders[i, j, k].Orientation.Equals("horizontal"))
                            {
                                if (player.Rectangle.Y > borders[i, j, k].Rectangle.Y)
                                {
                                    float y = player.Vector.Y + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                                else
                                {
                                    float y = player.Vector.Y - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(player.Vector.X, y);
                                }
                            }
                            else
                            {
                                if (player.Rectangle.X > borders[i, j, k].Rectangle.X)
                                {
                                    float x = player.Vector.X + player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                                else
                                {
                                    float x = player.Vector.X - player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                    player.Vector = new Vector2(x, player.Vector.Y);
                                }
                            }
                        }
                    }
                }

            }
            Rectangle r = player.Rectangle;
            r.X = (int)player.Vector.X;
            r.Y = (int)player.Vector.Y;
            player.Rectangle = r;
        }


        /// <summary>
        ///Draw the walls
        /// </summary>
        void DrawWalls()
        {
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (((i == 0 && k != 0) || (i == 1 && j != 0)) && walls[i, j, k].Active == true)
                            spriteBatch.Draw(walls[i, j, k].Image, walls[i, j, k].Rectangle, Color.White);
                    }
                }
            }
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < mapWidth; ++j)
                {
                    for (int k = 0; k < mapHeight; ++k)
                    {
                        if (((i == 0 && k == 0) || (i == 0 && k == mapHeight - 1) || (i == 1 && j == 0) || (i == 1 && j == mapWidth - 1)) && borders[i, j, k].Active == true)
                        {
                            spriteBatch.Draw(borders[i, j, k].Image, borders[i, j, k].Rectangle, Color.White);
                        }
                    }
                }
            }
        }


        /// <summary>
        ///Use a proximity mine with a delay
        ///current proximity is 0...increase it? 
        /// </summary>
        /// <param name="player">current player</param>
        void UseBomb(Player player)
        {
            Trap t = new Trap(50.0, 5.0);
            t.Image = Content.Load<Texture2D>("bomb");
            t.Rectangle = new Rectangle(player.Rectangle.X, player.Rectangle.Y, t.Image.Width, t.Image.Height);
            traps.Add(t);
        }


        /// <summary>
        ///draw traps when they are laid 
        /// </summary>
        void DrawTraps()
        {
            foreach (Trap t in traps)
            {
                if (t.Active == true)
                {
                    spriteBatch.Draw(t.Image, t.Rectangle, Color.White);

                }
            }
        }


        /// <summary>
        ///health bar
        ///player size is determined by health
        /// </summary>
        /// <param name="player">current player</param>
        void ResizePlayer(Player player)
        {
            player.Rectangle = new Rectangle(player.Rectangle.X, player.Rectangle.Y, (int)(player.HitPoints.Current / player.HitPoints.Maximum * 15 + 10.0), (int)(player.HitPoints.Current / player.HitPoints.Maximum * 15 + 10.0));
        }


        /// <summary>
        ///collision check for DAMAGE 
        /// </summary>
        /// <param name="player">current player</param>
        /// <param name="gameTime">gametime</param>
        void CheckCollisions(Player player, GameTime gameTime)
        {
            foreach (Trap t in traps)
            {
                if (player.Rectangle.Intersects(t.Rectangle) && t.Delay <= 0 && t.Active == true)
                {
                    player.HitPoints.Current -= t.Damage;
                    t.Active = false;
                }
            }
            if (player.Team.Equals("good"))
            {
                foreach (Player p in evilPlayers)
                {
                    if (player.Rectangle.Intersects(p.Rectangle))
                    {
                        player.HitPoints.Current -= p.Damage * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        player.Healable = false;
                    }
                    else
                    {
                        player.Healable = true;
                    }
                }
            }
            else if (player.Team.Equals("evil"))
            {
                foreach (Player p in goodPlayers)
                {
                    if (player.Rectangle.Intersects(p.Rectangle))
                    {
                        player.HitPoints.Current -= p.Damage * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        player.Healable = false;
                    }
                    else
                    {
                        player.Healable = true;
                    }
                }
            }
        }


        /// <summary>
        ///Check to see if game is over
        ///Only set up for 1v1. Rules for multiplayer unknown
        /// </summary>
        void GameOver()
        {
            if (goodPlayers[0].HitPoints.Current <= 0)
            {
                Exit();
            }
            else if (goodPlayers[0].Rectangle.X > borders[1, mapWidth - 1, 0].Rectangle.X)
            {
                Exit();
            }
            else if (evilPlayers[0].HitPoints.Current <= 0)
            {
                Exit();
            }
        }


        /// <summary>
        ///Progress the cooldown rates
        /// </summary>
        /// <param name="player">current player</param>
        /// <param name="gameTime">gametime</param>
        void DecrementCooldown(Player player, GameTime gameTime)
        {

            player.DestroyCooldown.Current -= gameTime.ElapsedGameTime.TotalSeconds;
            player.BuildCooldown.Current -= gameTime.ElapsedGameTime.TotalSeconds;
            player.BombCooldown.Current -= gameTime.ElapsedGameTime.TotalSeconds;
            if (player.DestroyCooldown.Current < 0)
            {
                player.DestroyCooldown.Current = 0;
            }
            if (player.BuildCooldown.Current < 0)
            {
                player.BuildCooldown.Current = 0;
            }
            if (player.BombCooldown.Current < 0)
            {
                player.BombCooldown.Current = 0;
            }
            foreach (Trap t in traps)
            {
                t.Delay -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }


        /// <summary>
        ///player can heal, but cannot move
        /// </summary>
        /// <param name="p">current player</param>
        /// <param name="gameTime">gametime</param>
        void Heal(Player p, GameTime gameTime)
        {
            if (p.HitPoints.Current > 0 && p.HitPoints.Current < p.HitPoints.Maximum && p.Healable == true)
            {
                p.HitPoints.Current += p.Heal * (float)gameTime.ElapsedGameTime.TotalSeconds;
                p.Mobile = false;
            }
            else
            {
                p.Mobile = true;
            }
        }

    }
}