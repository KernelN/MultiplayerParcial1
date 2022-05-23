﻿using UnityEngine;

namespace Universal.SceneManaging
{
	[System.Serializable] public enum Scenes { proto, gameplay, menu }
	public class SceneGetter : MonoBehaviour
	{
		public Scenes scene;
		public int level = -1;
	}
}