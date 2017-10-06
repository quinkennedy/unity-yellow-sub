using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationPoint : MonoBehaviour {

    public static Transform Instance;

    private void Awake()
    {
        Instance = transform;
        Debug.LogFormat("[CalibrationPoint.Awake] rotation:{0}",
            Instance.rotation.eulerAngles);
    }
}
