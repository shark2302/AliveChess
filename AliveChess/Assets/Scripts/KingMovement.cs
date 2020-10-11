using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
    
    private Cell[] _cells;

    private Vector2 _target;

    private bool _isMoving;
    
    public void OnClicked(Vector2 pos)
    {
        if (!_isMoving)
        {
            _isMoving = true;
            _target = pos;
        }
    }
    
    void Update()
    {
        if (_target != Vector2.zero)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, _target, Time.deltaTime * 5);
        }
        
        if (transform.position.Equals(_target))
        {
            _target = Vector2.zero;
            _isMoving = false;
        }
        
    }
}
