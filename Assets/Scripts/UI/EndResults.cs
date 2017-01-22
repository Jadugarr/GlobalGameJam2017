using System;
using Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.Managers;

namespace UI
{
	public class EndResults : SingletonMonoBehaviour<EndResults>
	{
		public const float TEXT_SPEED = 0.05f;

		[SerializeField]
		protected Text HighScoreText;

		[SerializeField]
		protected Text CurrentScoreText;

		[SerializeField]
		protected Text NewScoreText;

		[SerializeField]
		protected Image Fade;

		private Coroutine routine;
		private bool allShown;
		private Color fadeColor = Color.black;

		public int currentScore {private get; set;}
		public int highScore {private get; set;}

		public void ShowResults()
		{
			allShown = false;
			HighScoreText.text = "";
			CurrentScoreText.text = "";
			NewScoreText.text = "";

			fadeColor.a = 0f;
			Fade.color = fadeColor;
			Fade.gameObject.SetActive (true);

			routine = StartCoroutine (ShowResultsRoutine());
		}

		public bool IsFinished{get{return allShown;}}


		public void Skip()
		{
			if(!allShown)
			{
				if(routine != null)
				{
					StopCoroutine (routine);
					routine = null;
					allShown = true;

					HighScoreText.text = "HighScore: " + highScore;
					CurrentScoreText.text = "Your Score: " + currentScore;
					if(currentScore > highScore)
					{
						NewScoreText.text = "NEW HIGHSCORE!!!";
					}
					AudioManager.Instance.Chalk (false);
				}
			}
			else
			{

				HighScoreText.text = "";
				CurrentScoreText.text = "";
				NewScoreText.text = "";
				routine = StartCoroutine (FadeInRoutine());
			}
		}

		private IEnumerator FadeInRoutine()
		{
			while(Fade.color.a > 0f)
			{
				fadeColor.a -= 0.01f;
				Fade.color = fadeColor;

				yield return null;
			}
			routine = null;
			Fade.gameObject.SetActive (false);
		}

		private IEnumerator ShowResultsRoutine()
		{
			while(Fade.color.a < 1f)
			{
				fadeColor.a += 0.01f;
				Fade.color = fadeColor;

				yield return null;
			}

			AudioManager.Instance.Chalk (true);
			string highscoreText = "HighScore: " + highScore;
			for (int i = 0, len = highscoreText.Length; i < len; i++) 
			{
				HighScoreText.text += highscoreText [i];
				yield return new WaitForSeconds (TEXT_SPEED);
			}

			string yourScoreText = "Your Score: " + currentScore;
			for (int i = 0, len = yourScoreText.Length; i < len; i++) 
			{
				CurrentScoreText.text += yourScoreText [i];
				yield return new WaitForSeconds (TEXT_SPEED);
			}

			if(currentScore > highScore)
			{
				yield return new WaitForSeconds (0.5f);

				string newText = "NEW HIGHSCORE!!!";
				for (int i = 0, len = newText.Length; i < len; i++) 
				{
					NewScoreText.text += newText [i];
					yield return new WaitForSeconds (TEXT_SPEED);
				}
			}
			AudioManager.Instance.Chalk (false);
			allShown = true;
			routine = null;
		}
	}
}

