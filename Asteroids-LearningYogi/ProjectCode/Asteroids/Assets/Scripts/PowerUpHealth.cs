using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerUpHealth : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public bool isStart;
    private Coroutine _timerCoroutine;

    public PowerUPType currentType;

    [SerializeField] private Sprite sheildSprite;
    [SerializeField] private Sprite normalSpritee;
    [SerializeField] private Image powerUpTypeImage;
    [SerializeField] private Text typecount;
    public int powerUpLifetime;
    void SetUpUI()
    {
        typecount.text = "" + (int)currentType;
        if (currentType == PowerUPType.Sheild) powerUpTypeImage.sprite = sheildSprite;
        else powerUpTypeImage.sprite = normalSpritee;
    }
    int remainingTime = 0;
    IEnumerator I_PowerUpTimer(int time)
    {

        remainingTime = time;
        while (true)
        {
            remainingTime = remainingTime - 1;
            SetHealth(remainingTime);

            if (remainingTime <= 0)
            {
                if(currentType == PowerUPType.Sheild) GameManager.instance.StopsheildPower();
                else GameManager.instance.StopPowerUp();
                this.PowerUp(false);
                break;
            }

            yield return new WaitForSeconds(1);
        }


    }

    public void StartPowerUp(PowerUPType _type, int _maxHealth = 10)
    {
        currentType = _type;
        remainingTime = _maxHealth;
        PowerUp(true);
        SetMaxHealth(_maxHealth);
    }
    public void StopPowerUp()
    {
        PowerUp(false);
    }
    public void PowerUp(bool _switch)
    {
        isStart = _switch;
        if (_switch)
        {
            SetUpUI();
            if (_timerCoroutine == null) 
                _timerCoroutine = StartCoroutine(I_PowerUpTimer(powerUpLifetime));

        }
        else
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }
    }


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
