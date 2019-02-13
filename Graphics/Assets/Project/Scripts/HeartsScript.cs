using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour
{
    public static int heartsValue = 4;
    Text hearts;

    // Start is called before the first frame update
    void Start()
    {
        hearts = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hearts.text = "Hearts : " + heartsValue;
    }
}
