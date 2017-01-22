using Assets.Scripts.GameInput;
using Common;
using GameInput.Constants;
using Gameplay.Movement;
using UnityEngine;
using Gameplay.Player;
using Gameplay.Managers;
using Gameplay.Enums;

namespace GameInput.Managers
{
	public class InputManager : SingletonMonoBehaviour<InputManager>
	{
		public bool PlayerControlsEnabled = true;

		[SerializeField]
		protected PlayerMovement PlayerMovement;

		[SerializeField]
		protected ClassRoomManager ClassRoomManager;

		[SerializeField]
		protected Shout Shout;

		protected void Update()
		{
			if(ClassRoomManager.CurrentGameState == GameStateEnum.BeforeGame && UnityEngine.Input.GetButtonDown (InputConstants.A_BUTTON))
			{
				ClassRoomManager.StartGame ();
			}
			else if(ClassRoomManager.CurrentGameState == GameStateEnum.GameEnd && UnityEngine.Input.GetButtonDown (InputConstants.A_BUTTON))
			{
				ClassRoomManager.Init ();
			}
			else
			{
				if (!Shout.IsShouting && !Shout.IsOnCooldown && PlayerMovement.CanMove && IsShouting)
                {
                    Shout.StartShout();
                }
				else if (Shout.IsShouting && !IsShouting)
                {
                    Shout.EndShout();
                }

				if (Shout.IsShouting && !Shout.IsOnCooldown && PlayerMovement.CanMove && ClassRoomManager.CurrentGameState == GameStateEnum.DoorOpen)
				{
					ClassRoomManager.CaughtByTeacher ();
				}
			}
		}

		private bool IsShouting
		{
			get{
				bool shouting = UnityEngine.Input.GetButton (InputConstants.A_BUTTON);
				#if !UNITY_WEBGL
				shouting |= MicInput.MicLoudness > 0.05f;
				#endif

				return shouting;
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

