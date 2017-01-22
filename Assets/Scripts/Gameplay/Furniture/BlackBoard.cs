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

	    private int points = 0;

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
            eventManager.RegisterForEvent(EventTypes.KidHitHazard, OnKidFuckingRekt);

            DynamicTime.transform.parent.gameObject.SetActive(false);
            DynamicHighScore.transform.parent.gameObject.SetActive(true);

            DynamicHighScore.text = string.Format(startTextBeforeGame, PlayerPrefs.GetInt("HighScore", 0));
        }

		protected void OnDestroy()
		{
			eventManager.RemoveFromEvent (EventTypes.GameStart, OnGameStart);
			eventManager.RemoveFromEvent (EventTypes.GameEnd, OnGameEnd);
            eventManager.RemoveFromEvent(EventTypes.KidHitHazard, OnKidFuckingRekt);
        }

		protected void FixedUpdate()
		{
			DynamicTime.text = classRoomManager.RemainingTimeInSeconds.ToString ();
			DynamicScore.text = points.ToString();
		}

		private void OnGameStart(IEvent evtArgs)
		{
		    points = 0;
			DynamicTime.transform.parent.gameObject.SetActive (true);
			DynamicHighScore.transform.parent.gameObject.SetActive (false);
		}

		private void OnGameEnd(IEvent evtArgs)
		{
		    points += (classRoomManager.RemainingTimeInSeconds * classRoomManager.GameOptions.PointsPerRemainingSecond);

		    int currentHighscore = PlayerPrefs.GetInt("HighScore", 0);

		    if (points > currentHighscore)
            {
                PlayerPrefs.SetInt("HighScore", points);
            }

			DynamicTime.transform.parent.gameObject.SetActive (false);
			DynamicHighScore.transform.parent.gameObject.SetActive (true);

			DynamicHighScore.text = string.Format (startTextBeforeGame, PlayerPrefs.GetInt ( "HighScore", 0));
		}

	    private void OnKidFuckingRekt(IEvent evtArgs)
	    {
	        points += ClassRoomManager.Instance.GameOptions.PointsPerKid;
	    }
	}
}

