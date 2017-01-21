using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.AI;
using AI.Enums;

namespace Assets.Scripts.AI
{
    public class ChildAI : MonoBehaviour
    {
        [SerializeField]
        private ChildType childType;

		[SerializeField]
		private float ScareThreshold = 0.1f;

        private Vector3 targetPosition;
        private float distance = 2f;

		private NavMeshAgent navAgent;
        private EventManager eventManager = EventManager.Instance;

		public ChildBehaviourEnum Behaviour { get; set;} 

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

		public bool IsScared( Transform playerTransform, float shoutStrength )
		{
			float distanceFallOff = 1f - Vector3.Distance (playerTransform.position, transform.position) / 6f;
			Debug.Log(shoutStrength * distanceFallOff + ">" + ScareThreshold + "?");
			return shoutStrength * distanceFallOff > ScareThreshold;
		}

        protected void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

		protected void Update()
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
