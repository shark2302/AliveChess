using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{

    public Action<Vector2> ClickEvent;

    public int SlowEffect;
    private void OnEnable()
    {
        ClickEvent = vector2 => {};
    }

    private void OnMouseDown()
    {
        ClickEvent.Invoke(transform.position);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var king = other.GetComponent<Player>();
        if (king != null)
        {
            king.SlowEffect = SlowEffect;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var king = other.collider.GetComponent<Player>();
        if (king != null)
        {
            king.SlowEffect = SlowEffect;
            
        }
    }
}
