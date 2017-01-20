using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI
{
    public class ChildController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] children;

        [SerializeField]
        private GameObject[] testWayPoints;

        void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                NavMeshAgent agent = children[0].GetComponent<NavMeshAgent>();

                agent.SetDestination(testWayPoints[0].transform.position);
            }
        }
    }
}
