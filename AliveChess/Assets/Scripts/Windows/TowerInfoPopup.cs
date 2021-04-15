using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopup : MonoBehaviour
{
	public GameProxy GameProxy;
	public Text NicknameText;
	public Button BattleButton;
	public Button CloseButton;
	private string _playerId;
	private string _yourId;
	
	public void SetData(string id, string yourId)
	{
		NicknameText.text = "Башня принадлежит игроку " + id;
		_playerId = id;
		_yourId = yourId;
	}

	private void OnEnable()
	{
		GameProxy.GameManager.AnswerEvent += () => Destroy(gameObject);
	}

	public void OnBattleButtonClick()
	{
		GameProxy.GameManager.SendInviteToPlayer(_yourId, _playerId);
		BattleButton.gameObject.SetActive(false);
		CloseButton.gameObject.SetActive(false);
		NicknameText.text = "Ожидание игрока...";
	}

	public void OnCloseButtonClick()
	{
		Destroy(gameObject);
	}
	
	private void OnDisable()
	{
		GameProxy.GameManager.AnswerEvent -= () => Destroy(gameObject);
	}
}

