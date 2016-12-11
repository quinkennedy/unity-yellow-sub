using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class StartLogic : MonoBehaviour {

    #region Inspector properties

    public NetworkManagerHUD netMgrHUD = null;
    public GameObject vivePrefab = null;
    public GameObject hololensPrefab = null;
    public GameObject screenPrefab = null;

    #endregion

    private bool _triedOnce = false;
    private bool _onVive = false;

    // Use this for initialization
    void Start () {
#if UNITY_WSA_10_0
        //If on Hololens
        SceneManager.LoadScene("HololensCamera", LoadSceneMode.Additive);
        if (hololensPrefab != null){
            netMgrHUD.manager.playerPrefab = hololensPrefab;
        }
#else
        //_onVive = (VRSettings.loadedDeviceName.Length != 0);
        _onVive = ((VRSettings.loadedDeviceName == "OpenVR") && (VRDevice.model == "Vive MV"));
        Debug.Log("[Start] loadedDeviceName: '" + VRSettings.loadedDeviceName + "'");
        Debug.Log("[Start] isPresent : " + VRDevice.isPresent);
        Debug.Log("[Start] model : '" + VRDevice.model + "'");
        Debug.Log("[Start] active : " + VRSettings.isDeviceActive);

        if (_onVive)
        {
            SceneManager.LoadScene("ViveCam", LoadSceneMode.Additive);
            if (vivePrefab != null)
            {
                netMgrHUD.manager.playerPrefab = vivePrefab;
            }
        } else
        {
            SceneManager.LoadScene("ScreenCam", LoadSceneMode.Additive);
            if (screenPrefab != null)
            {
                netMgrHUD.manager.playerPrefab = screenPrefab;
            }
        }

        if (netMgrHUD != null)
        {
            netMgrHUD.enabled = true;
        }
#endif
        SceneManager.LoadScene("Content", LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if we have a main camera, and no active network,
        //  then let's start up the network!
        // - we have to wait for a main camera so the avatar will get placed correctly.
        if (netMgrHUD != null && !netMgrHUD.manager.isNetworkActive && Camera.main != null)
        {
            Debug.Log("starting up network");
#if UNITY_WSA_10_0
            //aka if HoloLens
            netMgrHUD.manager.serverBindToIP = false;
            netMgrHUD.manager.StartHost();
#else
            //otherwise standalone or in Editor

            //if we have already tried connecting to the server once, don't try again.
            //  the user can use the HUD to set the correct IP and attempt connecting again.
            if (!_triedOnce)
            {
                _triedOnce = true;
                netMgrHUD.manager.networkAddress = "192.168.86.163";
                netMgrHUD.manager.StartClient();
            }
#endif
        }
    }
}
