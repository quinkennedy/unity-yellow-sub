using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pizza.chill.yellowsub
{
    public class PlatformModel
    {
        public bool hasXR;
        public bool usingXR;
        public string typeofXR;
        public string whichXR;
        public XRtypes XRType;
        public bool Occluded
        {
            get
            {
                switch (XRType)
                {
                    case XRtypes.Hololens:
                        return false;
                    default:
                        return true;
                }
            }
        }
        public bool CanPassCamera
        {
            get
            {
                switch (XRType)
                {
                    case XRtypes.ARCore:
                    case XRtypes.ARKit:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public int DOF
        {
            get
            {
                switch (XRType)
                {
                    case XRtypes.Desktop:
                        return 0;
                    case XRtypes.Cardboard:
                        return 3;
                    default:
                        return 6;
                }
            }
        }

        public enum XRtypes
        {
            Vive,
            WindowsMR,
            Hololens,
            ARKit,
            ARCore,
            Desktop,
            Oculus,
            Cardboard,
            GearVR
        }
    }
}
