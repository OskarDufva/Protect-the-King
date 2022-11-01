using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingHealth : MonoBehaviour
{
    public Image KingHealthBarImage;
    public King king;
    

    public void UpdateHealthBar()
    {
        KingHealthBarImage.fillAmount = Mathf.Clamp(king._health / king._maxHealth, 0, 1f);
    }
}
