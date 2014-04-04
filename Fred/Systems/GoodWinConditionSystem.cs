using Artemis;
using Artemis.Attributes;
using Artemis.Blackboard;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class GoodWinConditionSystem : TagSystem
    {
        Entity exit;
        Game game;
        Entity maze;
        EntityWorld world;
        GraphicsDevice graphicsDevice;
        ContentManager content;
        int wallWidth;
        int wallHeight;
        public GoodWinConditionSystem()
            : base("GOOD_PLAYER")
        {
        }

        public override void LoadContent()
        {

            content = BlackBoard.GetEntry<ContentManager>("ContentManager");
            wallWidth = content.Load<Texture2D>("wall").Width;
            wallHeight = content.Load<Texture2D>("wall").Height;
            world = BlackBoard.GetEntry<EntityWorld>("EntityWorld");
            graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
            exit = BlackBoard.GetEntry<Entity>("Exit");
            game = BlackBoard.GetEntry<Game>("Game");
            maze = BlackBoard.GetEntry<Entity>("Maze");
        }

        public override void Process(Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            if (transformComponent.X > maze.GetComponent<MazeComponent>().Width * wallWidth || transformComponent.Y > maze.GetComponent<MazeComponent>().Height * wallHeight || transformComponent.X < 0 || transformComponent.Y < 0)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("Sounds/winorchestra.wav");

                player.Play();
                world.Clear();
                Entity gameOverText = world.CreateEntity();
                gameOverText.AddComponentFromPool<TransformComponent>();
                gameOverText.AddComponent(new SpatialFormComponent("Text"));
                gameOverText.GetComponent<TransformComponent>().X = graphicsDevice.DisplayMode.Width * 0.5F;
                gameOverText.GetComponent<TransformComponent>().Y = graphicsDevice.DisplayMode.Height * 0.5F;
                gameOverText.AddComponent(new TextComponent("Arial", "WELL PLAYED, FRED!!", Color.Red));


                KeyboardState pressedKey = Keyboard.GetState();
                GamePadState controller = GamePad.GetState(PlayerIndex.One);
                if (pressedKey.IsKeyDown(Keys.RightShift) || controller.Buttons.A == ButtonState.Pressed)
                {
                    game.Exit();
                }

            }


        }
    }

}