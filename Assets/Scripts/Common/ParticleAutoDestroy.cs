using System;
using UnityEngine;
using System.Collections;

namespace Common
{
	public class ParticleAutoDestroy : MonoBehaviour
	{
		[SerializeField]
		protected ParticleSystem ps;

		protected IEnumerator Start()
		{
			yield return new WaitForSeconds (ps.main.startLifetime.constant);
			Destroy ( gameObject );
		}
	}
}

