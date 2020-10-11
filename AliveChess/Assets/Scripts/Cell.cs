using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{

    public Action<Vector2> ClickEvent;
    
    private void OnEnable()
    {
        ClickEvent = vector2 => {};
    }

    private void OnMouseDown()
    {
        ClickEvent.Invoke(transform.position);
    }
}
