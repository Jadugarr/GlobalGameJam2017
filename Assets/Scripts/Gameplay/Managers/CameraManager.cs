using System;
using Common;
using UnityEngine;

namespace Gameplay.Managers
{
	public class CameraManager : SingletonMonoBehaviour<CameraManager>
	{
		[SerializeField]
		protected Transform MainCamera;

		[SerializeField]
		protected Transform TiltAxis;

		[SerializeField]
		protected float Smoothing = 5f;

		[SerializeField]
		protected float Distance = 2f;

		// targets:
		[SerializeField]
		protected Transform PlayerTarget;

		[SerializeField]
		protected Transform BlackBoardTarget;

		[SerializeField]
		protected Transform BlackBoardPosition;

		[SerializeField]
		protected Transform DoorTarget;

		private Transform target;
		private Vector3 cameraPosition;

		private bool isFollowingPlayer;

		protected void FixedUpdate()
		{
			if(isFollowingPlayer)
			{
				Follow ();
			}
			else
			{
				MoveToTarget ();
			}
		}

		public void FollowPlayer()
		{
			target = PlayerTarget;
			isFollowingPlayer = true;
		}

		public void LookAtBlackBoard( bool instant = false)
		{
			SetCameraPosition (BlackBoardTarget, BlackBoardPosition.position);
			if(instant)
			{
				MainCamera.position = BlackBoardPosition.position;
				MainCamera.rotation = Quaternion.LookRotation(BlackBoardTarget.position - MainCamera.position);
			}
		}

		public void LookAtDoor()
		{
			SetCameraPosition (DoorTarget, MainCamera.position);
		}

		private void SetCameraPosition(Transform target, Vector3 cameraPosition)
		{
			this.target = target;
			this.cameraPosition = cameraPosition;
			isFollowingPlayer = false;
		}

		private void MoveToTarget()
		{
			MainCamera.position = Vector3.Lerp (MainCamera.position, cameraPosition, Smoothing * Time.deltaTime);

			Quaternion newRot = Quaternion.LookRotation(target.position - MainCamera.position);
			MainCamera.rotation = Quaternion.Lerp ( MainCamera.rotation, newRot, Smoothing * Time.deltaTime);
		}

		private void Follow()
		{
			cameraPosition = TiltAxis.position + Distance * (TiltAxis.position - target.position).normalized;
			MoveToTarget ();
		}


	}
}

