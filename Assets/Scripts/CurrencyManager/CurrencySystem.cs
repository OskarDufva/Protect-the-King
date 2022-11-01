using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrencySystem : MonoBehaviour
{
    public int Gold;
    private TMP_Text CurrencyIndicator;


    void Start()
    {
        //CurrencyIndicator = gameObject.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        UpdateGoldCounter();
    }

    void UpdateGoldCounter()
    {
        //CurrencyIndicator.text = "Gold:" + Gold.ToString();
        print(Gold);
    }

    // function for updating the amount of gold you have and making sure that it is not possible to have a negative amount.
    public void ChangeGold(int amount)
    {
        Gold += amount;
        if (Gold < 0)
        {
            Gold = 0;
        }

        UpdateGoldCounter();
    }

    public int GetGold()
    {
        return Gold;
    }
}
