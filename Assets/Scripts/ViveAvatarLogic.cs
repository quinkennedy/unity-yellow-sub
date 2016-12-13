using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class ViveAvatarLogic : NetworkBehaviour {

    private Transform _head, _rightController, _leftController;

	// Use this for initialization
	void Start () {
        _head = transform.Find("head");
        _rightController = transform.Find("right hand");
        _leftController = transform.Find("left hand");
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            if (Camera.main != null)
            {
                _head.position = Camera.main.transform.position;
            }
            _rightController.position = InputTracking.GetLocalPosition(VRNode.RightHand);
            _rightController.rotation = InputTracking.GetLocalRotation(VRNode.RightHand);
            _leftController.position = InputTracking.GetLocalPosition(VRNode.LeftHand);
            _leftController.rotation = InputTracking.GetLocalRotation(VRNode.LeftHand);
        }
    }
}
