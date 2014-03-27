

using Artemis.Interface;
namespace Fred.Components
{
    class MenuSelectionComponent : IComponent
    {
        public bool IsSelected { get; set; }
        public int CurrentSelection { get; set; }
        public int IndexNumber { get; set; }
        public int NumberOfMenuOptions { get; set; }

        public MenuSelectionComponent(int i, int length)
        {
            this.IsSelected = false;
            this.CurrentSelection = 0;
            this.IndexNumber = i;
            this.NumberOfMenuOptions = length;
        }
    }
}
