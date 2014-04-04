﻿#region Using Statements
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
        Entity maze;
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
            LoadContent();

            IsMouseVisible = false;

            world = new EntityWorld();
            maze = world.CreateEntity();

            EntitySystem.BlackBoard.SetEntry<ContentManager>("ContentManager", Content);
            EntitySystem.BlackBoard.SetEntry<GraphicsDevice>("GraphicsDevice", GraphicsDevice);
            EntitySystem.BlackBoard.SetEntry<SpriteBatch>("SpriteBatch", spriteBatch);
            EntitySystem.BlackBoard.SetEntry<EntityWorld>("EntityWorld", world);
            EntitySystem.BlackBoard.SetEntry<Entity>("Maze", maze);
            EntitySystem.BlackBoard.SetEntry<Game>("Game", this);

            world.InitializeAll(true);

            InitializeMenu();

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

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            this.world.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            Texture2D background = Content.Load<Texture2D>("title");

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle((int)(GraphicsDevice.DisplayMode.Width * 0.25F), 0, background.Width, background.Height), background.Bounds, Color.White);
            world.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void InitializeMenu()
        {

            String[] items = new String[] { "Play Random Level", "Level 1", "Level 2", "Quit" };
            Entity[] entities = new Entity[items.Length];
            for (int i = 0; i < items.Length; i++)
            {

                Entity menuItem = world.CreateEntity();
                entities[i] = menuItem;
                menuItem.AddComponentFromPool<TransformComponent>();
                menuItem.AddComponent(new MenuSelectionComponent(i, entities.Length));
                menuItem.AddComponent(new CooldownComponent());
                menuItem.AddComponent(new SpatialFormComponent("Level"));
                menuItem.GetComponent<TransformComponent>().X = GraphicsDevice.DisplayMode.Width * 0.5F;
                menuItem.GetComponent<TransformComponent>().Y = (200 + (100 * (1 + i)));
            }


        }

    }
}
