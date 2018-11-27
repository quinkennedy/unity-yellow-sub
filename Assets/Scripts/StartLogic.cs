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
    private bool _enableNetworking = false;

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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadScene("Content", LoadSceneMode.Additive);
        //TODO: maybe the align scene should only be loaded on the HoloLens
        SceneManager.LoadScene("Align", LoadSceneMode.Additive);
        if (platformModel.Occluded)
        {
            SceneManager.LoadScene("Environment", LoadSceneMode.Additive);
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Content"))
        {
            _enableNetworking = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //if we have a main camera, and no active network,
        //  then let's start up the network!
        // - we have to wait for a main camera so the avatar will get placed correctly.
        if (_enableNetworking && netMgrHUD != null && !netMgrHUD.manager.isNetworkActive && Camera.main != null)
        {
            //force re-read the config
            NetworkService.GetInstance().clearModel();
            NetworkModel networkModel = NetworkService.GetInstance().Model;

            if (networkModel.server)
            {
                Debug.Log("[StartLogic.Update] starting Host");
                netMgrHUD.manager.serverBindToIP = false;
                netMgrHUD.manager.StartHost();
            } else {
                netMgrHUD.manager.networkAddress = networkModel.IP;
                netMgrHUD.manager.StartClient();
            }
        }
    }
}
