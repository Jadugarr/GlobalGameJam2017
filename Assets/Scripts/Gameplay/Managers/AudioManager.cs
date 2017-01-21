using System;
using Common;
using UnityEngine;

namespace Gameplay.Managers
{
	public class AudioManager : SingletonMonoBehaviour<AudioManager>
	{
		[SerializeField]
		private AudioSource KnockKnockSound;

		[SerializeField]
		private AudioSource OpenDoorSound;

		[SerializeField]
		public AudioSource CloseDoorSound;

		[SerializeField]
		private AudioSource BellSound;

		[SerializeField]
		private AudioSource BeforeGameAtmosphereSound;

		[SerializeField]
		private AudioSource ChalkSound;

		public void Chalk()
		{
			Play (ChalkSound);
		}

		public void BeforeGameAtmosphere( bool enabled )
		{
			Play (BeforeGameAtmosphereSound, enabled);
		}

		public void Bell()
		{
			Play (BellSound);
		}

		public void KnockKnock()
		{
			Play (KnockKnockSound);
		}

		public void OpenDoor()
		{
			Play (OpenDoorSound);
		}

		public void CloseDoor()
		{
			Play(CloseDoorSound);
		}

		private void Play( AudioSource sound, bool soundOn = true)
		{
			if( sound != null)
			{
				if(soundOn)
				{
					sound.Play();
				}
				else
				{
					sound.Stop ();
				}
			}
			else
			{
				//Debug.LogWarning ("Could not play sound!");
			}
		}
	}
}

