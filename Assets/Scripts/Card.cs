using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data" , menuName = "Card/newCard")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int attackPoints;
    public int defPoints;
    public Sprite sprite;

}
