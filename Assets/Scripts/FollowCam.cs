using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FollowCam : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("[FollowCam:Start]");
		if (isLocalPlayer && Camera.main != null)
        {
            transform.SetParent(Camera.main.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
