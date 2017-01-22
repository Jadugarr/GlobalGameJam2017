using System.Collections.Generic;
using Assets.Scripts.Event;
using UnityEngine;
using UnityEngine.AI;
using AI.Enums;
using Gameplay.Managers;

namespace Assets.Scripts.AI
{
    public class ChildController : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer movementPlane;

        [SerializeField]
        private GameObject player;

		[SerializeField]
		private GameObject SoulParticleTemplate;

        private Bounds movementBounds;
        private EventManager eventManager = EventManager.Instance;
        private bool bulliesActivated = false;

        private List<ChildAI> activeBullies = new List<ChildAI>(2);
        private List<ChildAI> children;

        private void Awake()
        {
            children = new List<ChildAI>(FindObjectsOfType<ChildAI>());
            movementBounds = movementPlane.bounds;
            eventManager.RegisterForEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
            eventManager.RegisterForEvent(EventTypes.KidScared, OnKidScared);
            eventManager.RegisterForEvent(EventTypes.KidHitHazard, OnKidHitHazard);
            eventManager.RegisterForEvent(EventTypes.GameStart, OnGameStart);
            eventManager.RegisterForEvent(EventTypes.ChildSpawned, OnChildSpawned);
            eventManager.RegisterForEvent(EventTypes.GameEnd, OnGameEnd);
        }

        void OnDestroy()
        {
            eventManager.RemoveFromEvent(EventTypes.KidReachedDestination, OnKidReachedDestination);
            eventManager.RemoveFromEvent(EventTypes.KidScared, OnKidScared);
            eventManager.RemoveFromEvent(EventTypes.KidHitHazard, OnKidHitHazard);
            eventManager.RemoveFromEvent(EventTypes.GameStart, OnGameStart);
            eventManager.RemoveFromEvent(EventTypes.ChildSpawned, OnChildSpawned);
            eventManager.RemoveFromEvent(EventTypes.GameEnd, OnGameEnd);
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
                foreach (ChildAI childAi in activeBullies)
                {
                    childAi.TargetPosition = player.transform.position;
                }
            }
        }

        private void OnGameStart(IEvent evtArgs)
        {
            ActivateChildren();
        }

        private void OnGameEnd(IEvent evtArgs)
        {
            if (children.Count > 0)
            {
                ChildAI currentChild;

                for (int i = children.Count-1; i >= 0; i--)
                {
                    currentChild = children[i];
                    Destroy(currentChild.gameObject);
                    children.Remove(currentChild);

                    if (activeBullies.Contains(currentChild))
                    {
                        activeBullies.Remove(currentChild);
                    }
                }
            }
        }

        private void CheckInput()
        {
            //if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            //{
            //    ActivateChildren();
            //}

            if (UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                eventManager.FireEvent(EventTypes.SpawnChild, new SpawnChildArgs(ChildType.Regular));
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                eventManager.FireEvent(EventTypes.SpawnChild, new SpawnChildArgs(ChildType.Bully));
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                eventManager.FireEvent(EventTypes.KidHitHazard, new KidHitHazardArgs(children[Random.Range(0, children.Count)], null));
            }
        }

        private void OnChildSpawned(IEvent evtArgs)
        {
            ChildSpawnedArgs args = (ChildSpawnedArgs) evtArgs;

            children.Add(args.ChildAi);
            SetRandomPosition(args.ChildAi);
        }

        private void ActivateChildren()
        {
            foreach (ChildAI child in children)
            {
                SetRandomPosition(child);
            }
        }

        private void ActivateBullies()
        {
            bulliesActivated = true;

            foreach (ChildAI childAi in children)
            {
                if (childAi.ChildType == ChildType.Bully && childAi.Behaviour != ChildBehaviourEnum.Scared)
                {
                    childAi.Behaviour = ChildBehaviourEnum.Aggroed;
                    childAi.TargetPosition = player.transform.position;

                    if (activeBullies.Contains(childAi) == false)
                    {
                        activeBullies.Add(childAi);
                    }
                }
            }
        }

        private void DeactivateBullies()
        {
            bulliesActivated = false;

            foreach (ChildAI childAi in activeBullies)
            {
                childAi.Behaviour = ChildBehaviourEnum.Walking;
                SetRandomPosition(childAi);
            }

            activeBullies.Clear();
        }

        private void OnKidHitHazard(IEvent evtArgs)
        {
            KidHitHazardArgs args = (KidHitHazardArgs) evtArgs;

            if (children.Contains(args.ChildAi))
            {
                children.Remove(args.ChildAi);

                if (activeBullies.Contains(args.ChildAi))
                {
                    activeBullies.Remove(args.ChildAi);
                }
            }

			Instantiate (SoulParticleTemplate);
			SoulParticleTemplate.transform.position = args.ChildAi.transform.position;
            Destroy(args.ChildAi.gameObject);
        }

        private void OnKidScared(IEvent evtArgs)
        {
			KidScaredArgs args = (KidScaredArgs)evtArgs;
			ChildAI childAi = args.ScaredKidAI;
            if (childAi.Behaviour != ChildBehaviourEnum.Scared)
            {
                bool isScared = childAi.IsScared(player.transform, args.ShoutStrength);

                if (isScared)
                {
                    childAi.Behaviour = ChildBehaviourEnum.Scared;
                    
                    Vector3 newTarget = childAi.transform.position + 2f * (childAi.transform.position - player.transform.position);
                    newTarget.y = childAi.transform.position.y;

                    SetPosition(childAi, newTarget);

                    if (childAi.ChildType == ChildType.Bully)
                    {
                        DeactivateBully(childAi, ChildBehaviourEnum.Scared);
                    }

                    AudioManager.Instance.PlayScaredSound();
                }
                ActivateBullies();
            }
        }

        private void DeactivateBully(ChildAI child, ChildBehaviourEnum behaviour)
        {
            if (activeBullies.Contains(child))
            {
                activeBullies.Remove(child);
                child.Behaviour = behaviour;

                if (activeBullies.Count == 0)
                {
                    bulliesActivated = false;
                    Debug.Log("All Bullies deactivated");
                }
                Debug.Log("Bully deactivated");
            }
        }

        private void OnKidReachedDestination(IEvent eventArgs)
        {
            KidReachedDestinationArgs args = (KidReachedDestinationArgs) eventArgs;

            if (args.ChildAI.ChildType == ChildType.Bully && activeBullies.Contains(args.ChildAI))
            {
                eventManager.FireEvent(EventTypes.PlayerHit, new PlayerHitArgs());
                DeactivateBullies();
            }
            else
            {
                SetRandomPosition(args.ChildAI);
            }

            args.ChildAI.Behaviour = ChildBehaviourEnum.Walking;
        }

        private void SetRandomPosition(ChildAI child)
        {
            Vector3 newTarget = new Vector3(Random.Range(movementBounds.min.x, movementBounds.max.x), 1f, Random.Range(movementBounds.min.z, movementBounds.max.z));
            child.TargetPosition = FindNextPosOnNavMesh(newTarget);
        }

        private void SetPosition(ChildAI child, Vector3 targetPosition)
        {
            child.TargetPosition = FindNextPosOnNavMesh(targetPosition);
        }

        private Vector3 FindNextPosOnNavMesh(Vector3 targetPosition)
        {
            NavMeshHit hit;
            float range = 0f;
            do
            {
                range++;
                if (NavMesh.SamplePosition(targetPosition, out hit, range, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            } while (!hit.hit);

            return Vector3.zero;
        }
    }
}
