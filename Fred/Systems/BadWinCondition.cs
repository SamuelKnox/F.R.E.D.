

using Artemis;
using Artemis.Attributes;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Fred.Systems
{

    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    class BadWinConditionSystem : TagSystem
    {
        Game game;
        EntityWorld world;
        GraphicsDevice graphicsDevice;
        public BadWinConditionSystem()
            : base("TIMER")
        {
        }
        public override void LoadContent()
        {
            game = BlackBoard.GetEntry<Game>("Game");
            world = BlackBoard.GetEntry<EntityWorld>("EntityWorld");
            graphicsDevice = BlackBoard.GetEntry<GraphicsDevice>("GraphicsDevice");
        }
        public override void Process(Entity entity)
        {
            TimerComponent timerComponent = entity.GetComponent<TimerComponent>();
            if (timerComponent.IsExpired)
            {
                //game.Exit();
                world.Clear();
                Entity gameOverText = world.CreateEntity();
                gameOverText.AddComponentFromPool<TransformComponent>();
                gameOverText.AddComponent(new SpatialFormComponent("Text"));
                gameOverText.GetComponent<TransformComponent>().X = graphicsDevice.DisplayMode.Width * 0.5F;
                gameOverText.GetComponent<TransformComponent>().Y = graphicsDevice.DisplayMode.Height * 0.5F;
                gameOverText.AddComponent(new TextComponent("Arial", "WELL PLAYED, SHERRIFF!!", Color.White));
            }
        }
    }
}

