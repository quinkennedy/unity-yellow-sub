using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class ViveAvatarLogic : NetworkBehaviour {

    private Transform _head, _rightController, _leftController;
    private Vector3 _rightControllerOffset = Vector3.zero;
    private Vector3 _leftControllerOffset = Vector3.zero;

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

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("[ViveAvatarLogic:Update] Fire1 pressed");
                _rightControllerOffset = -InputTracking.GetLocalPosition(VRNode.RightHand);
            }
            _rightController.position = InputTracking.GetLocalPosition(VRNode.RightHand) + _rightControllerOffset;
            _rightController.rotation = InputTracking.GetLocalRotation(VRNode.RightHand);

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("[ViveAvatarLogic:Update] Fire2 pressed");
                _leftControllerOffset = -InputTracking.GetLocalPosition(VRNode.LeftHand);
            }
            _leftController.position = InputTracking.GetLocalPosition(VRNode.LeftHand) + _leftControllerOffset;
            _leftController.rotation = InputTracking.GetLocalRotation(VRNode.LeftHand);
        }
    }
}
