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
                IsFullScreen = false
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

            world = new EntityWorld();

            EntitySystem.BlackBoard.SetEntry("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry("GraphicsDevice", GraphicsDevice);
            EntitySystem.BlackBoard.SetEntry("SpriteBatch", spriteBatch);

            world.InitializeAll(true);

            InitializeGoodPlayers();
            InitializeEvilPlayers();
            InitializeWalls();

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

        void InitializeGoodPlayers()
        {
            Entity player = world.CreateEntity();

            player.AddComponentFromPool<TransformComponent>();
            player.AddComponent(new SpatialFormComponent("GoodPlayer"));
            player.AddComponent(new HealthComponent(10));
            player.AddComponent(new VelocityComponent());

            player.GetComponent<TransformComponent>().X = GraphicsDevice.Viewport.Width * 0.2f;
            player.GetComponent<TransformComponent>().Y = GraphicsDevice.Viewport.Height - 50;
            player.Tag = "GOOD_PLAYER";

        }
        void InitializeEvilPlayers()
        {
            Entity enemy = world.CreateEntity();

            enemy.AddComponentFromPool<TransformComponent>();
            enemy.AddComponent(new SpatialFormComponent("BadPlayer"));

            enemy.GetComponent<TransformComponent>().X = GraphicsDevice.Viewport.Width * 0.95f;
            enemy.GetComponent<TransformComponent>().Y = GraphicsDevice.Viewport.Height - 50;
            enemy.Tag = "BAD_PLAYER";
        }
        void InitializeWalls()
        {
            float[] xArray = {20, 20, 20, 20, 20, 20, 20, 20, 20,
                              60, 60, 60,
                              100, 100, 100, 100, 100, 100, 100, 100, 100,
                              140, 140,
                              180, 180, 180, 180, 180, 180, 180, 180, 180,
                              220, 220, 220, 220,
                              260, 260, 260, 260, 260, 260, 260, 260, 260,
                              300, 300, 300,
                              340, 340, 340, 340, 340, 340, 340, 340, 340,
                              380, 380,
                              420, 420, 420, 420, 420, 420, 420, 420, 420, 420,
                              460, 460,
                              500, 500, 500, 500, 500, 500, 500, 500, 500, 500,
                              540, 540, 540,
                              580, 580, 580, 580, 580, 580, 580,
                              620, 620, 620, 620,
                              660, 660, 660, 660, 660, 660, 660, 660,
                              700, 700, 700, 700, 700,
                              740, 740, 740, 740, 740, 740, 740, 740,
                              780, 780};

            float[] yArray = {20, 60, 100, 180, 260, 300, 340, 380, 420,
                              180, 260, 340,
                              20, 60, 100, 180, 220, 260, 340, 380, 420,
                              100, 420,
                              20, 100, 140, 180, 260, 300, 340, 420, 460,
                              20, 100, 180, 340,
                              20, 100, 180, 220, 260, 300, 340, 380, 420,
                              20, 100, 420,
                              20, 100, 140, 180, 260, 300, 340, 380, 420,
                              20, 180,
                              20, 100, 140, 180, 260, 300, 340, 380, 420, 460,
                              100, 260,
                              20, 60, 100, 140, 180, 260, 300, 340, 420, 460,
                              180, 340, 420,
                              20, 60, 100, 180, 220, 260, 340,
                              20, 100, 260, 340,
                              20, 100, 180, 220, 260, 340, 380, 420,
                              20, 100, 180, 260, 420,
                              20, 100, 180, 260, 300, 340, 420, 460,
                              100, 180};

            wall.AddComponentFromPool<TransformComponent>();
            wall.AddComponent(new SpatialFormComponent("Wall"));
            wall.AddComponent(new HealthComponent(3));
            wall.AddComponent(new OrientationComponent("horizontal"));

            wall.GetComponent<TransformComponent>().X = GraphicsDevice.Viewport.Width * 0.75F;
            wall.GetComponent<TransformComponent>().Y = GraphicsDevice.Viewport.Height *.5F;
            wall.Group = "Walls";

            for(int x=0; x<xArray.Length; x++){
                Entity wall = world.CreateEntity();

                wall.AddComponentFromPool<TransformComponent>();
                wall.AddComponent(new SpatialFormComponent("Wall"));

                wall.GetComponent<TransformComponent>().X = xArray[x];
                wall.GetComponent<TransformComponent>().Y = yArray[x];
                wall.Group = "Walls";
            }
        }

    }
}
