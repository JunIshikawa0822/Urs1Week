using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image[] tutorialImagesArray;

    private int tutorialIndex = 0;

    private void Start()
    {
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
    }

    private void SetImage()
    {
        Debug.Log(tutorialIndex);
        if (tutorialImagesArray.Length < 1) return;

        for(int i = 0; i < tutorialImagesArray.Length; i++)
        {
            if(i == tutorialIndex)
            {
                tutorialImagesArray[i].enabled = true;
            }
            else
            {
                tutorialImagesArray[i].enabled = false;
            }
        }
    }
}
