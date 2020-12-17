using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	private GameObject _followObject;

    // Update is called once per frame
    void Update()
    {
	    if (_followObject != null)
	    {
		    transform.position = new Vector3(_followObject.transform.position.x, _followObject.transform.position.y, -10);
	    }
    }

    public void SetFollowObject(GameObject follow)
    {
	    _followObject = follow;
    }
}
