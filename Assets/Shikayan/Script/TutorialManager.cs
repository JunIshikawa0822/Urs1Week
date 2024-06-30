using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialImagesArray;

    private int tutorialIndex = 0;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetImage();
    }

    public void IndexMinus()
    {
        if (tutorialImagesArray.Length < 1) return;
        if (tutorialIndex < 1) return;
        tutorialIndex--;

        SetImage();
    }

    public void IndexPlus()
    {
        if (tutorialImagesArray.Length < 1) return;

        if (tutorialIndex >= tutorialImagesArray.Length - 1) return;
        tutorialIndex++;

        SetImage();
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void Reset()
    {
        tutorialIndex = 0;
    }

    private void SetImage()
    {
        Debug.Log(tutorialIndex);
        if (tutorialImagesArray.Length < 1) return;

        for(int i = 0; i < tutorialImagesArray.Length; i++)
        {
            if(i == tutorialIndex)
            {
                tutorialImagesArray[i].SetActive(true);
            }
            else
            {
                tutorialImagesArray[i].SetActive(false);
            }
        }
    }
}
