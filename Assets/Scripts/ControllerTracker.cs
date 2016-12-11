using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class ControllerTracker : MonoBehaviour {

    #region Inspector properties

    public VRNode node = VRNode.RightHand;

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = InputTracking.GetLocalPosition(node);
        transform.rotation = InputTracking.GetLocalRotation(node);

        if (node == VRNode.RightHand && Input.GetButtonDown("Fire1"))
        {
            InputTracking.Recenter();
        } else if (node == VRNode.LeftHand && Input.GetButtonDown("Fire2"))
        {
            InputTracking.Recenter();
        }
	}
}
