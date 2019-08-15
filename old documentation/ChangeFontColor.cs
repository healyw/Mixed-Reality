using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeFontColor : MonoBehaviour
{
    TextMesh outputData;
    Color robotoBlack;
    Color robotoGrey;
    Color robotoRed;

    void Start()
    {
        outputData = GetComponent<TextMesh>();
        robotoBlack = new Color(256, 256, 256);
        robotoGrey = new Color(101, 101, 101);
        robotoRed = new Color(230, 50, 69);

    }
    void Update()
    {
        ChangeColor(outputData, robotoBlack, robotoGrey, robotoRed, JSONUpdater603.sensorValue, 500.0, 100.0);    
    }

    public void ChangeColor(TextMesh proUGUI, Color normal, Color inactive, Color error, double value, double inactiveValue, double errorValue)
    {
        if(value < errorValue)
        {
            proUGUI.color = error;
        }
        else if (value >= inactiveValue)
        {
            proUGUI.color = inactive;
        }
        else
        {
            proUGUI.color = normal;
        }
    }
}
