using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;
    [SerializeField] private int CurrentMoney;
    public const string PrefMoney = "PrefMoney";
   


    private void Awake()
    {
        Instance=this;
        //In Awake, get the value of the prefab money
        CurrentMoney=PlayerPrefs.GetInt(PrefMoney);
    }
    public void AddMoney(int moneyToAdd)
    {
        CurrentMoney+=moneyToAdd;
    }

    public void saveMoney()
    {   
        //Save the money in a prefab called "money"
        PlayerPrefs.SetInt(PrefMoney, CurrentMoney);
    }

 

}
