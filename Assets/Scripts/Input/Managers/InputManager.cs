using Assets.Scripts.Input;
using Common;
using Gameplay.Movement;
using Input.Constants;
using UnityEngine;
using Gameplay.Player;
using Gameplay.Managers;
using Gameplay.Enums;

namespace Input.Managers
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

	    private AudioClip microphoneRecording;

	    void Awake()
	    {
	    }

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
                if (!Shout.IsShouting && !Shout.IsOnCooldown && MicInput.MicLoudness > 0.05f)
                {
                    Shout.StartShout();
                }
                else if (Shout.IsShouting && MicInput.MicLoudness <= 0.05f)
                {
                    Shout.EndShout();
                }

                //if (!Shout.IsShouting && !Shout.IsOnCooldown && UnityEngine.Input.GetButton (InputConstants.A_BUTTON)) 
                //{
                //	Shout.StartShout ();
                //}
                //else if (Shout.IsShouting && UnityEngine.Input.GetButtonUp (InputConstants.A_BUTTON)) 
                //{
                //	Shout.EndShout ();
                //}

                if (Shout.IsShouting && ClassRoomManager.CurrentGameState == GameStateEnum.DoorOpen)
				{
					ClassRoomManager.CaughtByTeacher ();
				}
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

