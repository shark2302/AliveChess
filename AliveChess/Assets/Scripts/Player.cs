using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable
{

    public Action<int> TowerHoldedEvent;

    public int Speed;
    public float MaxDistanceForMove;

    public float MaxDistancetoHold;
        
    private Vector2 _target;
    private bool _isMoving;

    private List<Tower> _holdedTowers;

    public int SlowEffect { get; set; }

    private void OnEnable()
    {
        _holdedTowers = new List<Tower>();
        TowerHoldedEvent = i => { };
    }

    public void OnCellClicked(Vector2 pos)
    {
        if (!_isMoving && Vector2.Distance(transform.position, pos) <= MaxDistanceForMove)
        {
            _isMoving = true;
            _target = pos;
        }
        else
        {
            Debug.Log("Нельзя так далеко ходить!!!");
        }
    }
    
    void Update()
    {
        if (_target != Vector2.zero)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, _target, Time.deltaTime * (Speed - SlowEffect));
        }
        
        if (transform.position.Equals(_target))
        {
            _target = Vector2.zero;
            _isMoving = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3) stream.ReceiveNext();
        }
    }
}
