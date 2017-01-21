using Common;
using Gameplay.Movement;
using Input.Constants;
using UnityEngine;
using Gameplay.Player;

namespace Input.Managers
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		public bool PlayerControlsEnabled = true;

		[SerializeField]
		protected PlayerMovement PlayerMovement;

		[SerializeField]
		protected Shout Shout;

		protected void Update()
		{
			if (!Shout.IsShouting && !Shout.IsOnCooldown && UnityEngine.Input.GetButton (InputConstants.A_BUTTON)) 
			{
				Shout.StartShout ();
			}
			else if (Shout.IsShouting && UnityEngine.Input.GetButtonUp (InputConstants.A_BUTTON)) 
			{
				Shout.EndShout ();
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

