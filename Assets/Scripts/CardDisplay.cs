using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public int id;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardAtt;
    public TextMeshProUGUI cardDef;

    public int defPoints;
    public int attackPoints;

    public Image img;
    public Image CardBack;
    public Image highlight;
    // Start is called before the first frame update
    void Start()
    {
        defPoints = card.defPoints;
        attackPoints = card.attackPoints;

        cardName.text = card.cardName;
        cardDescription.text = card.description;
        
        img.sprite = card.sprite;
        UpdateCardDisplay();
        ToggleHighlight(false);

    }

    public void ToggleHighlight(bool toggle)
    {
        highlight.gameObject.SetActive(toggle);
    }
    public void UpdateCardDisplay()
    {
        cardAtt.text = "ATK/" + attackPoints.ToString();
        cardDef.text = "DEF/" + defPoints.ToString();
    }
    public void RevealCard(bool cover)
    {
        
        CardBack.gameObject.SetActive(cover);
    }
}
