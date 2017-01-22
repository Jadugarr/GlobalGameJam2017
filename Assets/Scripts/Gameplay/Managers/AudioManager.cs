using System;
using Common;
using UnityEngine;
using System.Collections;

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

		[SerializeField]
		private AudioSource TeacherCaughtSound;

		[SerializeField]
		private AudioSource BeepSound;

		private Coroutine startGameSoundRoutine;


		public void StartGameSound()
		{
			startGameSoundRoutine = StartCoroutine (StartGameSoundRoutine ());
		}

		protected void OnDestroy()
		{
			StopAllCoroutines ();
		}

		private IEnumerator StartGameSoundRoutine()
		{
			TeacherCaughtSound.Stop ();
			BellSound.Stop ();
			Play (BeepSound);

			yield return new WaitForSeconds (0.3f);

			BeforeGameAtmosphere (true);
			startGameSoundRoutine = null;
		}

		public void Chalk( bool enable)
		{
			Play (ChalkSound, enable);
		}

		public void BeforeGameAtmosphere( bool enabled )
		{
			Play (BeforeGameAtmosphereSound, enabled);

			// stop the possible time offset of this sound
			if(!enabled && startGameSoundRoutine != null)
			{
				StopCoroutine (startGameSoundRoutine);
				startGameSoundRoutine = null;
			}
		}

		public void TeacherCaught()
		{
			Play (TeacherCaughtSound);
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

