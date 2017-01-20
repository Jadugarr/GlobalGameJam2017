using Common;
using Gameplay.Movement;
using Input.Constants;
using UnityEngine;

namespace Input.Managers
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		public bool PlayerControlsEnabled = true;

		[SerializeField]
		protected PlayerMovement PlayerMovement;


		protected void Update()
		{
			if (UnityEngine.Input.GetButtonDown (InputConstants.A_BUTTON)) 
			{
			}
		}


		protected void FixedUpdate()
		{
			if (PlayerMovement != null && PlayerControlsEnabled) 
			{
				float horizontalAxis = UnityEngine.Input.GetAxis (InputConstants.MOVEMENT_HORIZONTAL) * Time.deltaTime;
				float verticalAxis = UnityEngine.Input.GetAxis (InputConstants.MOVEMENT_VERTICAL) * Time.deltaTime;

				PlayerMovement.Look( horizontalAxis, verticalAxis );
				PlayerMovement.Walk( horizontalAxis, verticalAxis );
			}
		}
	}
}

