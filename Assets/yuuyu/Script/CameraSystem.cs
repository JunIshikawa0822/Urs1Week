using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSystem : SystemBase, IOnUpdate
{
    public float zoomSpeed = 1;
    private Camera mainCamera;
    float sensitiveMove = 0.8f;
    float sensitiveRotate = 5.0f;

    float xLimit = 3.0f;
    float yLimitHight = 6.0f;
    float yLimitLow = 2.0f;
    float zLimit = 3.0f;

    private GameStatus gameStatus;

    public override void SetUp()
    {
        gameStatus = new GameStatus();
        SetCamera();


    }

    public void OnUpdate()
    {
        /*
        if (GameStatus.isCameraSet)
        {
            GameStatus.isCameraSet = false;
        }
        */


        Vector3 currentPos;
        if (Input.GetMouseButton(0))
        {
            float moveX = Input.GetAxis("Mouse X") * sensitiveMove;
            float moveY = Input.GetAxis("Mouse Y") * sensitiveMove;
            mainCamera.transform.localPosition -= new Vector3(moveX, 0.0f, moveY);
           
        }
        if (Input.GetMouseButton(1))
        {
            float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
            float rotateY = Input.GetAxis("Mouse Y") * sensitiveRotate;
            mainCamera.transform.Rotate(rotateY, rotateX, 0.0f);
        }
       
        var scroll = Input.mouseScrollDelta.y;
        mainCamera.transform.position += -mainCamera.transform.forward * scroll * zoomSpeed;
        currentPos = mainCamera.transform.localPosition;
        mainCamera.transform.localPosition =
            new Vector3(Mathf.Clamp(currentPos.x, -xLimit, xLimit), Mathf.Clamp(currentPos.y, yLimitLow, yLimitHight), Mathf.Clamp(currentPos.z, -zLimit, zLimit));

    }

    public void SetCamera()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            mainCamera=GameObject.Find("MainCamera1").GetComponent<Camera>();
            Debug.Log(mainCamera.gameObject.name);
        }
        else
        {
            mainCamera = GameObject.Find("MainCamera2").GetComponent<Camera>();
            Debug.Log(mainCamera.gameObject.name);
        }
    }

}
