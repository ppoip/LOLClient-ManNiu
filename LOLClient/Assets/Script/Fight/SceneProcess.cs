using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SceneProcess : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    private FightScene fightScene;

    [SerializeField]
    private CameraController mainCamera;

    public float minCameraPosX;
    public float maxCameraPosX;
    public float minCameraPosY;
    public float maxCameraPosY;


    private void Update()
    {
        var mousePos = Input.mousePosition;

        if (mousePos.x < 10)
        {
            //相机左移
            mainCamera.CameraTranslate(CameraController.MoveDirection.Left);
        }
        else if(mousePos.x > Screen.width - 10)
        {
            //相机右移
            mainCamera.CameraTranslate(CameraController.MoveDirection.Right);
        }

        if(mousePos.y < 10)
        {
            //相机下移
            mainCamera.CameraTranslate(CameraController.MoveDirection.Down);
        }
        else if(mousePos.y > Screen.height - 10)
        {
            //相机上移
            mainCamera.CameraTranslate(CameraController.MoveDirection.Up);
        }

        //Debug.Log(mousePos.x + " : " + mousePos.y);


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId == PointerInputModule.kMouseLeftId)
        {
            //鼠标左键点下
        } else if (eventData.pointerId == PointerInputModule.kMouseRightId) 
        {
            //鼠标右键点下
            fightScene.OnMapGroundRightClick(eventData.position);
        }
    }
}
