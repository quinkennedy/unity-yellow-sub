using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.WSA.Input;

namespace pizza.chill.yellowsub
{
    public class MRAvatarPresenter : NetworkBehaviour
    {

        public Transform HMD;
        public ControllerPresenter RightController, LeftController;
        public GameObject bulletPrefab;

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
                            target = LeftController.transform;
                            calibrateTo = LeftController.CalibrationPoint;
                            break;
                        case InteractionSourceHandedness.Right:
                            target = RightController.transform;
                            calibrateTo = RightController.CalibrationPoint;
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

        [Command]
        void CmdFire(Vector3 position, Quaternion rotation)
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                position,
                rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 5;

            // Spawn the bullet on the Clients
            NetworkServer.Spawn(bullet);

            // Destroy the bullet after 10 seconds
            Destroy(bullet, 10.0f);
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                HMD.localPosition = cam.localPosition;
                HMD.localRotation = cam.localRotation;
                if (Input.GetButtonDown("FireRight"))
                {
                    CmdFire(
                        RightController.BulletSpawn.position,
                        RightController.BulletSpawn.rotation);
                }
                if (Input.GetButtonDown("FireLeft"))
                {
                    CmdFire(
                        LeftController.BulletSpawn.position,
                        LeftController.BulletSpawn.rotation);
                }
            }
        }
    }
}
