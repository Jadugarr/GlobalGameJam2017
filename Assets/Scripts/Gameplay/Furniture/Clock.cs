using System;
using UnityEngine;

namespace Gameplay.Furniture
{
	public class Clock : MonoBehaviour
	{
		[SerializeField]
		protected Transform MinutePointer;

		[SerializeField]
		protected Transform SecondPointer;

		public void SetTimeRatio( float ratio )
		{
			SecondPointer.localRotation = Quaternion.AngleAxis (-360f * ratio, Vector3.up);
			MinutePointer.localRotation = Quaternion.AngleAxis (-30f * ratio, Vector3.up);
		}
	}
}

