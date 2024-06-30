using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSystem : SystemBase, IOnUpdate
{
    public float zoomSpeed = 1;
    private Camera mainCamera;
    private GameObject camera1;
    private GameObject camera2;
    float sensitiveMove = 0.8f;
    float sensitiveRotate = 5.0f;

    float xLimit = 4.0f;
    float yLimitHight = 10.0f;
    float yLimitLow = 4.0f;
    float zLimit = 6.0f;
    Vector3 currentPos;
    bool isCamera1or2;

    public override void SetUp()
    {

        camera1 = gameStat.camera1;
        camera2 = gameStat.camera2;

    }

    public void OnUpdate()
    {
        if (gameStat.isEnterRoom)
        {
            gameStat.isEnterRoom = false;
            SetCamera();
        }
        
        if (Input.GetMouseButton(0))
        {
            float moveX = Input.GetAxis("Mouse X") * sensitiveMove;
            float moveY = Input.GetAxis("Mouse Y") * sensitiveMove;
            if(isCamera1or2)
                mainCamera.transform.localPosition -= new Vector3(moveX, 0.0f, moveY);
            else
                mainCamera.transform.localPosition += new Vector3(moveX, 0.0f, moveY);

        }
        /*
        if (Input.GetMouseButton(1))
        {
            float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
            float rotateY = Input.GetAxis("Mouse Y") * sensitiveRotate;
            mainCamera.transform.Rotate(rotateY, rotateX, 0.0f);
        }
        */
        
        //var scroll = Input.mouseScrollDelta.y;
        //mainCamera.transform.position += -mainCamera.transform.forward * scroll * zoomSpeed;
        currentPos = mainCamera.transform.localPosition;
        
        if (isCamera1or2)
        {
            mainCamera.transform.localPosition =
            new Vector3(Mathf.Clamp(currentPos.x, -xLimit, xLimit), Mathf.Clamp(currentPos.y, yLimitLow, yLimitHight), Mathf.Clamp(currentPos.z, -zLimit, zLimit-3));
        }
        else
        {
            mainCamera.transform.localPosition =
            new Vector3(Mathf.Clamp(currentPos.x, -xLimit, xLimit ), Mathf.Clamp(currentPos.y, yLimitLow, yLimitHight), Mathf.Clamp(currentPos.z, -zLimit+1, zLimit));
        }
    
    }

    
    public void SetCamera()
    {
        //photonの接続より先に反応してしまうため！をつけてる。
        if (PhotonNetwork.IsMasterClient)
        {
            mainCamera = camera1.GetComponent<Camera>();
            camera1.SetActive(true);
            camera2.SetActive(false);
            isCamera1or2 = true; 
        }
        else
        {
            mainCamera = camera2.GetComponent<Camera>();
            camera1.SetActive(false);
            camera2.SetActive(true);
            isCamera1or2 = false;
        }
    }

}
