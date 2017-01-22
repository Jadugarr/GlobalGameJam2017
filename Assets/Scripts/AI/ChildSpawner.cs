using Assets.Scripts.Event;
using Gameplay.Managers;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class ChildSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnPoints;

        [SerializeField] private GameObject regularChildTemplate;

		[SerializeField] private GameObject girlChildTemplate;

        [SerializeField] private GameObject bullyTemplate;

        private EventManager eventManager = EventManager.Instance;

        void Awake()
        {
            eventManager.RegisterForEvent(EventTypes.SpawnChild, OnSpawnChild);
            eventManager.RegisterForEvent(EventTypes.GameStart, OnGameStart);
        }

        void OnDestroy()
        {
            eventManager.RemoveFromEvent(EventTypes.SpawnChild, OnSpawnChild);
            eventManager.RemoveFromEvent(EventTypes.GameStart, OnGameStart);
        }

        private void OnGameStart(IEvent evtArgs)
        {
            for (int i = 0; i < ClassRoomManager.Instance.GameOptions.startingRegularChildren; i++)
            {
                SpawnChild(ChildType.Regular);
            }
            for (int i = 0; i < ClassRoomManager.Instance.GameOptions.startingBullies; i++)
            {
                SpawnChild(ChildType.Bully);
            }
        }

        private void OnSpawnChild(IEvent evtArgs)
        {
            SpawnChildArgs args = (SpawnChildArgs) evtArgs;
            
            SpawnChild(args.ChildType);
        }

        private void SpawnChild(ChildType childType)
        {
            GameObject spawnedChild;
            Transform spawnPoint = GetRandomSpawnpoint();

            if (childType == ChildType.Regular)
            {
				if(UnityEngine.Random.value < 0.35f)
				{
					spawnedChild = GameObject.Instantiate(girlChildTemplate, spawnPoint, false);
				}
				else
				{
					spawnedChild = GameObject.Instantiate(regularChildTemplate, spawnPoint, false);
				}
            }
            else
            {
                spawnedChild = GameObject.Instantiate(bullyTemplate, spawnPoint, false);
            }

            eventManager.FireEvent(EventTypes.ChildSpawned, new ChildSpawnedArgs(spawnedChild.GetComponent<ChildAI>()));
        }

        private Transform GetRandomSpawnpoint()
        {
            int index = Random.Range(0, spawnPoints.Length);
            return spawnPoints[index];
        }
    }
}
