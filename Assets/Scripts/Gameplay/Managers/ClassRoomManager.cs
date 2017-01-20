using System;
using Common;
using UnityEngine;
using Gameplay.Constants;
using System.Collections;
using UnityEngine.Events;
using Gameplay.Enums;
using Gameplay.Furniture;

namespace Gameplay.Managers
{
	public class ClassRoomManager : SingletonMonoBehaviour<ClassRoomManager>
	{
		[SerializeField]
		public GameOptions GameOptions;

		[SerializeField]
		public TeacherDoor Door;

		public GameStateEnum CurrentGameState { get; private set;}

		private int currentKidCount;
		private int initialKidCount;
		private bool gameIsRunning;

		private Coroutine doorRoutine;

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
			currentKidCount = GameOptions.NumberOfKids;
		}

		public void StartGame()
		{
			CurrentGameState = GameStateEnum.DoorClosed;
			doorRoutine = StartCoroutine (DoorRoutine());
		}

		public void EndGame()
		{
			CurrentGameState = GameStateEnum.GameEnd;
			StopCoroutine (doorRoutine);
		}

		public void KillKid()
		{
			currentKidCount--;
		}

		public float AliveKidRatio
		{
			get{ return (float) currentKidCount / initialKidCount;}
		}

		#region Door
		private IEnumerator DoorRoutine()
		{
			while(CurrentGameState != GameStateEnum.BeforeGame
				&& CurrentGameState != GameStateEnum.GameEnd)
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
	}
}

