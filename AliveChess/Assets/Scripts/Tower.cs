using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

	public Action<Tower> TowerClickEvent; 
	private void OnEnable()
	{
		TowerClickEvent = tower => {};
	}

	private void OnMouseDown()
	{
		TowerClickEvent.Invoke(this);
	}
}
