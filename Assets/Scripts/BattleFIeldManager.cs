using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleFIeldManager : MonoBehaviour
{
    public static BattleFIeldManager Instance;

    public List<CardDisplay> P1BattleField = new List<CardDisplay>();
    public List<CardDisplay> P2BattleField = new List<CardDisplay>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCardToBattleField(CardDisplay card , player player)
    {
        if(player == player.P1)
        {
            P1BattleField.Add(card);
        }
        else
        {
            card.CardBack.gameObject.SetActive(false);
            P2BattleField.Add(card);
            
        }
    }

    public void RemoveFromField(CardDisplay card)
    {
        if(P1BattleField.Count > 0)
        {
            foreach (CardDisplay c1 in P1BattleField.ToList())
            {
                if (card == c1)
                {
                    P1BattleField.Remove(card);
                    return;
                }
            }
        }
        
        if(P2BattleField.Count > 0)
        {
            foreach (CardDisplay c2 in P2BattleField.ToList())
            {
                if (card == c2)
                {
                    P2BattleField.Remove(card);
                }
            }
        }
        
    }

    public void RemoveAllCards()
    {
        foreach (CardDisplay c in P1BattleField.ToList())
        {
            Destroy(c.gameObject,0.1f);
            Debug.Log(c + "Destroyed");
        }


        foreach (CardDisplay c2 in P2BattleField.ToList())
        {
            Destroy(c2.gameObject, 0.1f);
            Debug.Log(c2 + "Destroyed");
        }

        P1BattleField?.Clear();

        P2BattleField?.Clear();
    }
}
