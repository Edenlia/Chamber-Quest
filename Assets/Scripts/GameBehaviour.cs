using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public struct MoveStateAttribs
{
    public int maxMove;
}

public struct RotateCardAttribs
{
    public int tileX;
    public int tileY;
    public int direction; // -1 not set, 0 counterclockwise, 1 clockwise
}

public struct PushCardAttribs
{
    public int isHorizontal; // -1 not set, 0 false, 1 True
    public int isPositive; // -1 not set, 0 false, 1 True
    public int index; // -1 not set
}

public struct SwapCardAttribs
{
    public int tileX1;
    public int tileY1;
    public int tileX2;
    public int tileY2;
}

public class GameBehaviour : MonoBehaviour
{
    public GameObject gridController;
    public GameObject playerText;
    public GameObject stageText;
    public GameObject pauseMenuCanvas;
    public GameObject endMenuCanvas;
    public GameObject endText;
    public GameObject cardSelectCanvas;
    public GameObject pushSelectCanvas;
    
    public GameObject rotateCardButton;
    public GameObject pushCardButton;
    public GameObject swapCardButton;
    
    private int curPlayerInd = 0;
    private GameObject curPlayer;
    private int turnCount = 0;
    // 0 = roll the dice, 1 = move, 2 = select action card
    // 3 = rotate, 4 = push, 5 = swap
    private int stage = 0; 
    
    
    private MoveStateAttribs curMoveState;
    private RotateCardAttribs curRotateCard;
    private PushCardAttribs curPushCard;
    private SwapCardAttribs curSwapCard;

    void NextTurn()
    {
        if (curPlayerInd == 3)
        {
            turnCount++;
        }
        curPlayerInd = (curPlayerInd + 1) % 4;
        AssignPlayer();
        stage = 0;
        stageText.GetComponent<TextMeshProUGUI>().text = "Press Space to roll the dice";
    }
    
    void EndGame()
    {
        stage = -1;
        endMenuCanvas.SetActive(true);

        switch (curPlayerInd)
        {
            case 0:
                endText.GetComponent<TextMeshProUGUI>().text = "Red player wins!";
                endText.GetComponent<TextMeshProUGUI>().color = Color.red;
                break;
            case 1:
                endText.GetComponent<TextMeshProUGUI>().text = "Purple player wins!";
                endText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                break;
            case 2:
                endText.GetComponent<TextMeshProUGUI>().text = "Green player wins!";
                endText.GetComponent<TextMeshProUGUI>().color = Color.green;
                break;
            case 3:
                endText.GetComponent<TextMeshProUGUI>().text = "Blue player wins!";
                endText.GetComponent<TextMeshProUGUI>().color = Color.blue;
                break;
        }
    }
    
    void AssignPlayer()
    {
        curPlayer = gridController.GetComponent<GridBehaviour>().GetCurrentPlayer(curPlayerInd);
        int rotateCardCount = curPlayer.GetComponent<PlayerAttribs>().rotateCardCount;
        int pushCardCount = curPlayer.GetComponent<PlayerAttribs>().pushCardCount;
        int swapCardCount = curPlayer.GetComponent<PlayerAttribs>().swapCardCount;
        string playerColor = "";
        Color textColor = Color.clear;
        switch (curPlayerInd)
        {
            case 0:
                playerColor = "Red";
                textColor = Color.red;
                break;
            case 1:
                playerColor = "Purple";
                textColor = Color.magenta;
                break;
            case 2:
                playerColor = "Green";
                textColor = Color.green;
                break;
            case 3:
                playerColor = "Blue";
                textColor = Color.blue;
                break;
        }
        playerText.GetComponent<TextMeshProUGUI>().text = playerColor + "Player's Turn!\n" +
                                                         "Rotate Card: " + rotateCardCount + "\n" +
                                                         "Push Card: " + pushCardCount + "\n" +
                                                         "Swap Card: " + swapCardCount;
        playerText.GetComponent<TextMeshProUGUI>().color = textColor;
    }
    
