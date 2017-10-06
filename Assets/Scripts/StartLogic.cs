using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using pizza.chill.yellowsub;

public class StartLogic : MonoBehaviour {

    #region Inspector properties

    public NetworkManagerHUD netMgrHUD = null;

    #endregion

    private bool _triedOnce = false;
    private bool _onVive = false;
    private const string ClassName = "StartLogic";
    private PlatformModel platformModel;

    // Use this for initialization
    void Start () {
        PlatformService platformService = PlatformService.GetInstance();
        platformModel = platformService.Model;
        List<GameObject> spawnable = netMgrHUD.manager.spawnPrefabs;
        bool foundPlayer = false;
        string name = System.Enum.GetName(
            typeof(PlatformModel.XRtypes),
            platformModel.XRType);
        for (int i = spawnable.Count - 1; i >= 0 && !foundPlayer; --i)
        {
            if (spawnable[i].name == name)
            {
                netMgrHUD.manager.playerPrefab = spawnable[i];
                foundPlayer = true;
            }
        }
        if (!foundPlayer)
        {
            Debug.LogErrorFormat("[{0}.Start] no prefab found for {1}",
                ClassName, name);
            netMgrHUD.manager.playerPrefab = spawnable[0];
        }
        SceneManager.LoadScene("Content", LoadSceneMode.Additive);
        //TODO: maybe the align scene should only be loaded on the HoloLens
        SceneManager.LoadScene("Align", LoadSceneMode.Additive);
        if (platformModel.Occluded)
        {
            SceneManager.LoadScene("Environment", LoadSceneMode.Additive);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if we have a main camera, and no active network,
        //  then let's start up the network!
        // - we have to wait for a main camera so the avatar will get placed correctly.
        if (netMgrHUD != null && !netMgrHUD.manager.isNetworkActive && Camera.main != null && !_triedOnce)
        {
            _triedOnce = true;
            Debug.Log("[StartLogic:Update] starting up network");
            switch (platformModel.XRType)
            {
                case PlatformModel.XRtypes.Hololens:
                default:
#if UNITY_EDITOR
                    netMgrHUD.manager.serverBindToIP = false;
                    netMgrHUD.manager.StartHost();
#else
                    netMgrHUD.manager.networkAddress = "192.168.1.7";
                    netMgrHUD.manager.StartClient();
#endif
                    break;
                //default:
                //    //otherwise standalone or in Editor

                //    //if we have already tried connecting to the server once, don't try again.
                //    //  the user can use the HUD to set the correct IP and attempt connecting again.
                //    netMgrHUD.manager.networkAddress = "192.168.1.19";
                //    netMgrHUD.manager.StartClient();
                //    break;
            }
        }
    }
}
