using System;
using Common;
using UnityEngine;
using AssemblyCSharp;

namespace Gameplay.Managers
{
	public class AudioManager : SingletonMonoBehaviour<AudioManager>
	{
		[SerializeField]
		protected AudioOptions AudioOptions;

		public void KnockKnock()
		{
			Play (AudioOptions.KnockKnockSound);
		}

		public void OpenDoor()
		{
			Play (AudioOptions.OpenDoorSound);
		}

		public void CloseDoor()
		{
			Play(AudioOptions.CloseDoorSound);
		}

		private void Play( AudioSource sound)
		{
			if( sound != null)
			{
				sound.Play();
			}
			else
			{
				Debug.LogWarning ("Could not play sound!");
			}
		}
	}
}

