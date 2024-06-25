using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gene :MonoBehaviour
{
    [ContextMenu("生成")]
    public void GeneNetObj()
    {
        var position = new Vector3(1.0f, 1.0f, 1.0f);
        PhotonNetwork.Instantiate("TestCube", position, Quaternion.identity);
    }

}
