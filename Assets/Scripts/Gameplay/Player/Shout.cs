using System;
using UnityEngine;
using Gameplay.Managers;
using Gameplay.Constants;
using UnityEngine.UI;
using Common.Constants;
using Assets.Scripts.Event;
using Assets.Scripts.AI;

namespace Gameplay.Player
{
	public class Shout : MonoBehaviour
	{
		[SerializeField]
		protected ParticleSystem ShockwaveParticles;

		private EventManager eventManager = EventManager.Instance;

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

			eventManager.RegisterForEvent(EventTypes.GameStart, OnGameStart);
			eventManager.RegisterForEvent(EventTypes.PlayerHit, OnPlayerHit);
		}

		protected void OnDestroy()
		{
			eventManager.RemoveFromEvent(EventTypes.GameStart, OnGameStart);
			eventManager.RemoveFromEvent(EventTypes.PlayerHit, OnPlayerHit);
		}

		protected void OnTriggerStay(Collider other)
		{
			if(IsShouting && other.tag == TagConstants.Child)
			{
				eventManager.FireEvent (EventTypes.KidScared, 
					new KidScaredArgs 
					{ 
						ScaredKidAI = other.GetComponent<ChildAI> (),
						ShoutStrength = shoutStrength
					}
				);
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

			// fix particle direction
			var particleRota = ShockwaveParticles.main.startRotationZ;
			//particleRota.constant = -transform.rotation.eulerAngles.y;
			//var mainEmitter = ShockwaveParticles.main;
			//mainEmitter.startRotationZ = particleRota;
		}

		public void StartShout()
		{
			// do not start the shout on cooldown
			if( Time.realtimeSinceStartup > cooldownTimeStamp)
			{
				shoutStrength = 1f;
				startTimeStamp = Time.realtimeSinceStartup;
				ShockwaveParticles.Play ();
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
				ShockwaveParticles.Stop ();
			}
		}

		private void OnGameStart(IEvent evtArgs)
		{
			cooldownTimeStamp = Time.realtimeSinceStartup + shoutCooldownDuration;
		}

		private void OnPlayerHit(IEvent evtArgs)
		{
			EndShout ();
		}

		public bool IsShouting{get{return shoutStrength > 0f;}}

		public bool IsOnCooldown{get{return Time.realtimeSinceStartup < cooldownTimeStamp;}}
	}
}

