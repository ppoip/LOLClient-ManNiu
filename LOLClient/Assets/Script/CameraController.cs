using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    public GameObject Parent
    {
        get
        {
            return transform.parent.gameObject;
        }
    }
    public float moveSpeed = 30f;



    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }


    public void LookAtTarget(Transform target,Vector3 offset)
    {
        Parent.transform.position = target.position + offset;
    }

    public Vector3 GetCameraPos()
    {
        return _camera.gameObject.transform.position;
    }

    public Vector3 GetParentPos()
    {
        return Parent.transform.position;
    }

    public void CameraTranslate(MoveDirection direction)
    {
        Vector3 movement = Vector3.zero;
        switch (direction)
        {
            case MoveDirection.Left:
                movement = new Vector3(0, 0, 1);
                break;
            case MoveDirection.Right:
                movement = new Vector3(0, 0, -1);
                break;
            case MoveDirection.Up:
                movement = new Vector3(1, 0, 0);
                break;
            case MoveDirection.Down:
                movement = new Vector3(-1, 0, 0);
                break;
            default:
                break;
        }
        Parent.transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    public enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down
    }

}
