using AI.Enums;
using Assets.Scripts.AI;
using Assets.Scripts.Event;
using UnityEngine;

namespace Assets.Scripts.Hazards
{
    public class Hazard : MonoBehaviour
    {
        private EventManager eventManager = EventManager.Instance;
        private Collider hazardCollider;

        void Awake()
        {
            hazardCollider = gameObject.GetComponent<Collider>();
        }

        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Child")
            {
                ChildAI child = other.gameObject.GetComponent<ChildAI>();

                if (child.Behaviour == ChildBehaviourEnum.Scared)
                {
                    eventManager.FireEvent(EventTypes.KidHitHazard, new KidHitHazardArgs(child, this.gameObject));
                }
            }
        }
    }
}
