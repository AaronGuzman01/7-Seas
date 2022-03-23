using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



//[Serializable]

public class Player : MonoBehaviour
{

    public Vector2 homePort;
    public int buriedTreasure = 0;
    public Color safeColor;
    public int[] shipsPirated;
    public int powderKegs;
    public int canonBalls;
    public int gold;
    public bool onSafeColor;
    public Vector2 shipPos;
    public bool loseThisTurn;
    public int shipOn; //(shoal, ocean, deep blue, buriedtreasure)
    public bool isOnLava = false;
    public int ratCount;
    public int ghostDice = 0;
    public SaveEngine saveEngine;
    public LoadTargetScreenButton loadTarScreen;
    public int playerNum = 0;

    public GameLoop gameLoop;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool move = false;
    //float timeStartedMoving;
    //float timeTaken = .2f;
    public bool isMyTurn = false;
    public GameObject wheel;
    public GameObject octagon;
    public GameObject arrow;
    public GameObject parrot;
    public GameObject spotLight;

    public TileSprite tile;
    public TilingEngine tE;
    public TileSprite[,] playerMap;
    public List<TileSprite> PlayerSprites;

    public GameObject TileContainerPrefab;
    public GameObject TilePrefab;
    private GameObject playerTiles;
    public Sirens siren;
    private List<GameObject> _tiles = new List<GameObject>();
    Collision2D colTar;


    public float speed;

    private Player()
    {

    }

    // Use this for initialization
    void Start()
    {
        //initialize playermap and set the player tiles on board
        playerMap = new TileSprite[(int)tE.MapSize.x, (int)tE.MapSize.y];

        SetTiles();

    }

    //sets the player tiles on board
    private void SetTiles()
    {
        for (var y = 0; y < tE.MapSize.y; y++)
        {
            for (var x = 0; x < tE.MapSize.x; x++)
            {
                playerMap[x, y] = new TileSprite();

            }
        }
    }


    //adds the player tiles to the world
    private void AddTilesToWorld()
    {
        playerTiles = Instantiate(TileContainerPrefab);
        for (var y = 0; y < tE.MapSize.y; y++)
        {
            for (var x = 0; x < tE.MapSize.x; x++)
            {
                var tX = x * tE.tileSize;
                var tY = y * tE.tileSize;


                //add tile prefabs and position
                var t = Instantiate(TilePrefab);
                t.transform.position = new Vector3(tX, -tY, 0);
                t.transform.SetParent(playerTiles.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = playerMap[x, y].TileImage;

                _tiles.Add(t);

            }
        }
    }


    void Update()
    {
        //turn the player wheel on or off based on if its their turn or not
        if (isMyTurn)
        {
            wheel.SetActive(true);

        }
        else
        {
            wheel.SetActive(false);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if we hit a player ship
        if (col.gameObject.tag == "Player")
        {
            var hitPlyr = col.gameObject.GetComponent<Player>();

            if (isMyTurn && !siren.move && ratCount < gameLoop.RATS_MAX && !hitPlyr.onSafeColor)
            {
                //if we hit another player
                hitPlyr.transform.position = new Vector3(hitPlyr.homePort.x * (int)tE.tileSize, -hitPlyr.homePort.y * (int)tE.tileSize, hitPlyr.transform.position.z);
                gameLoop.alertWindow.ShowAlertWindow("Player " + hitPlyr.playerNum + " has been pirated!");

                //if double gold event get twice the gold
                if (gameLoop.roundEvents.currentEvent == "Double Gold")
                {
                    this.gold += hitPlyr.gold * 2;
                }
                //else just get the other players gold amount
                else
                {
                    this.gold += hitPlyr.gold;
                }
                hitPlyr.gold = 0;
                gameLoop.UpdateUI();
                gameLoop.CheckForWin();

            }
        }
        //else if we hit a target ship
        else if(col.gameObject.tag == "TargetShip")
        {
            colTar = col;
            //make sure we arent going toward siren or have too many rats
            if (!siren.move && ratCount < gameLoop.RATS_MAX)
            {
                //make sure we have enough ammo
                if (powderKegs > 0 && canonBalls > 0)
                {
                    //events to load the cannon screen
                    gameLoop.targetShips.RepositionShip(col);
                    PlayerPrefs.SetInt("collidedShipGold", col.gameObject.GetComponent<TargetShip>().gold);
                    saveEngine.Save();
                    loadTarScreen.LoadSceneNum(6);
                }

            }
            
        }
    }
}
