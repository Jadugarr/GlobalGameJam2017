﻿using System;
using Common;
using UnityEngine;
using System.Collections;

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

		[SerializeField]
		protected Transform ClockPosition;

		[SerializeField]
		protected Transform ClockTarget;

		[SerializeField]
		protected Transform TeacherTarget;

		[SerializeField]
		protected Transform TeacherPosition;

		[SerializeField]
		protected float TeacherTilt1;

		[SerializeField]
		protected float TeacherTilt2;

		[SerializeField]
		protected float TeacherTilt3;

		private Transform target;
		private Vector3 cameraPosition;

		private bool isFollowingPlayer;

		protected void FixedUpdate()
		{
			if(isFollowingPlayer)
			{
				Follow ();
			}
			else// if(!isDoingTeacherSequence)
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
			SetCameraPosition (BlackBoardTarget, BlackBoardPosition.position, instant);
		}

		public void LookAtClock()
		{
			SetCameraPosition (ClockTarget, ClockPosition.position);
		}

		public void LookAtTeacherCaught()
		{
			StartCoroutine (TeacherCaughtRoutine());
		}

		private IEnumerator TeacherCaughtRoutine()
		{
			Vector3 camPos = TeacherPosition.position;
			SetCameraPosition (TeacherTarget, camPos, true);
			SetTilt (TeacherTilt1);

			yield return new WaitForSeconds (0.65f);
			camPos = TeacherPosition.position + 0.33f * (TeacherTarget.position - TeacherPosition.position);
			SetCameraPosition (TeacherTarget, camPos, true);
			SetTilt (TeacherTilt2);

			yield return new WaitForSeconds (0.6f);
			camPos = TeacherPosition.position + 0.63f * (TeacherTarget.position - TeacherPosition.position);
			SetCameraPosition (TeacherTarget, camPos, true);
			SetTilt (TeacherTilt3);

			yield return new WaitForSeconds (1.5f);
		}

		public void LookAtDoor()
		{
			SetCameraPosition (DoorTarget, MainCamera.position);
		}

		private void SetTilt( float tilt)
		{
			
		}

		private void SetCameraPosition(Transform target, Vector3 cameraPosition, bool instant = false)
		{
			this.target = target;
			this.cameraPosition = cameraPosition;
			isFollowingPlayer = false;

			if(instant)
			{
				MainCamera.position = cameraPosition;
				MainCamera.rotation = Quaternion.LookRotation(target.position - MainCamera.position);
			}
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

