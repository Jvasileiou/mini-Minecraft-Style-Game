using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CylinderStackScript : MonoBehaviour
{
    public static int numberOfCylinders = 0;
    Text cylinderStack;

    // Start is called before the first frame update
    void Start()
    {
        cylinderStack = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        cylinderStack.text = "CylinderStack : " + numberOfCylinders;
    }
}
