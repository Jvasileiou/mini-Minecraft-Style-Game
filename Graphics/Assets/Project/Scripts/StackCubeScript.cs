using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackCubeScript : MonoBehaviour
{
    public static int numberOfCubes = 0 ;
    Text cubeStack;

    // Start is called before the first frame update
    void Start()
    {
        cubeStack = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        cubeStack.text = "CubeStack : " + numberOfCubes;
    }
}
