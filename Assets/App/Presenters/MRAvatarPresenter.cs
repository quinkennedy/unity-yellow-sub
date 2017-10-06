using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.WSA.Input;

namespace pizza.chill.yellowsub
{
    public class MRAvatarPresenter : NetworkBehaviour
    {

        public Transform HMD, RightController, LeftController, RightCalibrationTransform, LeftCalibrationTransform;
        private Transform cam;
        private bool down = false;

        private const string ClassName = "MRAvatarPresenter";

        // Use this for initialization
        void Start()
        {
            if (isLocalPlayer)
            {
                cam = Camera.main.transform;
                transform.SetParent(cam.parent, false);
                InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
                InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
                InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
            }
        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
        {
            Debug.LogFormat("[{0}.SourceLost] {1}:{2} @ {3}s",
                ClassName, 
                args.state.source.kind, 
                args.state.source.handedness,
                Time.realtimeSinceStartup);
        }

        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
        {
            Debug.LogFormat("[{0}.SourceDetected] {1}:{2} @ {3}s",
                ClassName, 
                args.state.source.kind, 
                args.state.source.handedness,
                Time.realtimeSinceStartup);
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {
            if (args.state.source.kind == InteractionSourceKind.Controller)
            {
                Vector3 position;
                Quaternion rotation;
                if (args.state.sourcePose.TryGetPosition(out position, InteractionSourceNode.Pointer)
                    && args.state.sourcePose.TryGetRotation(out rotation, InteractionSourceNode.Pointer))
                {
                    Transform target = null;
                    Transform calibrateTo = null;
                    switch (args.state.source.handedness)
                    {
                        case InteractionSourceHandedness.Left:
                            target = LeftController;
                            calibrateTo = LeftCalibrationTransform;
                            break;
                        case InteractionSourceHandedness.Right:
                            target = RightController;
                            calibrateTo = RightCalibrationTransform;
                            break;
                    }
                    if (target != null)
                    {
                        target.localPosition = position;
                        target.localRotation = rotation;
                        if (args.state.menuPressed && !down)
                        {
                            CalibrationService.Calibrate(transform.parent, calibrateTo, CalibrationPoint.Instance);
                        }
                        down = args.state.menuPressed;
                    }
                }
            }
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                HMD.localPosition = cam.localPosition;
                HMD.localRotation = cam.localRotation;
            }
        }
    }
}
