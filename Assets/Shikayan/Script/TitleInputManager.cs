using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    public void ActiveChange()
    {
        _object.SetActive(!_object.activeSelf);
    }
}
