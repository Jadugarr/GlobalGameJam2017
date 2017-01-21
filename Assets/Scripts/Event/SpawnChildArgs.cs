using Assets.Scripts.AI;

namespace Assets.Scripts.Event
{
    public class SpawnChildArgs : IEvent
    {
        public ChildType ChildType;

        public SpawnChildArgs(ChildType childType)
        {
            ChildType = childType;
        }
    }
}
