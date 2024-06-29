using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneChangerScript : MonoBehaviour
{
    public void SceneChange(string _nextScene){
        SceneManager.LoadScene(_nextScene);
    }
}
