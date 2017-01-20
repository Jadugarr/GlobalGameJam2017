using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Common
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance = null;
		public static T Instance
		{
			get {
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			_instance = FindObjectOfType<T>();
		}
	}
}

