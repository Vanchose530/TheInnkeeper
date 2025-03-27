using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meal", menuName = "ScriptableObjects/Dish/Meal", order = 1)]
public class Meal : Dish
{
    [Header("Meal")]

    [SerializeField] private int satietyLevel;

    public int GetSatietyLevel() { return satietyLevel; }
}
