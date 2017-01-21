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

		public void FollowPlayer(Transform targetPlayer)
		{
			target = targetPlayer;
			isFollowingPlayer = true;
		}

		public void SetCameraPosition(Transform target, Vector3 cameraPosition)
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

