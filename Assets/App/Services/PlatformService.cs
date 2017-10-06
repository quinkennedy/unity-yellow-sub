using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace pizza.chill.yellowsub
{
    public class PlatformService
    {
        private static PlatformService _instance;
        private const string ClassName = "PlatformService";

        private PlatformModel _model;
        public PlatformModel Model
        {
            get
            {
                if (_model == null) {
                    InitModel();
                }
                return _model;
            }
        }

        private PlatformService() { }

        public static PlatformService GetInstance()
        {
            if (PlatformService._instance == null)
            {
                PlatformService._instance = new PlatformService();
            }
            return PlatformService._instance;
        }

        private void InitModel()
        {
            _model = new PlatformModel();
            _model.hasXR = XRDevice.isPresent;
            _model.usingXR = XRSettings.isDeviceActive;
            // you can use XRSettings.enabled to enable/disable using the headset
            _model.typeofXR = XRSettings.loadedDeviceName;
            _model.whichXR = XRDevice.model;
            Debug.LogFormat(
                "[{0}.Start] hasXR:{1} usingXR:{2} typeofXR:{3} whichXR:{4}",
                ClassName, 
                _model.hasXR, _model.usingXR, _model.typeofXR, _model.whichXR);
            switch (_model.typeofXR)
            {
                case "OpenVR":
                    switch (_model.whichXR)
                    {
                        case "Vive MV":
                            Debug.LogFormat("[{0}.InitModel] on Vive",
                                ClassName);
                            _model.XRType = PlatformModel.XRtypes.Vive;
                            break;
                        default:
                            Debug.LogErrorFormat(
                                "[{0}.Start] unhandled {1} model: \"{2}\"",
                                ClassName, _model.typeofXR, _model.whichXR);
                            break;
                    }
                    break;
                case "WindowsMR":
                    _model.XRType = PlatformModel.XRtypes.WindowsMR;
                    //whichXR can be:
                    // Acer AH100
                    //
                    break;
                default:
                    Debug.LogErrorFormat(
                        "[{0}.Start] unhandled device type: \"{1}\"",
                        ClassName, _model.typeofXR);
                    break;
            }
        }
    }
}
