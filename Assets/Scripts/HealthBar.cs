using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private float health;
    private float maxHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    public void Set(float newHealth, float newMax)
    {
        SetMax(newMax);
        SetHealth(newHealth);
    }

    public void SetMax(float max)
    {
        maxHealth = ValidateMaxHealth(max);
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        UpdateSlider();
    }

    public void SetHealth(float newHealth)
    {
        health = ValidateHealth(newHealth);
        UpdateSlider();
    }

    private void UpdateSlider() 
    {
        slider.value = health;
        text.text = $"{health}/{maxHealth}";
    }

    private float ValidateMaxHealth(float num)
    {
        return Mathf.Abs(num);
    }

    private float ValidateHealth(float num)
    {
        return Mathf.Clamp(num, 0, maxHealth);
    }
}
