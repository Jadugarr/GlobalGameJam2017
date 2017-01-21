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
			AudioManager.Instance.StartGameSound ();

			CameraManager.Instance.LookAtBlackBoard ( true );
			CurrentGameState = GameStateEnum.BeforeGame;
			initialKidCount = GameOptions.startingRegularChildren + GameOptions.startingBullies;
		}

		public void StartGame()
		{
			// close door if it is still open
			Door.CloseDoor();

			currentKidCount = initialKidCount;
			CurrentGameState = GameStateEnum.DoorClosed;
			startTimeStamp = Time.realtimeSinceStartup;

			doorRoutine = StartCoroutine (DoorRoutine());
			clockRoutine = StartCoroutine (ClockRoutine());
			PlayerMovement.Enabled = true;

			AudioManager.Instance.BeforeGameAtmosphere (false);

			// set camera
			CameraManager.Instance.FollowPlayer ();

            // add event listeners
            eventManager.RegisterForEvent(EventTypes.PlayerHit, OnPlayerHit);
            eventManager.RegisterForEvent(EventTypes.KidHitHazard, OnKidHit);


			eventManager.FireEvent (EventTypes.GameStart, null);
		}

	    private void OnKidHit(IEvent evtArgs)
	    {
	        KillKid();
	    }

		public void CaughtByTeacher()
		{
			AudioManager.Instance.TeacherCaught();
			CameraManager.Instance.LookAtTeacherCaught ();
			EndGame ();
		}

		public void EndGame()
		{
			CurrentGameState = GameStateEnum.GameEnd;
			StopCoroutine (doorRoutine);
			StopCoroutine (clockRoutine);

			PlayerMovement.Enabled = false;

            eventManager.FireEvent(EventTypes.GameEnd, null);

            eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
            eventManager.RemoveFromEvent(EventTypes.KidHitHazard, OnKidHit);

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

			Clock.SetTimeRatio ( 1f );

			AudioManager.Instance.Bell ();
			CameraManager.Instance.LookAtClock ();
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

