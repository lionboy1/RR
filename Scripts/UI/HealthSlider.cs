using System.Collections;
using System.Collections.Generic;
using RR.Core;
using UnityEngine;
using UnityEngine.UI;

//Only the player health should reference this, not the AI's.

///Borrowed the idea from https://www.youtube.com/watch?v=BLfNP4Sc_iA

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fillableImage;

    void Start()
    {
        SetMaxHealth(100);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fillableImage.color = gradient.Evaluate(1f);
    }

    // Update is called once per frame
    public void UpdateHealth(int healthAmount)
    {
        slider.value = healthAmount;
        fillableImage.color = gradient.Evaluate(slider.normalizedValue);//translates to 0-1.
    }
}
