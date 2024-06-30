using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnBase : MonoBehaviour
{
    [SerializeField]
    private float disappearTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnDisappear", 3);
    }

    // Update is called once per frame
    private void EnDisappear()
    {
        Destroy(this.gameObject);
    }
}
