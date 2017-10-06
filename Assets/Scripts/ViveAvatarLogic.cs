using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class ViveAvatarLogic : NetworkBehaviour {

    private Transform _head, _rightController, _leftController;
    //private Transform _rightControllerRef, _leftControllerRef;
    private GameObject target1, target2;

    // Use this for initialization
    void Start () {
        if (isLocalPlayer) {
            Debug.Log("[ViveAvatarLogic:Start]");
            _head = transform.Find("head");
            _rightController = transform.Find("right hand");
            _leftController = transform.Find("left hand");

            //transform.SetParent(Camera.main.transform.parent, false);
        }

        //_rightControllerRef = new GameObject().transform;
        //_rightControllerRef.parent = Camera.main.transform.parent;

        //_leftControllerRef = new GameObject().transform;
        //_leftControllerRef.parent = Camera.main.transform.parent;
    }

    private void setTargets()
    {
        target1.transform.position = new Vector3(-1, 0, 0);
        target2.transform.position = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer)
        {
            if (Camera.main != null && !Camera.main.transform.parent.Equals(transform.parent))
            {
                Debug.Log("[ViveAvatarLogic:Update] setting parent/adding targets");
                transform.SetParent(Camera.main.transform.parent, false);

                target1 = new GameObject("Target1");
                target1.transform.SetParent(Camera.main.transform.parent);

                target2 = new GameObject("Target2");
                target2.transform.SetParent(Camera.main.transform.parent);

                setTargets();
            }

            _rightController.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
            _rightController.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand);

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("[ViveAvatarLogic:Update] Fire1 pressed");
                
                Vector3 translate = (target1.transform.localPosition - _rightController.localPosition);
                Debug.Log("[ViveAvatarLogic:Update] to " + translate);
                //adjust position
                Camera.main.transform.parent.localPosition = translate;

                setTargets();

                //Camera.main.transform.parent.position = new Vector3(-1, 0, 0) - _rightController.position;
            }

            //_rightControllerRef.localPosition = InputTracking.GetLocalPosition(VRNode.RightHand);
            //_rightControllerRef.localRotation = InputTracking.GetLocalRotation(VRNode.RightHand);

            //_rightController.position = _rightControllerRef.position;
            //_rightController.rotation = _rightControllerRef.rotation;
            
            _leftController.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftHand);
            _leftController.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.LeftHand);

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("[ViveAvatarLogic:Update] Fire2 pressed");
                //adjust rotation
                float destAngle = getAngleFromPos(target1.transform.localPosition, target2.transform.localPosition);
                float currAngle = getAngleFromPos(target1.transform.localPosition, _leftController.localPosition);
                //float destAngle = getAngleFromPos( new Vector2(-1, 0), new Vector2(1, 0));
                //float currAngle = getAngleFromPos(new Vector2(_rightControllerRef.position.x, _rightControllerRef.position.z),
                //                                  new Vector2(_leftControllerRef.position.x, _leftControllerRef.position.z));

                Debug.Log("from" + currAngle);
                Debug.Log("to" + destAngle);
                Camera.main.transform.parent.RotateAround(target1.transform.position, new Vector3(0, 1, 0), Mathf.Rad2Deg * (currAngle - destAngle));
                //Debug.Log("result" + getAngleFromPos(transform.Find("Cylinder").localPosition, target2.transform.localPosition));
                //Camera.main.transform.parent.RotateAround(_rightControllerRef.position, new Vector3(0, 1, 0), -Mathf.Rad2Deg * (currAngle - destAngle));
                //_leftControllerOffset = -InputTracking.GetLocalPosition(VRNode.LeftHand);

                setTargets();

                ////adjust rotation
                //float destAngle = getAngleFromPos(new Vector2(-1, 0), new Vector2(1, 0));
                //float currAngle = getAngleFromPos(new Vector2(_rightControllerRef.position.x, _rightControllerRef.position.z),
                //                                  new Vector2(_leftControllerRef.position.x, _leftControllerRef.position.z));
                //Camera.main.transform.parent.RotateAround(_rightControllerRef.position, new Vector3(0, 1, 0), -Mathf.Rad2Deg * (currAngle - destAngle));
                ////_leftControllerOffset = -InputTracking.GetLocalPosition(VRNode.LeftHand);
            }

            //_leftControllerRef.localPosition = InputTracking.GetLocalPosition(VRNode.LeftHand);
            //_leftControllerRef.localRotation = InputTracking.GetLocalRotation(VRNode.LeftHand);

            //_leftController.position = _leftControllerRef.position;
            //_leftController.rotation = _leftControllerRef.rotation;


            if (Camera.main != null)
            {
                _head.position = Camera.main.transform.position;
                _head.rotation = Camera.main.transform.rotation;
            }
        }
    }

    private float getAngleFromPos(Vector2 origin, Vector2 pos)
    {
        Vector2 point = pos - origin;
        return Mathf.Atan2(point.y, point.x);
    }

    /// <summary>
    /// Extract translation from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Translation offset.
    /// </returns>
    public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 translate;
        translate.x = matrix.m03;
        translate.y = matrix.m13;
        translate.z = matrix.m23;
        return translate;
    }

    /// <summary>
    /// Extract rotation quaternion from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Quaternion representation of rotation transform.
    /// </returns>
    public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    /// <summary>
    /// Extract scale from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Scale vector.
    /// </returns>
    public static Vector3 ExtractScaleFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }

    /// <summary>
    /// Extract position, rotation and scale from TRS matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <param name="localPosition">Output position.</param>
    /// <param name="localRotation">Output rotation.</param>
    /// <param name="localScale">Output scale.</param>
    public static void DecomposeMatrix(ref Matrix4x4 matrix, out Vector3 localPosition, out Quaternion localRotation, out Vector3 localScale)
    {
        localPosition = ExtractTranslationFromMatrix(ref matrix);
        localRotation = ExtractRotationFromMatrix(ref matrix);
        localScale = ExtractScaleFromMatrix(ref matrix);
    }

    /// <summary>
    /// Set transform component from TRS matrix.
    /// </summary>
    /// <param name="transform">Transform component.</param>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    public static void SetTransformFromMatrix(Transform transform, ref Matrix4x4 matrix)
    {
        transform.localPosition = ExtractTranslationFromMatrix(ref matrix);
        transform.localRotation = ExtractRotationFromMatrix(ref matrix);
        transform.localScale = ExtractScaleFromMatrix(ref matrix);
    }


    // EXTRAS!

    /// <summary>
    /// Identity quaternion.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Quaternion.identity</c>.</para>
    /// </remarks>
    public static readonly Quaternion IdentityQuaternion = Quaternion.identity;
    /// <summary>
    /// Identity matrix.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Matrix4x4.identity</c>.</para>
    /// </remarks>
    public static readonly Matrix4x4 IdentityMatrix = Matrix4x4.identity;

    /// <summary>
    /// Get translation matrix.
    /// </summary>
    /// <param name="offset">Translation offset.</param>
    /// <returns>
    /// The translation transform matrix.
    /// </returns>
    public static Matrix4x4 TranslationMatrix(Vector3 offset)
    {
        Matrix4x4 matrix = IdentityMatrix;
        matrix.m03 = offset.x;
        matrix.m13 = offset.y;
        matrix.m23 = offset.z;
        return matrix;
    }

}
