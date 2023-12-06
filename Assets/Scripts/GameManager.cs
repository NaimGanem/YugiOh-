using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController[] players;
    public PlayerController currentPlayerTurn;

    public turns turn;

    public AttackHandler CardAttacker;
    public AttackHandler cardGetHit;

    public GameObject PausePanel;
    public GameObject gameoverPanel;
    public GameObject BattleField;

    public GameObject p1;
    public GameObject p2;

    public TextMeshProUGUI winnerText;
    public bool isPlayer1Win;
    public bool isGameOver;
    #region multipalyergameMnage
  
    #endregion
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        players = FindObjectsOfType<PlayerController>();
        
    }

    private void Start()
    {
        Invoke("StartGame", 3f);
    }

    private void StartGame()
    {
        currentPlayerTurn = players[0];
        currentPlayerTurn.StartTurn();

    }

    public void ManageTurns()
    {
        Debug.Log("Manage Turns");

        if(currentPlayerTurn == players[0])
        {
            Debug.Log("player1 change to p2");
            currentPlayerTurn = players[1];
            turn = turns.P2turn;
            currentPlayerTurn.StartTurn();
            return;
        }

        if (currentPlayerTurn == players[1])
        {
            Debug.Log("player2 change to p1");
            currentPlayerTurn = players[0];
            turn = turns.P1turn;
            currentPlayerTurn.StartTurn();
            return;
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            FindObjectOfType<SC_MenuController>().ChangeVolume();
        }
    }
    public void GameOver()
    {
        gameoverPanel.SetActive(true);
        Time.timeScale = 0;
        isGameOver = true;
    }

    public void CallResetGame()
    {
        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        Time.timeScale = 1;

        currentPlayerTurn = players[0];
        BattleFIeldManager.Instance.RemoveAllCards();
        
        gameoverPanel.SetActive(false);

        foreach (PlayerController controller in players)
        {
            controller.ResetStats();
        }

        yield return new WaitForSeconds(1);

        isGameOver = false;
        ResumeGame();
        StartGame();
        yield break;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }
}


    

public enum turns
{
    P1turn,
    P2turn
}
