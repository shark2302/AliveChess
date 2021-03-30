using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoPopup : MonoBehaviour
{
	public GameProxy GameProxy;
	public Text NicknameText;
	private string _playerId;
	private string _yourId;
	
	public void SetData(string id, string yourId)
	{
		NicknameText.text = "Башня принадлежит игроку " + id;
		_playerId = id;
		_yourId = yourId;
	}

	public void OnBattleButtonClick()
	{
		GameProxy.GameManager.SendInviteToPlayer(_yourId, _playerId);
	}

	public void OnCloseButtonClick()
	{
		Destroy(gameObject);
	}
}

