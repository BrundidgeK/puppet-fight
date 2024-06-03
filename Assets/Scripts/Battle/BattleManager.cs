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
        "right hook",
        "left hook",
        "hammer",
        "block",
        "duck",
        "idle"
    };
    //Cooldown times for each action
    Dictionary<string, float> seconds = new Dictionary<string, float>
        {
        {"right hook", 1.5f },
        {"left hook", 1.5f },
        {"hammer", 1.75f },
        {"block", 3f },
        {"duck", 1f },
        {"idle",.5f }
        };

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

    //UI
    public Slider p_healthBar, e_healthBar;
    public TMP_Text p_healthText, e_healthText;

    public ChangeScene changeScene;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("chooseEnemyMove", 1); //Enemy picks a move 1 second after scene has loaded
    }

    // Update is called once per frame
    void Update()
    {
        fight(); //Continously runs the fight function
    }

    public void fight()
    {
        bool playerDodged = false;
        bool enemyDodged = false;
        player = playerScript.FingerPositions();
        string[] playerMoveArray = player.Split(',');

        // Check if player or enemy dodged
        foreach (string move in playerMoveArray)
        {
            gfx.changePlayerSprites(move);
            if ((move == "lean right" && enemyMove == "left hook") ||
                (move == "lean left" && enemyMove == "right hook") ||
                (move == "duck" && (enemyMove == "right hook" || enemyMove == "left hook")))
            {
                playerDodged = true;
                TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
                a.text = "DODGE!";
            }
            else if (move == "block" && (enemyMove == "right hook" || enemyMove == "left hook" || enemyMove == "hammer"))
            {
                playerDodged = true;
                TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
                a.text = "BLOCK!";
            }
        }

        //Checks if the enemy dodged
        if ((enemyMove.Contains("lean right") && (player.Contains("left hook"))) ||
            (enemyMove.Contains("lean left") && (player.Contains("right hook"))) ||
            (enemyMove.Contains("duck") && ((player.Contains("right hook") || player.Contains("left hook")))))
        {
            enemyDodged = true;
            TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
            a.text = "DODGE!";
        }
        else if (enemyMove.Contains("block") && (player.Contains("right hook") || player.Contains("left hook")))
        {
            enemyDodged = true;
            TMP_Text a = Instantiate(feedbackText, new Vector3(Random.Range(start.x, end.x), Random.Range(start.y, end.y), Random.Range(start.z, end.z)), Quaternion.identity).GetComponent<TMP_Text>();
            a.text = "BLOCK!";
        }

        // Check outcomes based on moves
        if (!playerDodged) {
            if ((enemyMove.Contains("right hook") || enemyMove.Contains("left hook")))
            {
                Invoke("playerDamage", 1.5f);
            } else if (enemyMove == "hammer")
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

        //Updates gfx and UI accordingly
        gfx.changeEnemySprites(enemyMove);
        updateUI();

        //Gives the enemy a cooldown until its next move
        if (seconds.ContainsKey(enemyMove))
        {
            Invoke("chooseEnemyMove", seconds[enemyMove]);
            if (enemyMove == "left hook" || enemyMove == "right hook")
            {
                enemyMove = "none";
            } else
            {
                enemyMove += "m";
            }
        } 

        //Win/Lost State
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
        enemyMove = actions[Random.Range(0, 6)];
    }

    //Damages the player
    void playerDamage()
    {
        playerHP -= damage;
    }

    //Updates the health bars and HP text
    void updateUI()
    {
        p_healthBar.value = playerHP;
        e_healthBar.value = enemyHP;
        e_healthText.text = enemyHP + "/100";
        p_healthText.text = playerHP + "/100";
    }
}
