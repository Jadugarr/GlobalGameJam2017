 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gameplay.Movement
{
	public class PlayerMovement : MonoBehaviour 
	{
		[SerializeField]
		protected float WalkSpeed = 8f;

		[SerializeField]
		protected float LookRotationSmoothing = 0.5f;

		[SerializeField]
		protected float WalkToLookThreshold = 5f;

		[SerializeField]
		protected CharacterController Controller = null;

		// move and look
		private Vector3 movementForce = Vector3.zero;
		private Quaternion targetRotation;
		private Quaternion lookDirection;

		public bool Enabled = false;

		// edge grabbing
		public bool IsGrabbingEdge{ get; private set; }

		#region Generic
		protected void Awake () 
		{
			lookDirection = Controller.transform.localRotation;
		}

		protected void FixedUpdate()
		{
			if (!Enabled)
				return;
			
			Controller.Move (movementForce);
		}
		#endregion

		#region Input-controlled
		public void Walk( float horizontal, float vertical )
		{
			if (!Enabled)
				return;
			
			// calculate speed
			float angleDiff = Quaternion.Angle (Controller.transform.localRotation, targetRotation);
			float rotationWalkSpeedFactor = 1f - angleDiff / WalkToLookThreshold;

			// only start walking if we somewhat look in the correct direction
			if( rotationWalkSpeedFactor > 0 )
			{
				movementForce.x = WalkSpeed * horizontal * rotationWalkSpeedFactor;
				movementForce.z = WalkSpeed * vertical * rotationWalkSpeedFactor;
			}
		}

		public void Look( float horizontal, float vertical )
		{
			if (!Enabled)
				return;
			
			if (horizontal != 0 || vertical != 0) 
			{
				float targetAngle = Mathf.Acos (vertical / Mathf.Sqrt ( horizontal * horizontal + vertical * vertical )) * 180f / Mathf.PI;
				if (horizontal < 0) 
				{
					targetAngle *= -1;
				}
				targetRotation = Quaternion.AngleAxis (targetAngle, Vector3.up);
			}

			// slowly rotate towards the desired direction
			lookDirection = Quaternion.Lerp ( Controller.transform.localRotation, targetRotation, LookRotationSmoothing);
			Controller.transform.localRotation = lookDirection;
		}
		#endregion
	}
}
