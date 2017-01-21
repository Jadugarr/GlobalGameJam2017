using System;
using Common;
using UnityEngine;
using Gameplay.Constants;
using System.Collections;
using UnityEngine.Events;
using Gameplay.Enums;
using Gameplay.Furniture;
using Gameplay.Movement;

namespace Gameplay.Managers
{
	public class ClassRoomManager : SingletonMonoBehaviour<ClassRoomManager>
	{
		[SerializeField]
		public GameOptions GameOptions;

		[SerializeField]
		public TeacherDoor Door;

		[SerializeField]
		public Clock Clock;

		[SerializeField]
		public PlayerMovement PlayerMovement;

		private EventManager EventManager = EventManager.Instance;

		public GameStateEnum CurrentGameState { get; private set;}

		private int currentKidCount;
		private int initialKidCount;
		private bool gameIsRunning;
        private EventManager eventManager = EventManager.Instance;

		private Coroutine doorRoutine;
		private Coroutine clockRoutine;
		private float startTimeStamp;

		protected void Start()
		{
			Init ();
		}

		public void Init()
		{
			CameraManager.Instance.LookAtBlackBoard ( true );
			CurrentGameState = GameStateEnum.BeforeGame;
			initialKidCount = GameOptions.NumberOfKids;

			AudioManager.Instance.BeforeGameAtmosphere (true);
		}

		public void StartGame()
		{
			currentKidCount = GameOptions.NumberOfKids;
			CurrentGameState = GameStateEnum.DoorClosed;
			startTimeStamp = Time.realtimeSinceStartup;

			doorRoutine = StartCoroutine (DoorRoutine());
			clockRoutine = StartCoroutine (ClockRoutine());
			PlayerMovement.Enabled = true;

			AudioManager.Instance.BeforeGameAtmosphere (false);

			// set camera
			CameraManager.Instance.FollowPlayer ();

            //Add event listeners
            eventManager.RegisterForEvent(EventTypes.PlayerHit, OnPlayerHit);
		}

		public void EndGame()
		{
			// close door if it is still open
			if(CurrentGameState == GameStateEnum.DoorOpen)
			{
				Door.CloseDoor();
			}

			CurrentGameState = GameStateEnum.GameEnd;
			StopCoroutine (doorRoutine);
			StopCoroutine (clockRoutine);

			PlayerMovement.Enabled = false;
			CameraManager.Instance.LookAtBlackBoard ();

            eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);

			bool isWinning = (currentKidCount == 0);
        }

		public void KillKid()
		{
			currentKidCount--;
		}

		public float AliveKidRatio
		{
			get{ return (float) currentKidCount / initialKidCount;}
		}

	    private void OnPlayerHit(IEvent evtArgs)
	    {
			PlayerMovement.Stun (GameOptions.StunDuration);
	    }

		#region Clock
		private IEnumerator ClockRoutine()
		{
			float duration = GameOptions.GameDuration;
			float passedTime = 0;

			while (passedTime < duration) 
			{
				yield return new WaitForSeconds (1f);

				passedTime = Time.realtimeSinceStartup - startTimeStamp;
				Clock.SetTimeRatio ( passedTime / duration );
			}

			AudioManager.Instance.Bell ();
			EndGame ();
		}
		#endregion

		#region Door
		private IEnumerator DoorRoutine()
		{
			while(true)
			{
				// wait for sensei
				float time = CurrentDoorDelay;
				yield return new WaitForSeconds (time);

				// play knock knock sound
				Door.KnockKnock ();
				time = CurrentTimeToOpenDoor;
				yield return new WaitForSeconds (time);

				// open door
				CameraManager.Instance.LookAtDoor ();

				CurrentGameState = GameStateEnum.DoorOpen;
				Door.OpenDoor ();

				time = CurrentTimeToCloseDoor;
				yield return new WaitForSeconds (time);

				// close door
				CurrentGameState = GameStateEnum.DoorClosed;
				Door.CloseDoor ();

				CameraManager.Instance.FollowPlayer();
			}
		}

		private float CurrentDoorDelay{
			get{
				float minDelay = GameOptions.EndMinDelay + (GameOptions.StartMinDelay - GameOptions.EndMinDelay) * AliveKidRatio;
				float maxDelay = GameOptions.EndMaxDelay + (GameOptions.StartMaxDelay - GameOptions.EndMaxDelay) * AliveKidRatio;

				return UnityEngine.Random.Range ( minDelay, maxDelay );
			}
		}

		private float CurrentTimeToOpenDoor{
			get{
				return GameOptions.EndTimeToOpenDoor + (GameOptions.StartTimeToOpenDoor - GameOptions.EndTimeToOpenDoor) * AliveKidRatio;
			}
		}

		private float CurrentTimeToCloseDoor{
			get{
				return UnityEngine.Random.Range (GameOptions.MinTimeToCloseDoor, GameOptions.MaxTimeToCloseDoor);
			}
		}
        #endregion
    }
}

