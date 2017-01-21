using UnityEngine;
using Assets.Scripts.AI;

namespace Assets.Scripts.Event
{
    public class KidScaredArgs : IEvent
    {
		public ChildAI ScaredKidAI {get; set;}
		public float ShoutStrength {get; set;}
    }
}
