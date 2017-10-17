using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkModel {
    public string IP;
    public bool server;

    public NetworkModel()
    {
        IP = "0.0.0.0";
        server = true;
    }
}
