using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] public float damagedHealthFadeTime;
    [SerializeField] private Image barImage;
    [SerializeField] private Image damagedBarImage;
    private Color damagedColor;
    private float damagedHealthFadeTimer;

    private void Awake()
    {
        damagedColor = damagedBarImage.color;
        damagedColor.a = 0f;
        damagedBarImage.color = damagedColor;
    }
    private void Update()
    {
        if (damagedColor.a > 0)
        {
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer <= 0)
            {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = damagedColor;
            }
        }
    }

    public void UpdateHealthBar(float curHealth, float maxHealth)
    {
        if (damagedColor.a <= 0)
        {
            damagedBarImage.fillAmount = barImage.fillAmount;
        }
        damagedColor.a = 1;
        damagedBarImage.color = damagedColor;
        damagedHealthFadeTimer = damagedHealthFadeTime;

        float healthNormalized = curHealth / maxHealth;
        barImage.fillAmount = healthNormalized;
    }
}
