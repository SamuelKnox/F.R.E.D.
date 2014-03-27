﻿
using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
namespace Fred.Systems
{

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class MenuSystem : EntityProcessingSystem<MenuSelectionComponent>
    {

        GraphicsDevice graphicsDevice;
        EntityWorld world;
        ContentManager content;
        Game game;

        public override void LoadContent()
        {
            graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            world = BlackBoard.GetEntry<EntityWorld>("EntityWorld");
            content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            game = BlackBoard.GetEntry<Game>("Game");
        }


        protected override void Process(Entity entity, MenuSelectionComponent menuSelectionComponent)
        {
            CooldownComponent cooldownComponent = entity.GetComponent<CooldownComponent>();
            KeyboardState pressedKey = Keyboard.GetState();
            GamePadState controller = GamePad.GetState(PlayerIndex.One);
            if (cooldownComponent.IsMenuSelectReady)
            {
                if (pressedKey.IsKeyDown(Keys.Up) || controller.ThumbSticks.Left.Y > 0)
                {
                    menuSelectionComponent.CurrentSelection--;
                    cooldownComponent.ResetMenuSelectCooldown();
                    menuSelectionComponent.CurrentSelection = (menuSelectionComponent.CurrentSelection < 0 ? menuSelectionComponent.NumberOfMenuOptions - 1 : menuSelectionComponent.CurrentSelection);
                }
                else if (pressedKey.IsKeyDown(Keys.Down) || controller.ThumbSticks.Left.Y < 0)
                {
                    menuSelectionComponent.CurrentSelection++;
                    cooldownComponent.ResetMenuSelectCooldown();
                    menuSelectionComponent.CurrentSelection = menuSelectionComponent.CurrentSelection % menuSelectionComponent.NumberOfMenuOptions;

                }

            }

            if (menuSelectionComponent.CurrentSelection == menuSelectionComponent.IndexNumber)
            {
                menuSelectionComponent.IsSelected = true;
            }
            else
            {
                menuSelectionComponent.IsSelected = false;
            }



            if (menuSelectionComponent.IsSelected)
            {
                entity.GetComponent<TextComponent>().Color = Color.Red;
            }
            else
            {
                entity.GetComponent<TextComponent>().Color = Color.White;
            }



            if (pressedKey.IsKeyDown(Keys.Enter) || controller.Buttons.A == ButtonState.Pressed)
            {

                if (menuSelectionComponent.IsSelected)
                {
                    switch (menuSelectionComponent.CurrentSelection)
                    {
                        //new Game
                        case (0):


                            List<Vector2> playerStartPositions = InitializeWalls();
                            InitializeGoodPlayers(playerStartPositions[0]);
                            InitializeEvilPlayers(playerStartPositions[1]);
                            InitializeTimer();
                            break;

                        //quit
                        case (1):
                            game.Exit();
                            break;
                    }
                }

                entity.Delete();

            }

        }



        void InitializeGoodPlayers(Vector2 playerStartPosition)
        {
            Entity player = world.CreateEntity();

            player.AddComponentFromPool<TransformComponent>();
            player.AddComponent(new SpatialFormComponent("GoodPlayer"));
            player.AddComponent(new HealthComponent(10));
            player.AddComponent(new DamageComponent(5, .25));
            player.AddComponent(new VelocityComponent());
            player.AddComponent(new CooldownComponent());

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
            enemy.AddComponent(new HealComponent(3));
            enemy.AddComponent(new CooldownComponent());

            enemy.GetComponent<TransformComponent>().X = playerStartPosition.X;
            enemy.GetComponent<TransformComponent>().Y = playerStartPosition.Y;
            enemy.Tag = "BAD_PLAYER";
        }
        List<Vector2> InitializeWalls()
        {
            int mazesNum = Directory.GetFiles("Mazes/", "*.*", SearchOption.TopDirectoryOnly).Length;
            Random rand = new Random();
            StreamReader file = new StreamReader("Mazes/maze" + rand.Next(0, mazesNum) + ".txt");
            string line = file.ReadLine();
            string[] firstLine = line.Split(' ');
            int wallSize = content.Load<Texture2D>("wall").Width;
            int width = int.Parse(firstLine[0]);
            int height = int.Parse(firstLine[1]);
            List<Vector2> playerStartPoints = new List<Vector2>();
            playerStartPoints.Add(new Vector2(graphicsDevice.DisplayMode.Width * 0.5F, graphicsDevice.DisplayMode.Height * 0.5F));
            playerStartPoints.Add(new Vector2(graphicsDevice.DisplayMode.Width * 0.8F, graphicsDevice.DisplayMode.Height * 0.5F));
            int[,] mazeLayout = new int[width, height];

            while (file.Peek() != -1)
            {
                for (int i = 0; i < height; ++i)
                {

                    line = file.ReadLine();

                    for (int j = 0; j < line.Length; ++j)
                    {
                        mazeLayout[j, i] = int.Parse(line[j] + "");
                    }
                }


                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        if (mazeLayout[i, j] == 0)
                        {
                            Entity wall = world.CreateEntity();

                            wall.AddComponentFromPool<TransformComponent>();
                            wall.AddComponent(new SpatialFormComponent("Wall"));
                            wall.AddComponent(new HealthComponent(0, 10));

                            wall.GetComponent<TransformComponent>().X = i * wallSize;
                            wall.GetComponent<TransformComponent>().Y = j * wallSize;
                            wall.Group = "Walls";
                        }
                        else if (mazeLayout[i, j] == 1)
                        {
                            Entity wall = world.CreateEntity();

                            wall.AddComponentFromPool<TransformComponent>();
                            wall.AddComponent(new SpatialFormComponent("Wall"));
                            wall.AddComponent(new HealthComponent(5, 10));

                            wall.GetComponent<TransformComponent>().X = i * wallSize;
                            wall.GetComponent<TransformComponent>().Y = j * wallSize;
                            wall.Group = "Walls";
                        }
                        else if (mazeLayout[i, j] == 2)
                        {
                            Entity wall = world.CreateEntity();

                            wall.AddComponentFromPool<TransformComponent>();
                            wall.AddComponent(new SpatialFormComponent("Wall"));
                            wall.AddComponent(new HealthComponent(1000000000));

                            wall.GetComponent<TransformComponent>().X = i * wallSize;
                            wall.GetComponent<TransformComponent>().Y = j * wallSize;
                            wall.Group = "Walls";
                        }
                        else if (mazeLayout[i, j] == 3)
                        {
                            float x = i * wallSize + wallSize / 2;
                            float y = j * wallSize + wallSize / 2;
                            playerStartPoints[0] = new Vector2(x, y);


                            Entity wall = world.CreateEntity();

                            wall.AddComponentFromPool<TransformComponent>();
                            wall.AddComponent(new SpatialFormComponent("Wall"));
                            wall.AddComponent(new HealthComponent(0));

                            wall.GetComponent<TransformComponent>().X = i * wallSize;
                            wall.GetComponent<TransformComponent>().Y = j * wallSize;
                            wall.Group = "Walls";
                        }
                        else if (mazeLayout[i, j] == 4)
                        {
                            float x = i * wallSize + wallSize / 2;
                            float y = j * wallSize + wallSize / 2;
                            playerStartPoints[1] = new Vector2(x, y);


                            Entity wall = world.CreateEntity();

                            wall.AddComponentFromPool<TransformComponent>();
                            wall.AddComponent(new SpatialFormComponent("Wall"));
                            wall.AddComponent(new HealthComponent(0));

                            wall.GetComponent<TransformComponent>().X = i * wallSize;
                            wall.GetComponent<TransformComponent>().Y = j * wallSize;
                            wall.Group = "Walls";
                        }
                    }
                }
            }
            return playerStartPoints;
        }
        void InitializeTimer()
        {
            Entity timer = world.CreateEntity();
            timer.AddComponent(new TextComponent("Arial", "Time", Color.White));
            timer.AddComponent(new SpatialFormComponent("Text"));
            timer.AddComponentFromPool<TransformComponent>();
            timer.GetComponent<TransformComponent>().X = graphicsDevice.DisplayMode.Width * 0.5F;
            timer.GetComponent<TransformComponent>().Y = graphicsDevice.DisplayMode.Height * 0.05F;
            timer.AddComponent(new TimerComponent(30000));

        }

    }
}