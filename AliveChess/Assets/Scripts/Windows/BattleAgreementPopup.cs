using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAgreementPopup : MonoBehaviour
{
	public Text InfoText;
    private string _invitorId;

    public void SetData(string invitorId)
    {
	    _invitorId = invitorId;
	    InfoText.text = "Вы получили приглашение от " + _invitorId;
    }
}
