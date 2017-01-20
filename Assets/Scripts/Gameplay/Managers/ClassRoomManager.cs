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
		public GameOptionsConstants GameOptions;

		[SerializeField]
		public TeacherDoor Door;

		public GameStateEnum CurrentGameState { get; private set;}

		private int currentKidCount;
		private int initialKidCount;
		private bool gameIsRunning;

		private Coroutine doorRoutine;


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
				yield return new WaitForSeconds (CurrentDoorDelay);

				// play knock knock sound
				yield return new WaitForSeconds (CurrentTimeToOpenDoor);
				Door.KnockKnock ();

				// open door
				CurrentGameState = GameStateEnum.DoorOpen;
				yield return new WaitForSeconds (CurrentTimeToCloseDoor);
				Door.OpenDoor ();

				// close door
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

