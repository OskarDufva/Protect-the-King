using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingHealth : MonoBehaviour
{
    public Image HealthBar;
    public King king;


    public void UpdateHealthBar()
    {
        float fillAmount = Mathf.Clamp(king._health / king._maxHealth, 0, 1f);
        HealthBar.fillAmount = fillAmount;
    }
}
