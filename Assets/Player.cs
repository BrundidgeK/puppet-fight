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
    
    //Records the action taken previously by the player
    private string prevAction;


    void FixedUpdate()
    {
        FingerPositions();
    }

    public string FingerPositions()
    {
        //Recieves the data
        string action = actDic[uDPReceive.data];

        //Cancels player action if action and previous action are the same and both an attack move
        //Prevents attacks from running each frame
        if ((action.Contains("left hook")|| action.Contains("right hook") 
            || action.Contains("hammer")) && action == prevAction)
        {
            return "none";
        }

        // Assigns previous action the current action
        prevAction = action;

        return action;
    }

}
