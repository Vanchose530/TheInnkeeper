using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "ScriptableObjects/Dish/Drink", order = 1)]
public class Drink : Dish
{
    [Header("Drink")]

    [SerializeField] private int alchoholLevel;

    public int GetAlchoholLevel() { return alchoholLevel; }
}
