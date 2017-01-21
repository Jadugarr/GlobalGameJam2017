using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI
{
    public class ChildAI : MonoBehaviour
    {
        [SerializeField]
        private ChildType childType;

        private Vector3 targetPosition;
        private float distance = 2f;

        private EventManager eventManager = EventManager.Instance;

        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set
            {
                targetPosition = value;
                navAgent.SetDestination(targetPosition);
            }
        }

        public ChildType ChildType
        {
            get { return childType; }
        }

        private NavMeshAgent navAgent;

        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (targetPosition != Vector3.zero)
            {
                if (Vector3.Distance(transform.position, targetPosition) <= distance)
                {
                    eventManager.FireEvent(EventTypes.KidReachedDestination, new KidReachedDestinationArgs()
                    {
                        ChildAI = this
                    });
                }
            }
        }
    }
}
