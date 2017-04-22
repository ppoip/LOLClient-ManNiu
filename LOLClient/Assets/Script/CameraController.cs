using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }


    public void LookAtTarget(Transform target,Vector3 offset)
    {
        this.transform.position = target.position + offset;
    }


}
