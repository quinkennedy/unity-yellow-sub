using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pizza.chill.yellowsub
{
    public abstract class CalibrationService
    {
        private const string ClassName = "CalibrationService";

        public static void Calibrate(Transform wrapper, Transform LocalCalibrationPoint, Transform GlobalCalibrationPoint)
        {
            Debug.LogFormat("[{0}.Calibrate] from p:{1} r:{2} to p:{3} r:{4} start p:{5} r:{6}",
                ClassName, 
                LocalCalibrationPoint.position, 
                LocalCalibrationPoint.rotation.eulerAngles, 
                GlobalCalibrationPoint.position, 
                GlobalCalibrationPoint.rotation.eulerAngles,
                wrapper.position,
                wrapper.rotation.eulerAngles);
            
            Debug.LogFormat("[{0}.Calibrate] localFwd:{1} globalFwd:{2}",
                ClassName,
                LocalCalibrationPoint.rotation * Vector3.forward,
                GlobalCalibrationPoint.rotation * Vector3.forward);
            Quaternion rotation =
                Quaternion.FromToRotation(
                    LocalCalibrationPoint.rotation * Vector3.forward,
                    GlobalCalibrationPoint.rotation * Vector3.forward);
            /*LocalCalibrationPoint.rotation
            * Quaternion.Inverse(GlobalCalibrationPoint.rotation);
            /*GlobalCalibrationPoint.rotation 
            * Quaternion.Inverse(LocalCalibrationPoint.rotation);
        /* Quaternion.RotateTowards(
            LocalCalibrationPoint.rotation,
            GlobalCalibrationPoint.rotation,
            float.PositiveInfinity);*/
            //float rotation = GlobalCalibrationPoint.rotation.eulerAngles.y
            //    - LocalCalibrationPoint.rotation.eulerAngles.y;
            Vector3 direction = rotation * Vector3.forward;
            direction.y = 0;
            rotation = Quaternion.FromToRotation(Vector3.forward, direction);
            wrapper.Rotate(0, rotation.eulerAngles.y, 0);// rotation.eulerAngles);
            Vector3 offset = 
                wrapper.InverseTransformPoint(GlobalCalibrationPoint.position)
                - wrapper.InverseTransformPoint(LocalCalibrationPoint.position);
            wrapper.Translate(offset.x, 0, offset.z);

            Debug.LogFormat("[{0}.Calibrate] result d:{3} p:{1} r:{2}",
                ClassName,
                wrapper.position,
                wrapper.rotation.eulerAngles,
                rotation.eulerAngles);
        }
    }
}
