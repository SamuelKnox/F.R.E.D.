#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Artemis;
using Fred.Components;
using Artemis.System;
using System.IO;
#endregion

namespace Fred
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Fred : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EntityWorld world;

        public Fred()
            : base()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true
            };
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
            LoadContent();

            IsMouseVisible = false;

            world = new EntityWorld();

            EntitySystem.BlackBoard.SetEntry("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry("GraphicsDevice", GraphicsDevice);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", spriteBatch);

            world.InitializeAll(true);

            List<Vector2> playerStartPositions = InitializeWalls();
            InitializeGoodPlayers(playerStartPositions[0]);
            InitializeEvilPlayers(playerStartPositions[1]);

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
            this.world.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            world.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void InitializeGoodPlayers(Vector2 playerStartPosition)
        {
            Entity player = world.CreateEntity();

            player.AddComponentFromPool<TransformComponent>();
            player.AddComponent(new SpatialFormComponent("GoodPlayer"));
            player.AddComponent(new HealthComponent(10));
            player.AddComponent(new VelocityComponent());

            player.GetComponent<TransformComponent>().X = playerStartPosition.X;
            player.GetComponent<TransformComponent>().Y = playerStartPosition.Y;
            player.Tag = "GOOD_PLAYER";

        }
        void InitializeEvilPlayers(Vector2 playerStartPosition)
        {
            Entity enemy = world.CreateEntity();

            enemy.AddComponentFromPool<TransformComponent>();
            enemy.AddComponent(new SpatialFormComponent("BadPlayer"));
            enemy.AddComponent(new HealthComponent(10));
            enemy.AddComponent(new VelocityComponent());

            enemy.GetComponent<TransformComponent>().X = playerStartPosition.X;
            enemy.GetComponent<TransformComponent>().Y = playerStartPosition.Y;
            enemy.Tag = "BAD_PLAYER";
        }
        List<Vector2> InitializeWalls()
        {
            int wallSize = 40;
            int width = 39;
            int height = 22;
            List<Vector2> playerStartPoints = new List<Vector2>();
            playerStartPoints.Add(new Vector2(GraphicsDevice.DisplayMode.Width * 0.5F, GraphicsDevice.DisplayMode.Height * 0.5F));
            playerStartPoints.Add(new Vector2(GraphicsDevice.DisplayMode.Width * 0.8F, GraphicsDevice.DisplayMode.Height * 0.5F));
            int[,] mazeLayout = new int[width, height];
            Random rand = new Random();

            StreamReader file = new StreamReader("Mazes/maze" + rand.Next(0,2) + ".txt");
            while (file.Peek() != -1)
            {
            for (int i = 0; i < height; ++i)
            {

                string line = file.ReadLine();
                Console.WriteLine(line);

                for (int j = 0; j < line.Length; ++j)
                {
                    mazeLayout[j, i] = int.Parse(line[j] + "");
                }
            }


            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (mazeLayout[i, j] == 1)
                    {
                        Entity wall = world.CreateEntity();

                        wall.AddComponentFromPool<TransformComponent>();
                        wall.AddComponent(new SpatialFormComponent("Wall"));
                        wall.AddComponent(new HealthComponent(3));

                        wall.GetComponent<TransformComponent>().X = i * wallSize;
                        wall.GetComponent<TransformComponent>().Y = j * wallSize;
                        wall.Group = "Walls";
                    }
                    else if (mazeLayout[i, j] == 2)
                    {
                        float x = i * wallSize + wallSize / 4;
                        float y = j * wallSize + wallSize / 4;
                        playerStartPoints[0] = new Vector2(x, y);
                    }
                    else if (mazeLayout[i, j] == 3)
                    {
                        float x = i * wallSize + wallSize / 4;
                        float y = j * wallSize + wallSize / 4;
                        playerStartPoints[1] = new Vector2(x, y);
                    }
                }
            }
            }
            return playerStartPoints;
        }

    }
}
