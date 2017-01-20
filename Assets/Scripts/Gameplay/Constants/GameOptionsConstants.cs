using System;
using UnityEngine;

namespace Gameplay.Constants
{
	public class GameOptionsConstants : ScriptableObject
	{
		[SerializeField]
		public int NumberOfKids = 10;

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
	}
}

