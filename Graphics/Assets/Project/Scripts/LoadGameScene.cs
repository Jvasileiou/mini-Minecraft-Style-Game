using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(loadScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadScene()
    {
        int n = Int32.Parse(inputField.text) ;
        if(n > 1 )
        {
            GameBrain.N = n;
            Debug.Log("..... --- > " + GameBrain.N);
            SceneManager.LoadScene("GameScene" , LoadSceneMode.Single); 
        }
        else
        {
            Debug.Log(" Enter a number > 1 again :");
        }
    }

}
