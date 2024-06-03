using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public UDPReceive uDPReceive;
    private string previousData;

    public TMP_Text instruction, winText;

    void Start()
    {
        if(PlayerPrefs.GetInt("Win State") != 0)
        {
            winText.text = PlayerPrefs.GetInt("Win State") == 1 ? "You Won!" : "You Lost!";
        }
        else
        {
            winText.gameObject.SetActive(false);
        }
        Invoke("UpdateText", .1f);
    }

    void UpdateText()
    {
        string data = uDPReceive.data;
        if (previousData == data)
        {
            instruction.text = "Right hand needs to be in camera frame.";
        }
        else
        {
            instruction.text = "Ready to Start!";
        }
        previousData = data;
        Invoke("UpdateText", .1f);
    }
}
