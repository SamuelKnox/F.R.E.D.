
using Artemis.Interface;
namespace Fred.Components
{
    class OrientationComponent : IComponent
    {
        public string Orientation { get; set; }

        public OrientationComponent()
            : this(string.Empty)
        {
        }

        public OrientationComponent(string orientation)
        {
            this.Orientation = orientation;
        }
    }
}
