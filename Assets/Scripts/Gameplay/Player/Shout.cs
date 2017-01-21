using System;
using UnityEngine;
using Gameplay.Managers;
using Gameplay.Constants;
using UnityEngine.UI;

namespace Gameplay.Player
{
	public class Shout : MonoBehaviour
	{
		private float shoutHoldDuration;
		private float shoutFadeoutDuration;
		private float shoutCooldownDuration;

		private float shoutStrength;
		private float startTimeStamp;
		private float cooldownTimeStamp;

		protected void Start()
		{
			GameOptions options = ClassRoomManager.Instance.GameOptions;
			shoutHoldDuration = options.ShoutHoldDuration;
			shoutFadeoutDuration = options.ShoutFadeoutDuration;
			shoutCooldownDuration = options.ShoutCooldownDuration;
		}

		protected void OnTriggerStay(Collider other)
		{
			if(IsShouting/*other is enemy && enemy.ScareLevel < shoutStrength*/)
			{
				// todo
			}
		}

		protected void FixedUpdate()
		{
			if(shoutStrength > 0f)
			{
				float elapsedShoutTime = Time.realtimeSinceStartup - startTimeStamp;
				shoutStrength = Mathf.Clamp (1f - (elapsedShoutTime - shoutHoldDuration) / shoutFadeoutDuration, 0f, 1f);

				if (shoutStrength == 0f) 
				{
					EndShout ();
				}
			}
		}

		public void StartShout()
		{
			// do not start the shout on cooldown
			if( Time.realtimeSinceStartup > cooldownTimeStamp)
			{
				shoutStrength = 1f;
				startTimeStamp = Time.realtimeSinceStartup;
			}
		}


		public void EndShout()
		{
			// do not end the shout twice
			if (Time.realtimeSinceStartup > cooldownTimeStamp) 
			{
				float heldDownFactor = (Time.realtimeSinceStartup - startTimeStamp) / (shoutHoldDuration + shoutFadeoutDuration);

				cooldownTimeStamp = Time.realtimeSinceStartup + shoutCooldownDuration * heldDownFactor;

				shoutStrength = 0f;
			}
		}

		public bool IsShouting{get{return shoutStrength > 0f;}}

		public bool IsOnCooldown{get{return Time.realtimeSinceStartup < cooldownTimeStamp;}}
	}
}

