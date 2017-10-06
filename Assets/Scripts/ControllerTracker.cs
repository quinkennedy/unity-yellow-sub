using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class ControllerTracker : MonoBehaviour {

    #region Inspector properties

    public UnityEngine.XR.XRNode node = UnityEngine.XR.XRNode.RightHand;

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = UnityEngine.XR.InputTracking.GetLocalPosition(node);
        transform.rotation = UnityEngine.XR.InputTracking.GetLocalRotation(node);

        if (node == UnityEngine.XR.XRNode.RightHand && Input.GetButtonDown("Fire1"))
        {
            UnityEngine.XR.InputTracking.Recenter();
        } else if (node == UnityEngine.XR.XRNode.LeftHand && Input.GetButtonDown("Fire2"))
        {
            UnityEngine.XR.InputTracking.Recenter();
        }
	}
}
