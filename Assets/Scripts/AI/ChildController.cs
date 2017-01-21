using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.AI;
using AI.Enums;

namespace Assets.Scripts.AI
{
    public class ChildController : MonoBehaviour
    {
        [SerializeField]
        private ChildAI[] regularChildren;

        [SerializeField]
        private ChildAI[] bullies;

        [SerializeField]
        private GameObject[] testWayPoints;

        [SerializeField]
        private MeshRenderer movementPlane;

        [SerializeField]
        private GameObject player;

        private Bounds movementBounds;
        private EventManager eventManager = EventManager.Instance;
        private bool bulliesActivated = false;

        private void Awake()
        {
            movementBounds = movementPlane.bounds;
            eventManager.RegisterForEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
            eventManager.RegisterForEvent(EventTypes.KidScared, OnKidScared);
        }

        void OnDestroy()
        {
            eventManager.RemoveFromEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
            eventManager.RemoveFromEvent(EventTypes.KidScared, OnKidScared);
        }

        void Update()
        {
            CheckInput();
            UpdateBullies();
        }


        private void UpdateBullies()
        {
            if (bulliesActivated)
            {
                foreach (ChildAI childAi in bullies)
                {
                    childAi.TargetPosition = player.transform.position;
                }
            }
        }

        private void CheckInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                foreach (ChildAI child in regularChildren)
                {
                    SetRandomPosition(child);
                }

                foreach (ChildAI childAi in bullies)
                {
                    if (bulliesActivated)
                    {
                        childAi.TargetPosition = player.transform.position;
                    }
                    else
                    {
                        SetRandomPosition(childAi);
                    }
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                OnKidScared(new KidScaredArgs());
            }
        }

        private void ActivateBullies()
        {
            bulliesActivated = true;

            foreach (ChildAI childAi in bullies)
            {
				childAi.Behaviour = ChildBehaviourEnum.Aggroed;
                childAi.TargetPosition = player.transform.position;
            }
        }

        private void DeactivateBullies()
        {
            bulliesActivated = false;

            foreach (ChildAI childAi in bullies)
            {
                SetRandomPosition(childAi);
            }
        }

        private void OnKidScared(IEvent evtArgs)
        {
			KidScaredArgs args = (KidScaredArgs)evtArgs;
			ChildAI childAi = args.ScaredKidAI;
			bool isScared = childAi.IsScared( player.transform, args.ShoutStrength);

			if(isScared)
			{
				childAi.Behaviour = ChildBehaviourEnum.Scared;

				Vector3 newTarget = childAi.transform.position + 1f * (childAi.transform.position - player.transform.position);
				newTarget.y = childAi.transform.position.y;

				NavMeshHit hit;
				if (NavMesh.SamplePosition(newTarget, out hit, 1f, NavMesh.AllAreas))
				{
					args.ScaredKidAI.TargetPosition = hit.position;
					Debug.Log ("kid scared:" + childAi.transform.position.x + ", " +childAi.transform.position.z + " -> " + newTarget.x + ", " + newTarget.z);
				}
				else
				{
					Debug.Log ("kid NOT scared:" + childAi.transform.position.x + ", " +childAi.transform.position.z + " -> " + newTarget.x + ", " + newTarget.z);

				}
			}

            ActivateBullies();
        }

        private void OnKidReachedDestination(IEvent eventArgs)
        {
            KidReachedDestinationArgs args = (KidReachedDestinationArgs) eventArgs;

            if (args.ChildAI.ChildType == ChildType.Bully && bulliesActivated)
            {
                eventManager.FireEvent(EventTypes.PlayerHit, new PlayerHitArgs());
                DeactivateBullies();
            }
            else
            {
                SetRandomPosition(args.ChildAI);
            }

        }

        private void SetRandomPosition(ChildAI child)
        {
            Vector3 newTarget = new Vector3(Random.Range(movementBounds.min.x, movementBounds.max.x), 1f, Random.Range(movementBounds.min.z, movementBounds.max.z));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newTarget, out hit, 1f, NavMesh.AllAreas))
            {
                child.TargetPosition = hit.position;
            }
        }
    }
}
