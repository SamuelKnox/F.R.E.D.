using Artemis;
using Artemis.Attributes;
using Artemis.Blackboard;
using Artemis.Manager;
using Artemis.System;
using Fred.Components;
using Microsoft.Xna.Framework;
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

        public GoodWinConditionSystem()
            : base("GOOD_PLAYER")
        {
        }

        public override void LoadContent()
        {
            exit = BlackBoard.GetEntry<Entity>("Exit");
            game = BlackBoard.GetEntry<Game>("Game");
        }

        public override void Process(Entity entity)
        {
            if (exit != null)
            {
                TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
                NearbyGridsComponent nearbyGridsComponent = entity.GetComponent<NearbyGridsComponent>();
                Console.WriteLine(nearbyGridsComponent.CurrentGrid.X + "  " + nearbyGridsComponent.CurrentGrid.Y);
                Console.WriteLine(exit.GetComponent<NearbyGridsComponent>().CurrentGrid.X + "  " + exit.GetComponent<NearbyGridsComponent>().CurrentGrid.Y);
                if (nearbyGridsComponent.CurrentGrid == exit.GetComponent<NearbyGridsComponent>().CurrentGrid)
                {
                    game.Exit();
                }
            }
        }
    }
}
