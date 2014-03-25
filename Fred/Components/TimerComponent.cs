
using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Fred.Components
{
    class TimerComponent : IComponent
    {
        public float Time { get; set; }

        public TimerComponent(float time)
        {
            this.Time = time;
        }
        public bool IsExpired
        {
            get
            {
                return this.Time <= 0;
            }
        }

    }
}
