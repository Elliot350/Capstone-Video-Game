using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Hero/Hero Base")]
public class HeroBase : FighterBase
{
    [SerializeField] private int gold;

    public int GetGold() {return gold;}
}
