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

	    [SerializeField]
        private AudioSource TeacherSound;

	    [SerializeField]
        private AudioClip[] PossibleTeacherLookSounds;

	    [SerializeField]
        private AudioClip TeacherLostSound;

		private Coroutine startGameSoundRoutine;


		public void StartGameSound()
		{
            TeacherCaughtSound.Stop();
            BellSound.Stop();
            //Play (BeepSound);

            //yield return new WaitForSeconds (0.3f);

            BeforeGameAtmosphere(true);
        }

		protected void OnDestroy()
		{
			StopAllCoroutines ();
		}

	    public void PlayTeacherLookSound()
	    {
	        TeacherSound.clip = PossibleTeacherLookSounds[Random.Range(0, PossibleTeacherLookSounds.Length)];
            TeacherSound.Play();
	    }

	    public void PlayTeacherLostSound()
	    {
	        TeacherSound.clip = TeacherLostSound;
            TeacherSound.Play();
	    }

		public void Chalk()
		{
			Play (ChalkSound);
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

