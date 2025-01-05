using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Object to recieve data from python socket
    public UDPReceive uDPReceive;

    public BattleManager battle;

    //Dictionary with all possible player interactions
    public Dictionary<string, string> actDic = new Dictionary<string, string>
    {
        { "FFFFF", "idle" },
        { "FFFFT", "right hook" },
        { "FFFTF", "lean right" },
        { "FFTTF", "lean right" },
        { "FFFTT", "lean right,right hook" },
        { "FFTFF", "head down" },
        { "FFTFT", "right hook" },
        { "FTFFF", "lean left" },
        { "FTFFT", "lean left,right hook" },
        { "FTFTF", "ducking" },
        { "FTFTT", "ducking,right hook" },
        { "FTTFF", "lean left" },
        { "FTTFT", "lean left,right hook" },
        { "FTTTF", "ducking" },
        { "FTTTT", "ducking,right hook" },
        { "TFFFF", "left hook" },
        { "TFFFT", "hammer" },
        { "TFFTF", "lean right,left hook" },
        { "TFFTT", "lean right,hammer" },
        { "TFTFF", "left hook" },
        { "TFTFT", "block" },
        { "TFTTF", "lean right,left hook" }, 
        { "TFTTT", "lean right,block" },
        { "TTFFF", "lean left,left hook" },
        { "TTFFT", "lean left,hammer" },
        { "TTFTF", "ducking,right hook" },
        { "TTFTT", "ducking,hammer" },
        { "TTTFF", "left hook,lean left" },
        { "TTTFT", "lean left,block" },
        { "TTTTF", "ducking,left hook" },
        { "TTTTT", "ducking,block" },
        { "FFTTT", "lean right, right hook" }
    };


    public GameObject warning;


    void Update()
    {
        //FingerPositions();
    }

    public string FingerPositions()
    {
        string action = "FFFFF";
        //Receives the data
        try
        {
            action = actDic[uDPReceive.data];
            warning.SetActive(false);
        }
        catch {
            warning.SetActive(true);
        }

        return action;
    }

}
