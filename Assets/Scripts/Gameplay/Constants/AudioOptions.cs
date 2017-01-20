using System;
using UnityEngine;

namespace AssemblyCSharp
{
	[CreateAssetMenu(fileName="AudioOptions", menuName="Config/AudioOptions", order = 2)]
	public class AudioOptions : ScriptableObject
	{
		[SerializeField]
		public AudioSource KnockKnockSound;

		[SerializeField]
		public AudioSource OpenDoorSound;

		[SerializeField]
		public AudioSource CloseDoorSound;
	}
}

