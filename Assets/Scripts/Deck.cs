using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private List <GameObject> optionalCards = new List<GameObject>();

    public List<GameObject> cards = new List<GameObject>();
    public int deckSize;
    public Transform DeckHolder;
    private float cardYoffest = 0;
    // Start is called before the first frame update
    void Start()
    {
        CreateCards(deckSize);
    }

    private void CreateCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newCard =
                Instantiate(optionalCards[Random.Range(0, optionalCards.Count)], DeckHolder);

            cardYoffest += 1f;
            newCard.GetComponent<CardDisplay>().id = i;
            newCard.transform.position = new Vector2(newCard.transform.position.x
                , newCard.transform.position.y - cardYoffest);


            cards.Add(newCard);
            newCard.GetComponent<CardController>().ToggleCanPlay(false);


        }
    }

    public void ResetDeck()
    {
        int cardsToAdd = deckSize - cards.Count;
        CreateCards(cardsToAdd);
    }

    public GameObject DrawCard()
    {
        GameObject currentCard = cards.Last();
        cards.Remove(currentCard);

        return currentCard;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        PlayerController pc = GetComponentInParent<PlayerController>();
        if (pc.CardsInHand.Count == 5)
        {
            return;
        }

        pc.InitHand(1);
        pc.EndTurn();

    }

    public void ToggleCanPlay(bool state)
    {
        GetComponent<GraphicRaycaster>().enabled = state;
    }
}
