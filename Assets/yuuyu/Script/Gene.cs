using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("生成")]
    public void GeneNetObj()
    {
        var position = new Vector3(1.0f, 1.0f, 1.0f);
        PhotonNetwork.Instantiate("TestCube", position, Quaternion.identity);
    }

}
