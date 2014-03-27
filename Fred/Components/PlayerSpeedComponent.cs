using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.Components
{
    class PlayerSpeedComponent : IComponent
    {
        public float MaxSpeed { get; set; }
        public float Acceleration { get; set; }
        public float Friction { get; set; }
        public int KeysPressed { get; set; }

        public PlayerSpeedComponent()
        {
            this.MaxSpeed = 0.15F;
            //this.Acceleration = 0.0008F * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            //this.Friction = 0.0005f * TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
            this.KeysPressed = 0;
        }
    }
}
