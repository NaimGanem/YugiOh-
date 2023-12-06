using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public player player;

    public Deck myDeck;
    public Transform[] HandCardsPositions;

    public List<CardDisplay> CardsInHand = new List<CardDisplay>();

    public static Transform myTransform;

    public int PlayerHealth = 500;
    public TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        UpdateHealth(0);
        myDeck = GetComponentInChildren<Deck>();
        StartCoroutine(CallInitHand());
    }

    public void ResetStats()
    {
        PlayerHealth = 500;
        healthText.text = "HP: " + PlayerHealth.ToString();
        foreach (CardDisplay c in CardsInHand)
        {
            Destroy(c.gameObject);
        }
        CardsInHand?.Clear();
        myDeck.ResetDeck();
        StartCoroutine(CallInitHand());
    }
    public void UpdateHealth(int amount)
    {
        PlayerHealth -= amount;
        healthText.text = "HP: " + PlayerHealth.ToString();
        if(PlayerHealth <= 0)
        {
            //gameover
            if(player == player.P1)
            {
                GameManager.Instance.isPlayer1Win = false;
                GameManager.Instance.GameOver();
            }
            else
            {
                GameManager.Instance.isPlayer1Win = true;
                GameManager.Instance.GameOver();
            }
        }
    }

    public void StartTurn()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        Debug.Log("Start turn " + player.ToString());
        ToggleCards(true);

        if(IsBot())
        {

            StartCoroutine(WaitForAIturn());
            
        }

    }

    private IEnumerator WaitForAIturn()
    {
        if (GameManager.Instance.isGameOver)
            yield break;

        yield return new WaitForSeconds(2f);
        ManageDecisions();

        yield return null;
    }

    private void ManageDecisions()
    {
        int randomAction = Random.Range(1, 4);
        if (CardsInHand.Count == 0)
        {
            randomAction = 2;
        }


        if (randomAction == 1)
        {
            Debug.Log("AI put card in battlefield");
            int rand = Random.Range(0, CardsInHand.Count);

            CardsInHand[rand].GetComponent<CardController>().PutCardInBattleField();
        }
        else if (randomAction == 2)
        {
            Debug.Log("AI draw card");
            if(CardsInHand.Count < 5)
            {
                InitHand(1);
                EndTurn();
            }
            else
            {
                Debug.Log("AI hand is full, make another decision");
                ManageDecisions();
            }
            
        }
        else if (randomAction == 3)
        {
            Debug.Log("AI Attack");
            //Attack
            if(BattleFIeldManager.Instance.P2BattleField.Count > 0 && BattleFIeldManager.Instance.P1BattleField.Count > 0)
            {
                int rand = Random.Range(0, BattleFIeldManager.Instance.P2BattleField.Count);
                CardController attacker = BattleFIeldManager.Instance.P2BattleField[rand].
                    GetComponent<CardController>();
                attacker.cardDisplay.ToggleHighlight(true);

                int rand2 = Random.Range(0, BattleFIeldManager.Instance.P1BattleField.Count);
                CardController target = BattleFIeldManager.Instance.P1BattleField[rand2].
                    GetComponent<CardController>();

                attacker.GetComponent<AttackHandler>().TriggerBotAttack(attacker, target);
                EndTurn();
            }
            else
            {
                ManageDecisions();
            }

            
        }
    }

    public void EndTurn()
    {
        Debug.Log("End Turn " + player.ToString());
        ToggleCards(false);
        GameManager.Instance.ManageTurns();
    }

    private void ToggleCards(bool state)
    {
        myDeck.ToggleCanPlay(state);
        if(CardsInHand.Count > 0)
        {
            foreach (CardDisplay c in CardsInHand)
            {
                c.GetComponent<CardController>().ToggleCanPlay(state);
            }
        }
        

        switch (player)
        {
            case player.P1:
                if(BattleFIeldManager.Instance.P1BattleField.Count > 0)
                {
                    foreach (CardDisplay c in BattleFIeldManager.Instance.P1BattleField)
                    {
                        c.GetComponent<CardController>().ToggleCanPlay(state);
                    }
                }
                
                break;

            case player.P2:
                if (BattleFIeldManager.Instance.P1BattleField.Count > 0)
                {
                    foreach (CardDisplay c in BattleFIeldManager.Instance.P2BattleField)
                    {
                        c.GetComponent<CardController>().ToggleCanPlay(state);
                    }
                }
                
                break;
           
            default:
                break;
        }

    }
    private IEnumerator CallInitHand()
    {
        Debug.Log("waiting for deck to complete...");
        yield return new WaitUntil(() => myDeck.cards.Count == myDeck.deckSize);

        Debug.Log("calling init hand");
            
        InitHand(5);
        yield break;
    }
 
    public void InitHand(int cardsCount)
    {
        
        Debug.Log("init hand start");
        for (int i = 0; i < cardsCount; i++)
        {
            Transform handPos = null;
            foreach (Transform hand in HandCardsPositions)
            {

               if(hand.childCount == 0)
                {
                    handPos = hand;
                    break;
                }
            }

            if(handPos == null)
            {
                Debug.Log("no free hand!");
                return;
            }

            GameObject newCard = myDeck.DrawCard();
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            CardsInHand.Add(cardDisplay);
            
            newCard.transform.SetParent(handPos);
            //newCard.transform.position = Vector3.zero;
            newCard.GetComponent<RectTransform>().localPosition = Vector3.zero;
            newCard.GetComponent<CardController>().controller = this;
            if (!IsBot())
            {
                cardDisplay.RevealCard(false);
            }
            
        }
    }

    public void DiscardFromHand(GameObject card)
    {
        CardsInHand.Remove(card.GetComponent<CardDisplay>());
 
    }


    public bool IsBot()
    {
        if(player == player.P1)
        {
            return false;
        }

        return true;
    }
}

public enum player
{
    P1,
    P2
}

