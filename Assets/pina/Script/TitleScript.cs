using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TitleScript : MonoBehaviour
{
    public string nextScene;

    //タイトルの遊び心
    public GameObject[] blocks;
    public float destroyTime;
    public InputField inputField;
    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            OnMouseDown();
        }
    }
    private void OnMouseDown() {
        float randX = Random.Range(-10.0f,10.0f);
        float randY = Random.Range(5.0f,6.0f);
        int randBlockNumber = Random.Range(0, blocks.Length);
        GameObject block = Instantiate(this.blocks[randBlockNumber], new Vector3(randX, randY, 10), Quaternion.identity);
        Destroy(block,destroyTime);

    }

    public void GameStartButton(){
        string inputName = inputField.text;
        if(inputName == ""){
            //名前なんでもいいよん
            playerName = "CodeBlocker";
        }else{
            playerName = inputField.text;
        }

        Debug.Log(playerName);
    }
}
