using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameProxy")]
public class GameProxy : ScriptableObject
{
	public GameManager GameManager { get; set; }
}