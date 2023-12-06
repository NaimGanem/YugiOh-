using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour , IPointerExitHandler, IPointerDownHandler , IPointerEnterHandler , IPointerUpHandler , IDragHandler
{
    public PlayerController controller;

    private RectTransform m_rectTransform;
    private Canvas m_canvas;
    public Vector3 scaleFactor;
    private Vector3 initScale;
    public float liftCardFactor;
    public Transform battleFieldArea;
    public bool AreaDetected;
    public bool cardInBattleField;
    public CardDisplay cardDisplay;
    public AttackHandler attackHandler;
    public bool canPlay;
    private GraphicRaycaster raycaster;

    public void ToggleCanPlay(bool toggle)
    {
       
        canPlay = toggle;
        raycaster.enabled = canPlay;
        attackHandler.ToggleClickable(toggle);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cardInBattleField)
            return;

        m_rectTransform.transform.position = eventData.position;
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardInBattleField)
            return;

        transform.localScale = initScale;

        m_rectTransform.transform.position = eventData.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardInBattleField)
            return;

        m_canvas.sortingOrder = 2;
        m_rectTransform.transform.localPosition =
            new Vector2(m_rectTransform.transform.localPosition.x,
            m_rectTransform.transform.localPosition.y + liftCardFactor);

        transform.localScale += scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardInBattleField)
            return;

        m_canvas.sortingOrder = 1;
        m_rectTransform.transform.localPosition = Vector2.zero;

        transform.localScale = initScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (cardInBattleField)
            return;

        if (AreaDetected)
        {
            PutCardInBattleField();
        }
    }

    public void PutCardInBattleField()
    {
        string tagName = controller.player.ToString() + "_CardsHolder";
        battleFieldArea = GameObject.Find(tagName).transform;
        cardInBattleField = true;

        m_rectTransform.SetParent(battleFieldArea);
        m_rectTransform.localPosition = Vector3.zero;
        m_rectTransform.localScale = new Vector3(1, 1, 1);
        BattleFIeldManager.Instance.AddCardToBattleField(cardDisplay, controller.player);
        controller.DiscardFromHand(this.gameObject);

        controller.EndTurn();
    }

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvas = GetComponent<Canvas>();
        cardDisplay = GetComponent<CardDisplay>();
        raycaster = GetComponent<GraphicRaycaster>();
        attackHandler = GetComponent<AttackHandler>();
        initScale = transform.localScale;

    }
    // Start is called before the first frame update
    void Start()
    {
        
       
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cardInBattleField)
            return;
        string tagName = controller.player.ToString() + "_Area";
        if (collision.tag == tagName)
        {
            AreaDetected = true;
        }
        else
        {
            AreaDetected = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cardInBattleField)
            return;

        string tagName = controller.player.ToString() + "_Area";
        if (collision.tag == tagName)
        {        
            AreaDetected = false;
        }
    }

    public void GetHit(int amount)
    {
        Debug.Log("$$$$$$Getting HIT");
        int maxDamage = amount - cardDisplay.defPoints;
        if(amount > maxDamage)
        {
            cardDisplay.defPoints -= maxDamage;
            int damageLeft = amount - maxDamage;
            controller.UpdateHealth(damageLeft);
            return;
        }
        else
        {
            cardDisplay.defPoints -= amount;
        }


        cardDisplay.UpdateCardDisplay();
        if(cardDisplay.defPoints <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        BattleFIeldManager.Instance.RemoveFromField(cardDisplay);
        Destroy(gameObject, 0.1f);
        Debug.Log("DEAD************");
    }
}
