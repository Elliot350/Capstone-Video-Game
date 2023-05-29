using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatus : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Slider slider;

    public void SetHero(Hero hero)
    {
        image.sprite = hero.GetSprite();
        slider.minValue = 0;
        slider.maxValue = hero.GetMaxHealth();
        SetHeroHealth(hero);
    }

    public void SetHeroHealth(Hero hero)
    {
        slider.value = hero.GetHealth();
    }
}
