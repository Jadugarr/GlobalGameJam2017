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

	    [SerializeField]
        private AudioSource GlobalSource;

	    [SerializeField]
        private AudioClip GameMusic;

	    [SerializeField]
        private AudioClip MenuMusic;

	    [SerializeField]
        private AudioClip HighscoreMusic;

	    [SerializeField]
        private AudioSource ChildAudio;

	    [SerializeField]
        private AudioSource HazardSource;

	    [SerializeField]
        private AudioClip ChildScaredClip;

        [SerializeField]
        private AudioClip TrashCanClip;

        [SerializeField]
        private AudioClip WindowClip;

        [SerializeField]
        private AudioClip ShelfSound;

        [SerializeField]
        private AudioSource SuccessSource;


        public void StartGameSound()
		{
            TeacherCaughtSound.Stop();
            BellSound.Stop();
            //Play (BeepSound);

            //yield return new WaitForSeconds (0.3f);

            BeforeGameAtmosphere(true);
        }

	    public void PlaySuccess()
	    {
	        SuccessSource.Play();
	    }

		protected void OnDestroy()
		{
			StopAllCoroutines ();
		}

	    public void PlayTrashCanSound()
	    {
	        HazardSource.clip = TrashCanClip;
            HazardSource.Play();
        }

        public void PlayWindowSound()
        {
            HazardSource.clip = WindowClip;
            HazardSource.Play();
        }

        public void PlayShelfSound()
        {
            HazardSource.clip = ShelfSound;
            HazardSource.Play();
        }

        public void PlayScaredSound()
	    {
	        ChildAudio.clip = ChildScaredClip;
            ChildAudio.Play();
	    }

	    public void PlayMenuMusic()
	    {
	        GlobalSource.clip = MenuMusic;
            GlobalSource.Play();
	    }

	    public void PlayGameMusic()
	    {
	        GlobalSource.clip = GameMusic;
            GlobalSource.Play();
	    }

	    public void PlayHighScoreMusic()
	    {
	        GlobalSource.clip = HighscoreMusic;
            GlobalSource.Play();
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

		public void Chalk( bool enable)
		{
			Play (ChalkSound, enable);
		}

		public void BeforeGameAtmosphere( bool enabled )
		{
		    GlobalSource.clip = MenuMusic;
            GlobalSource.Play();
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

