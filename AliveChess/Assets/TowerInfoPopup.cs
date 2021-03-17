using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopup : MonoBehaviour
{

	public Text NicknameText;

	public void SetData(string nickName)
	{
		NicknameText.text = "Башня принадлежит игроку " + nickName;
	}

	public void OnCloseButtonClick()
	{
		Destroy(gameObject);
	}
}

