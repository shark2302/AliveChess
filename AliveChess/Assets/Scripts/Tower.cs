using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class Tower : MonoBehaviour, IPunObservable
{

	public GameProxy GameProxy;
	
	public SpriteRenderer SpriteRenderer;

	public GameObject TowerInfoPopup;

	public PhotonView photonView;

	private string _ownerID;
	private string _oldID;
	
	public void Update()
	{
		if (!_ownerID.IsNullOrEmpty())
		{
			SpriteRenderer.color = Color.red;
			_oldID = _ownerID;
		}
	}

	private void OnMouseDown()
	{
		if (_ownerID != PhotonNetwork.LocalPlayer.UserId) 
		{
			Debug.Log(_ownerID);
			var win = Instantiate(TowerInfoPopup, GameProxy.GameManager.Canvas.transform);
			win.GetComponent<TowerInfoPopup>().SetData(_ownerID, PhotonNetwork.LocalPlayer.UserId);
		}
	}

	public void SetOwnerID(string ownerID)
	{
		_ownerID = ownerID;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(_ownerID);
		}
		else
		{
			_ownerID = (string) stream.ReceiveNext();
		}
	}
}
