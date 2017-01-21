using Assets.Scripts.AI;
using UnityEngine;

namespace Assets.Scripts.Event
{
    public class KidHitHazardArgs : IEvent
    {
        public ChildAI ChildAi;
        public GameObject Hazard;

        public KidHitHazardArgs(ChildAI childAi, GameObject hazard)
        {
            ChildAi = childAi;
            Hazard = hazard;
        }
    }
}
