using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribs : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private int playerInd;
    public int rotateCardCount = 3;
    public int pushCardCount = 2;
    public int swapCardCount = 1;
    
    public int X
    {
        get => x;
        set => x = value;
    }
    
    public int Y
    {
        get => y;
        set => y = value;
    }
    
    public int PlayerInd
    {
        set => playerInd = value;
    }
}
