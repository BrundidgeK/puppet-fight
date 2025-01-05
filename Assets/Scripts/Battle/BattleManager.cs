using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    // A list of actions for the enemy to take
    private string[] actions =
    {
        "right hook", // Active Moves: 0-2
        "left hook",
        "hammer",
        "block", //Passive Moves: 3-5
        "duck",
        "idle"
    };
    private int endActiveIndex = 2, endPassiveIndex = 5;
    bool activeAction;

    //Cooldown times for each action
    Dictionary<string, int> seconds = new Dictionary<string, int>
        {
        {"right hook", 2 },
        {"left hook", 2 },
        {"hammer", 3 },
        {"block", 1 },
        {"duck", 1 },
        {"idle", 1 }
        };
    [SerializeField]
    private int currentTime = 0;

    //Text that appears when a certain event occurs (dodge, block, etc.)
    public GameObject feedbackText;
    //Spawnpoint range for feedback text object
    public Vector3 start, end;

    //Stores the enemy and player moves
    public string enemyMove, player;
    int damage = 5; // Base damage for hooks
    int hammerDamage = 10; // Damage for hammers

    //Initial health
    public int playerHP = 100;
    public int enemyHP = 100;

    public Player playerScript;
    private bool leftHook = false, rightHook = false;

    // Manages the graphics of the player and enemy models
    public GFXManager gfx;

    public GameObject fightButton;
    public TMP_Text enemyAttackText;

    //UI
    public Slider p_healthBar, e_healthBar;
    public TMP_Text p_healthText, e_healthText;

    public ChangeScene changeScene;

    // Start is called before the first frame update
    void Start()
    {
        chooseEnemyMove();
    }

    // Update is called once per frame
    void Update()
    {
        player = playerScript.FingerPositions();
        string[] playerMoveArray = player.Split(',');
        foreach (string move in playerMoveArray)
        {
            gfx.changePlayerSprites(move);
        }
        updateUI();
    }

    public void fight()
    {
        currentTime++;
        bool playerDodged = false;
        bool enemyDodged = false;
        player = playerScript.FingerPositions();
        string[] playerMoveArray = player.Split(',');
        string currentEnemyMove = enemyMove;
        if (activeAction && seconds[enemyMove] != currentTime) // Enemy has to wait a set number of turns before striking
        {
            currentEnemyMove = "idle";
        }
        gfx.changeEnemySprites(enemyMove, seconds[enemyMove] != currentTime);

        if (currentTime >= seconds[enemyMove])
        {
            fightButton.SetActive(false);
            Invoke("chooseEnemyMove", 1.75f);
        }
        // Otherwise, passive action occurs for that many turns

        // Check if player or enemy dodged
        foreach (string move in playerMoveArray)
        {
            gfx.changePlayerSprites(move);
            if ((move == "lean right" && currentEnemyMove == "left hook") ||
                (move == "lean left" && currentEnemyMove == "right hook") ||
                (move == "duck" && (currentEnemyMove == "right hook" || currentEnemyMove == "left hook")))
            {
                playerDodged = true;
                TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
                a.text = "DODGE!";
            }
            else if (move == "block" && (currentEnemyMove == "right hook" || currentEnemyMove == "left hook" || currentEnemyMove == "hammer"))
            {
                playerDodged = true;
                TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
                a.text = "BLOCK!";
            }
        }

        //Checks if the enemy dodged
        if ((currentEnemyMove.Contains("lean right") && (player.Contains("left hook"))) ||
            (currentEnemyMove.Contains("lean left") && (player.Contains("right hook"))) ||
            (currentEnemyMove.Contains("duck") && ((player.Contains("right hook") || player.Contains("left hook")))))
        {
            enemyDodged = true;
            TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
            a.text = "DODGE!";
        }
        else if (currentEnemyMove.Contains("block") && (player.Contains("right hook") || player.Contains("left hook")))
        {
            enemyDodged = true;
            TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
            a.text = "BLOCK!";
        }

        // Check outcomes based on moves
        if (!playerDodged) {
            if ((currentEnemyMove.Contains("right hook") || currentEnemyMove.Contains("left hook")))
            {
                Invoke("playerDamage", 1.5f);
            } else if (currentEnemyMove == "hammer")
            {
                Invoke("playerDamage", 1.5f);
                Invoke("playerDamage", 1.5f);
            } 
        }
        if (!enemyDodged) {
            if ((player.Contains("right hook") || player.Contains("left hook")) && !player.Contains("hammer"))
            {
                enemyHP -= damage;
            } else if (player.Contains("hammer"))
            {
                enemyHP -= hammerDamage;
            }
        }

        // Win/Lost State
        if(enemyHP <= 0)
        {
            PlayerPrefs.SetInt("Win State", 1);
            changeScene.changeScene(0);
        } else if (playerHP <= 0)
        {
            PlayerPrefs.SetInt("Win State", -1);
            changeScene.changeScene(0);
        }
    }

    //Randomly picks an action for the enemy
    void chooseEnemyMove()
    {
        currentTime = 0;
        int index = Random.Range(0, 6);
        enemyMove = actions[index];
        activeAction = index <= endActiveIndex;
        gfx.changeEnemySprites(enemyMove, true);
        fightButton.SetActive(true);
    }

    //Damages the player
    void playerDamage()
    {
        playerHP -= damage;
    }

    //Updates the health bars and HP text
    void updateUI()
    {
        int turns = (seconds[enemyMove] - currentTime);
        if (activeAction)
            enemyAttackText.text = "Attacks in " + turns + " turn";
        else
            enemyAttackText.text = "Waiting " + turns + " turn";
        if (turns != 1)
            enemyAttackText.text += "s";

        p_healthBar.value = playerHP;
        e_healthBar.value = enemyHP;
        e_healthText.text = enemyHP + " / 100";
        p_healthText.text = playerHP + " / 100";
    }
}
