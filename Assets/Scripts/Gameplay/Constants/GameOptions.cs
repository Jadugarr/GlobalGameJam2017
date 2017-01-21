using UnityEngine;

namespace Gameplay.Constants
{
	[CreateAssetMenu(fileName="GameOptions", menuName="Config/GameOptions", order = 1)]
	public class GameOptions : ScriptableObject
	{
		[SerializeField]
		public float StartMinDelay = 10f;

		[SerializeField]
		public float StartMaxDelay = 30f;

		[SerializeField]
		public float EndMinDelay = 5f;

		[SerializeField]
		public float EndMaxDelay = 10f;

		[SerializeField]
		public float StartTimeToOpenDoor = 3f;

		[SerializeField]
		public float EndTimeToOpenDoor = 0.7f;

		[SerializeField]
		public float MinTimeToCloseDoor = 2f;

		[SerializeField]
		public float MaxTimeToCloseDoor = 5f;

		[SerializeField]
		public float GameDuration = 300f;

	    [SerializeField]
        public float StunDuration = 3f;

		[SerializeField]
		public float ShoutHoldDuration = 3f;

		[SerializeField]
		public float ShoutFadeoutDuration = 2f;

		[SerializeField]
		public float ShoutCooldownDuration = 2f;

	    [SerializeField]
        public float scaredSpeed;

	    [SerializeField]
        public float defaultSpeed;

	    [SerializeField]
        public int startingRegularChildren;

	    [SerializeField]
        public int startingBullies;
	}
}

