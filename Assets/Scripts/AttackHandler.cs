using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AttackHandler : MonoBehaviour, IPointerClickHandler
{
    public bool isClickable;
    public bool isEnemy;

    CardController Controller;
    private void Awake()
    {
        Controller = GetComponent<CardController>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("@@@@@@@@@@@@@@@@@Click");

        if (!isClickable)
            return;

        if(!isEnemy)
        {
            if (GameManager.Instance.turn == turns.P1turn)
            {
                foreach (CardDisplay c in BattleFIeldManager.Instance.P1BattleField)
                {
                    c.ToggleHighlight(false);
                }
                foreach (CardDisplay card in BattleFIeldManager.Instance.P2BattleField)
                {
                    card.GetComponent<CardController>().ToggleCanPlay(true);
                    
                    //card.GetComponent<AttackHandler>().ToggleClickable(true);
                    card.GetComponent<AttackHandler>().isEnemy = true;

                    Controller.cardDisplay.ToggleHighlight(true);
                    GameManager.Instance.CardAttacker = this;
                }

            }
            if (GameManager.Instance.turn == turns.P2turn)
            {
                foreach (CardDisplay c in BattleFIeldManager.Instance.P2BattleField)
                {
                    c.ToggleHighlight(false);
                }
                foreach (CardDisplay card in BattleFIeldManager.Instance.P1BattleField)
                {
                    card.GetComponent<CardController>().ToggleCanPlay(true);
                    
                    card.GetComponent<AttackHandler>().isEnemy = true;

                    Controller.cardDisplay.ToggleHighlight(true);
                    GameManager.Instance.CardAttacker = this;
                }

            }
        }
        else
        {
            Debug.Log("choosing enemy");
            if(GameManager.Instance.CardAttacker != null)
            {
                TriggerAttack();
            }
           
        }
      
    }

    public void TriggerAttack()
    {
     GameManager.Instance.CardAttacker.
     GetComponent<CardDisplay>().ToggleHighlight(false);

        Controller.GetHit(GameManager.Instance.CardAttacker.
      GetComponent<CardDisplay>().attackPoints);
        GetComponent<CardController>().controller.EndTurn();
    }

    public void TriggerBotAttack(CardController attacker, CardController target)
    {
        attacker.GetComponent<CardDisplay>().ToggleHighlight(false);

        target.GetHit(attacker.GetComponent<CardDisplay>().attackPoints);
        
    }

    public void ToggleClickable(bool state)
    {
        isClickable = state;
     
    }
}
