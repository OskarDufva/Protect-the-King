//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class KingHealth : MonoBehaviour
//{
//    public King king;
//    //The UI text for the health count
//    public TextMeshProUGUI txt_HealthCount;


//    private void Start()
//    {
//        king = GetComponent<King>();
//        txt_HealthCount.GetComponentInChildren<TextMeshProUGUI>();
//    }


//    public void Init()
//    {
//        king._health = king._maxHealth;        
//        txt_HealthCount.SetText("Health: " + king._health.ToString());
//    }

//    public void LoseHealth()
//    {
//        king._health--;
//        txt_HealthCount.SetText(king._health.ToString());

//        CheckHealthCount();
//    }

//    void CheckHealthCount()
//    {
//        if(king._health<1)
//        {
//            Debug.Log("You Lost");
//        }
//    }
//}
