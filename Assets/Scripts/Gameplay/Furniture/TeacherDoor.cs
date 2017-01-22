using UnityEngine;
using Gameplay.Managers;

namespace Gameplay.Furniture
{
	public class TeacherDoor : MonoBehaviour
	{
		[SerializeField]
		protected Animator Animator;

		[SerializeField]
		protected ParticleSystem AttentionParticle;

		public void OpenDoor()
		{
			// play open sound
			AudioManager.Instance.OpenDoor ();

			// rotate door
			Animator.SetTrigger ("Open");

			AttentionParticle.Play ();
		}

		public void CloseDoor()
		{
			// play close sound
			AudioManager.Instance.CloseDoor ();

			// rotate door
			Animator.SetTrigger ("Close");

		}

		public void KnockKnock()
		{
			// play knock sound
			AudioManager.Instance.KnockKnock ();
		}
	}
}

