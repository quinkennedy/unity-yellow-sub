using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NetworkService {
    private static NetworkService _instance;

    private string GetUserAccessibleDirectory()
    {
        return UnityEngine.Windows.Directory.localFolder;
    }

    private NetworkModel _model = null;

    public NetworkModel Model
    {
        get
        {
            if (_model == null)
            {
                LoadModel();
            }
            return _model;
        }
    }

    private NetworkService() { }

    public static NetworkService GetInstance()
    {
        if (NetworkService._instance == null)
        {
            NetworkService._instance = new NetworkService();
        }
        return NetworkService._instance;
    }

    private void LoadModel()
    {
        string filename = "network.json";
        string fullPath = Path.Combine(GetUserAccessibleDirectory(), filename);

        if (File.Exists(fullPath))
        {
            try
            {
                string json = File.ReadAllText(fullPath);
                _model = JsonUtility.FromJson<NetworkModel>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError(
                    "[NetworkService.LoadModel] couldn't load data: " + e);
                _model = new NetworkModel();
            }
        } else
        {
            _model = new NetworkModel();
        }
    }
    
    public void clearModel()
    {
        _model = null;
    }
}
