using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI
{
    public class ChildController : MonoBehaviour
    {
        [SerializeField]
        private ChildAI[] children;

        [SerializeField]
        private GameObject[] testWayPoints;

        [SerializeField]
        private MeshRenderer movementPlane;

        private Bounds movementBounds;
        private EventManager eventManager = EventManager.Instance;

        private void Awake()
        {
            movementBounds = movementPlane.bounds;
            eventManager.RegisterForEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
        }

        void OnDestroy()
        {
            eventManager.RemoveFromEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
        }

        void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                foreach (ChildAI child in children)
                {
                    SetRandomPosition(child);
                }
            }
        }

        private void OnKidReachedDestination(IEvent eventArgs)
        {
            KidReachedDestinationArgs args = (KidReachedDestinationArgs) eventArgs;

            SetRandomPosition(args.ChildAI);
        }

        private void SetRandomPosition(ChildAI child)
        {
            Vector3 newTarget = new Vector3(Random.Range(movementBounds.min.x, movementBounds.max.x), 1f, Random.Range(movementBounds.min.z, movementBounds.max.z));
            child.TargetPosition = newTarget;
        }
    }
}
