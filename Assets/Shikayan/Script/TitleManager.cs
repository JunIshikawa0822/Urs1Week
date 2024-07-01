using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private bool isKeyInput;
    private AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

        if (IsInputAnyKey())
        {
            audioSource.PlayOneShot(audioSource.clip);

            SceneChange(nextScene);
        }
    }

    private bool IsInputAnyKey()
    {

        if (Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private void SceneChange(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene);
    }
}
