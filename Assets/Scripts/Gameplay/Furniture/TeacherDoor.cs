using System;
using UnityEngine;
using Gameplay.Managers;
using UnityEditor.Animations;

namespace Gameplay.Furniture
{
	public class TeacherDoor : MonoBehaviour
	{
		[SerializeField]
		protected Animator Animator;

		public void OpenDoor()
		{
			// play open sound
			AudioManager.Instance.OpenDoor ();

			// rotate door
			Animator.SetTrigger ("Open");
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