    public void SelectRotateCard()
    {
        if (curPlayer.GetComponent<PlayerAttribs>().rotateCardCount > 0)
        {
            RotateCardAttribs rotateCard = new RotateCardAttribs();
            rotateCard.tileX = -1;
            rotateCard.tileY = -1;
            rotateCard.direction = -1;
            curRotateCard = rotateCard;
            stage = 3;
            cardSelectCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("No rotate card left");
        }
    }
    
    public void SelectPushCard()
    {
        if (curPlayer.GetComponent<PlayerAttribs>().pushCardCount > 0)
        {
            PushCardAttribs pushCard = new PushCardAttribs();
            pushCard.isHorizontal = -1;
            pushCard.isPositive = -1;
            pushCard.index = -1;
            curPushCard = pushCard;
            stage = 4;
            cardSelectCanvas.SetActive(false);
            pushSelectCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("No push card left");
        }
    }
    
    public void SelectSwapCard()
    {
        if (curPlayer.GetComponent<PlayerAttribs>().swapCardCount > 0)
        {
            SwapCardAttribs swapCard = new SwapCardAttribs();
            swapCard.tileX1 = -1;
            swapCard.tileY1 = -1;
            swapCard.tileX2 = -1;
            swapCard.tileY2 = -1;
            curSwapCard = swapCard;
            stage = 5;
            cardSelectCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("No swap card left");
        }
    }
    
    public void SelectSkipCard()
    {
        stage = 0;
        cardSelectCanvas.SetActive(false);
        NextTurn();
    }
    
