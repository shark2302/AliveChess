using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAgreementPopup : MonoBehaviour
{

	public GameProxy GameProxy;
	public Text InfoText;
	public GameObject ChessWindow;
    private string _invitorId;

    public void SetData(string invitorId)
    {
	    _invitorId = invitorId;
	    InfoText.text = "Вы получили приглашение от " + _invitorId;
    }

    public void OnDeclineButton()
    {
	    GameProxy.GameManager.SendAnswerToInvitor(_invitorId, false);
	    Destroy(gameObject);
    }

    public void OnAgreeButton()
    {
	    GameProxy.GameManager.SendAnswerToInvitor(_invitorId, true);
	    var win = Instantiate(ChessWindow, GameProxy.GameManager.Canvas.transform);
	    Destroy(gameObject);
    }
}
