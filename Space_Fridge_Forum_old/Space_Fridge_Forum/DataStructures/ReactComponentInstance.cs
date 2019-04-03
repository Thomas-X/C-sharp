using Space_Fridge_Forum.Enums;

namespace Space_Fridge_Forum.DataStructures
{
    public class ReactComponentInstance
    {
        public object State { get; set; }
        public Components Component { get; set; }
        public string AppId { get; set; }
    }
}