    public void PushButton0()
    {
        curPushCard.index = 0;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton1()
    {
        curPushCard.index = 1;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton2()
    {
        curPushCard.index = 2;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton3()
    {
        curPushCard.index = 3;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton4()
    {
        curPushCard.index = 4;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton5()
    {
        curPushCard.index = 5;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton6()
    {
        curPushCard.index = 0;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton7()
    {
        curPushCard.index = 1;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton8()
    {
        curPushCard.index = 2;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton9()
    {
        curPushCard.index = 3;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton10()
    {
        curPushCard.index = 4;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton11()
    {
        curPushCard.index = 5;
        curPushCard.isHorizontal = 0;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton12()
    {
        curPushCard.index = 0;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton13()
    {
        curPushCard.index = 1;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton14()
    {
        curPushCard.index = 2;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton15()
    {
        curPushCard.index = 3;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton16()
    {
        curPushCard.index = 4;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton17()
    {
        curPushCard.index = 5;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 0;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton18()
    {
        curPushCard.index = 0;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton19()
    {
        curPushCard.index = 1;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton20()
    {
        curPushCard.index = 2;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton21()
    {
        curPushCard.index = 3;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton22()
    {
        curPushCard.index = 4;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }
    
    public void PushButton23()
    {
        curPushCard.index = 5;
        curPushCard.isHorizontal = 1;
        curPushCard.isPositive = 1;
        pushSelectCanvas.SetActive(false);
        TriggerPush();
    }

    private void TriggerPush()
    {
        gridController.GetComponent<GridBehaviour>().PlayerPushTiles(
            curPushCard.isHorizontal == 1, curPushCard.isPositive == 1, curPushCard.index);
        curPlayer.GetComponent<PlayerAttribs>().pushCardCount--;
        pushSelectCanvas.SetActive(false);
        stage = 0;
        NextTurn();
    }
    
    public void ToSelectCardStage()
    {
        if (stage == -1) return;
        stage = 2;
        cardSelectCanvas.SetActive(true);
        if (curPlayer.GetComponent<PlayerAttribs>().rotateCardCount == 0)
        {
            rotateCardButton.GetComponent<Button>().interactable = false;
        }
        if (curPlayer.GetComponent<PlayerAttribs>().pushCardCount == 0)
        {
            pushCardButton.GetComponent<Button>().interactable = false;
        }
        if (curPlayer.GetComponent<PlayerAttribs>().swapCardCount == 0)
        {
            swapCardButton.GetComponent<Button>().interactable = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.SetActive(false);
        endMenuCanvas.SetActive(false);
        cardSelectCanvas.SetActive(false);
        pushSelectCanvas.SetActive(false);
        stageText.GetComponent<TextMeshProUGUI>().text = "Press Space to roll the dice";
    }

    // Update is called once per frame
    void Update()
    {
        if (curPlayer == null)
        {
            AssignPlayer();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuCanvas.SetActive(!pauseMenuCanvas.activeSelf);
        }
        
        switch (stage)
        {
            case 0:
                // roll the dice
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    int diceRoll = Random.Range(1, 7);
                    Debug.Log("Dice roll: " + diceRoll);
                    
                    MoveStateAttribs moveState = new MoveStateAttribs();
                    moveState.maxMove = diceRoll;
                    curMoveState = moveState;
                    stage = 1;
                    stageText.GetComponent<TextMeshProUGUI>().text = "You rolled " + diceRoll + "\nSelect a tile to move to";
                }
                break;
            
            case 1:
                if (Input.GetMouseButtonDown(0)) // left click
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
                    int fromTileX = curPlayer.GetComponent<PlayerAttribs>().X;
                    int fromTileY = curPlayer.GetComponent<PlayerAttribs>().Y;

                    int toTileX = 0, toTileY = 0;
                    if (gridController.GetComponent<GridBehaviour>().GetSelectedTile(mousePos, ref toTileX, ref toTileY))
                    {
                        int len = gridController.GetComponent<GridBehaviour>()
                            .MinLength(fromTileX, fromTileY, toTileX, toTileY);
                        if (len != -1 && len <= curMoveState.maxMove)
                        {
                            // Debug.Log("From: " + fromTileX + " " + fromTileY + " To: " + toTileX + " " + toTileY + " Len: " + len);
                            curPlayer.GetComponent<PlayerAttribs>().X = toTileX;
                            curPlayer.GetComponent<PlayerAttribs>().Y = toTileY;
                            
                            if ((curPlayerInd == 0 && gridController.GetComponent<GridBehaviour>().IsRedChamber(toTileX, toTileY)) ||
                                (curPlayerInd == 1 && gridController.GetComponent<GridBehaviour>().IsPurpleChamber(toTileX, toTileY)) ||
                                (curPlayerInd == 2 && gridController.GetComponent<GridBehaviour>().IsGreenChamber(toTileX, toTileY)) ||
                                (curPlayerInd == 3 && gridController.GetComponent<GridBehaviour>().IsBlueChamber(toTileX, toTileY)))
                            {
                                EndGame();
                            }
                            
                            ToSelectCardStage();
                        }
                        else
                        {
                            Debug.Log("Invalid move");
                        }
                    }
                }
                break;
            
            case 2:
                
                break;
            
            case 3:
                if (curRotateCard.tileX == -1)
                {
                    stageText.GetComponent<TextMeshProUGUI>().text = "Select a tile to rotate";
                    
                    if (Input.GetMouseButtonDown(0)) // left click
                    {
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        int tileX = 0, tileY = 0;
                        if (gridController.GetComponent<GridBehaviour>().GetSelectedTile(mousePos, ref tileX, ref tileY))
                        {
                            if (!gridController.GetComponent<GridBehaviour>().IsOccupied(tileX, tileY))
                            {
                                curRotateCard.tileX = tileX;
                                curRotateCard.tileY = tileY;
                            }
                            else
                            {
                                Debug.Log("Invalid tile");
                            }
                        }
                    }
                }
                else if (curRotateCard.direction == -1)
                {
                    stageText.GetComponent<TextMeshProUGUI>().text = "You selected tile " + curRotateCard.tileX + " " + 
                                                                     curRotateCard.tileY + 
                                                                     "\nSelect direction to rotate\n1. Counterclockwise\n2. Clockwise";
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        curRotateCard.direction = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        curRotateCard.direction = 1;
                    }
                }
                else
                {
                    gridController.GetComponent<GridBehaviour>().PlayerRotateTile(
                        curRotateCard.tileX, curRotateCard.tileY, curRotateCard.direction == 1);
                    curPlayer.GetComponent<PlayerAttribs>().rotateCardCount--;
                    stage = 0;
                    NextTurn();
                }
                break;
            
            case 4:
                // if (curPushCard.isHorizontal == -1)
                // {
                //     if (Input.GetKeyDown(KeyCode.Alpha1)) // left click
                //     {
                //         curPushCard.isHorizontal = 0;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha2))
                //     {
                //         curPushCard.isHorizontal = 1;
                //     }
                // }
                // else if (curPushCard.isPositive == -1)
                // {
                //     if (Input.GetKeyDown(KeyCode.Alpha1))
                //     {
                //         curPushCard.isPositive = 0;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha2))
                //     {
                //         curPushCard.isPositive = 1;
                //     }
                // }
                // else if (curPushCard.index == -1)
                // {
                //     if (Input.GetKeyDown(KeyCode.Alpha1))
                //     {
                //         curPushCard.index = 0;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha2))
                //     {
                //         curPushCard.index = 1;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha3))
                //     {
                //         curPushCard.index = 2;
                //     }
                //     if (Input.GetKeyDown(KeyCode.Alpha4))
                //     {
                //         curPushCard.index = 3;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha5))
                //     {
                //         curPushCard.index = 4;
                //     }
                //     else if (Input.GetKeyDown(KeyCode.Alpha6))
                //     {
                //         curPushCard.index = 5;
                //     }
                // }
                // else
                // {
                //     gridController.GetComponent<GridBehaviour>().PlayerPushTiles(
                //         curPushCard.isHorizontal == 1, curPushCard.isPositive == 1, curPushCard.index);
                //     curPlayer.GetComponent<PlayerAttribs>().pushCardCount--;
                //     stage = 0;
                //     NextTurn();
                // }
                break;
            
            case 5:
                if (curSwapCard.tileX1 == -1)
                {
                    stageText.GetComponent<TextMeshProUGUI>().text = "Select 2 tile to swap\nTile 1:";
                    if (Input.GetMouseButtonDown(0)) // left click
                    {
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        int tileX = 0, tileY = 0;
                        if (gridController.GetComponent<GridBehaviour>().GetSelectedTile(mousePos, ref tileX, ref tileY))
                        {
                            if (!gridController.GetComponent<GridBehaviour>().IsOccupied(tileX, tileY))
                            {
                                curSwapCard.tileX1 = tileX;
                                curSwapCard.tileY1 = tileY;
                                Debug.Log("Tile 1: " + tileX + " " + tileY);
                            }
                            else
                            {
                                Debug.Log("Invalid tile");
                            }
                        }
                    }
                }
                else if (curSwapCard.tileX2 == -1)
                {
                    stageText.GetComponent<TextMeshProUGUI>().text = "Select 2 tile to swap\n" +
                                                                     "Tile 1: " + (curSwapCard.tileX1 + 1) + " " + (curSwapCard.tileY1 + 1) + "\n" +
                                                                     "Tile 2:";
                    if (Input.GetMouseButtonDown(0)) // left click
                    {
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        int tileX = 0, tileY = 0;
                        if (gridController.GetComponent<GridBehaviour>().GetSelectedTile(mousePos, ref tileX, ref tileY))
                        {
                            if (!gridController.GetComponent<GridBehaviour>().IsOccupied(tileX, tileY))
                            {
                                curSwapCard.tileX2 = tileX;
                                curSwapCard.tileY2 = tileY;
                                Debug.Log("Tile 2: " + tileX + " " + tileY);
                            }
                            else
                            {
                                Debug.Log("Invalid tile");
                            }
                        }
                    }
                }
                else
                {
                    gridController.GetComponent<GridBehaviour>().PlayerSwapTiles(
                        curSwapCard.tileX1, curSwapCard.tileY1, curSwapCard.tileX2, curSwapCard.tileY2);
                    curPlayer.GetComponent<PlayerAttribs>().swapCardCount--;
                    stage = 0;
                    NextTurn();
                }

                break;
        }
    }
}
