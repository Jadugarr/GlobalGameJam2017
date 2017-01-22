using System;
using UnityEngine;
using Gameplay.Managers;

namespace GamePlay.Furniture
{
	public class BlackBoard : MonoBehaviour
	{
		[SerializeField]
		protected TextMesh DynamicTime;

		[SerializeField]
		protected TextMesh DynamicScore;

		[SerializeField]
		protected TextMesh DynamicHighScore;

		private string startTextBeforeGame;

		private ClassRoomManager classRoomManager;
		private EventManager eventManager;

		protected void Awake()
		{
			startTextBeforeGame = DynamicHighScore.text;
		}

		protected void Start()
		{
			classRoomManager = ClassRoomManager.Instance;
			eventManager = EventManager.Instance;

			eventManager.RegisterForEvent (EventTypes.GameStart, OnGameStart);
			eventManager.RegisterForEvent (EventTypes.GameEnd, OnGameEnd);

			OnGameEnd (null);
		}

		protected void OnDestroy()
		{
			eventManager.RemoveFromEvent (EventTypes.GameStart, OnGameStart);
			eventManager.RemoveFromEvent (EventTypes.GameEnd, OnGameEnd);
		}

		protected void FixedUpdate()
		{
			DynamicTime.text = classRoomManager.RemainingTimeInSeconds.ToString ();
			DynamicScore.text = 0.ToString (); // TODO
		}

		private void OnGameStart(IEvent evtArgs)
		{
			DynamicTime.transform.parent.gameObject.SetActive (true);
			DynamicHighScore.transform.parent.gameObject.SetActive (false);
		}

		private void OnGameEnd(IEvent evtArgs)
		{
			DynamicTime.transform.parent.gameObject.SetActive (false);
			DynamicHighScore.transform.parent.gameObject.SetActive (true);

			DynamicHighScore.text = string.Format (startTextBeforeGame, PlayerPrefs.GetInt ( "HighScore", 0));
		}
	}
}

