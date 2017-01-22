using AI.Enums;
using Assets.Scripts.AI;
using Assets.Scripts.Event;
using Gameplay.Managers;
using UnityEngine;

namespace Assets.Scripts.Hazards
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField]
        private HazardType HazardType;

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

                    if (HazardType == HazardType.Trashcan)
                    {
                        AudioManager.Instance.PlayTrashCanSound();
                    }
                    else if (HazardType == HazardType.Window)
                    {
                        AudioManager.Instance.PlayWindowSound();
                    }
                    else if (HazardType == HazardType.Bookshelf)
                    {
                        AudioManager.Instance.PlayShelfSound();
                    }
                }
            }
        }
    }
}
