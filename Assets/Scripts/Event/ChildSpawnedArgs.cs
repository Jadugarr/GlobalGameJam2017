using Assets.Scripts.AI;

namespace Assets.Scripts.Event
{
    public class ChildSpawnedArgs : IEvent
    {
        public ChildAI ChildAi;

        public ChildSpawnedArgs(ChildAI childAi)
        {
            ChildAi = childAi;
        }
    }
}
