using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonBase : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    public void ActiveChange()
    {
        _object.SetActive(!_object.activeSelf);
        Debug.Log("おせてる"); 
    }
}
