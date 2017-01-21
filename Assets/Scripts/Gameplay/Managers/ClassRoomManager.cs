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

		protected override void Awake()
		{
			base.Awake ();
			// todo move to a betta position, mon
			Init ();
			StartGame ();
		}

		public void Init()
		{
			CurrentGameState = GameStateEnum.BeforeGame;
			initialKidCount = GameOptions.NumberOfKids;
		}

		public void StartGame()
		{
			currentKidCount = GameOptions.NumberOfKids;
			CurrentGameState = GameStateEnum.DoorClosed;
			startTimeStamp = Time.realtimeSinceStartup;

			doorRoutine = StartCoroutine (DoorRoutine());
			clockRoutine = StartCoroutine (ClockRoutine());
			PlayerMovement.Enabled = true;

			// set camera
			CameraManager.Instance.FollowPlayer (PlayerMovement.transform);

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

            eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
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
	        StartCoroutine(StunRoutine());
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
				Debug.Log ("wait for sensei: " + time);
				yield return new WaitForSeconds (time);

				// play knock knock sound
				Door.KnockKnock ();
				time = CurrentTimeToOpenDoor;
				Debug.Log ("wait for open: " + time);
				yield return new WaitForSeconds (time);

				// open door
				Debug.Log ("open door");
				CurrentGameState = GameStateEnum.DoorOpen;
				Door.OpenDoor ();

				time = CurrentTimeToCloseDoor;
				Debug.Log ("wait for close: " + time);
				yield return new WaitForSeconds (time);

				// close door
				Debug.Log ("close door");
				CurrentGameState = GameStateEnum.DoorClosed;
				Door.CloseDoor ();
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

	    #region Stun

	    private IEnumerator StunRoutine()
	    {
	        float stunTimer = GameOptions.StunDuration;
	        PlayerMovement.Enabled = false;

	        while (stunTimer > 0f)
	        {
	            stunTimer -= Time.deltaTime;
	            yield return 0;
	        }

	        PlayerMovement.Enabled = true;
	    }

	    #endregion
    }
}

