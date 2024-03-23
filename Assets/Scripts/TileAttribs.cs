using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileAttribs : MonoBehaviour
{
    [SerializeField] private bool isRedChamber;
    [SerializeField] private bool isPurpleChamber;
    [SerializeField] private bool isGreenChamber;
    [SerializeField] private bool isBlueChamber;
    
    public bool IsRedChamber
    {
        get { return isRedChamber; }
    }
    
    public bool IsPurpleChamber
    {
        get { return isPurpleChamber; }
    }
    
    public bool IsGreenChamber
    {
        get { return isGreenChamber; }
    }
    
    public bool IsBlueChamber
    {
        get { return isBlueChamber; }
    }
    
    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool left;
    [SerializeField] private bool right;

    public bool Up
    {
        get { return up; }
    }
    
    public bool Down
    {
        get { return down; }
    }
    
    public bool Left
    {
        get { return left; }
    }
    
    public bool Right
    {
        get { return right; }
    }
    
    // 0 = not changed,
    // 1 = counter-clockwise 90,
    // 2 = counter-clockwise 180,
    // 3 = counter-clockwise 270
    public int rotate = 0;

    public bool upConnected()
    {
        if (rotate == 0)
        {
            return up;
        }
        else if (rotate == 1)
        {
            return right;
        }
        else if (rotate == 2)
        {
            return down;
        }
        else
        {
            return left;
        }
    }
    
    public bool downConnected()
    {
        if (rotate == 0)
        {
            return down;
        }
        else if (rotate == 1)
        {
            return left;
        }
        else if (rotate == 2)
        {
            return up;
        }
        else
        {
            return right;
        }
    }
    
    public bool leftConnected()
    {
        if (rotate == 0)
        {
            return left;
        }
        else if (rotate == 1)
        {
            return up;
        }
        else if (rotate == 2)
        {
            return right;
        }
        else
        {
            return down;
        }
    }
    
    public bool rightConnected()
    {
        if (rotate == 0)
        {
            return right;
        }
        else if (rotate == 1)
        {
            return down;
        }
        else if (rotate == 2)
        {
            return left;
        }
        else
        {
            return up;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
