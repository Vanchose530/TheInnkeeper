using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : ScriptableObject
{
    [Header("General")]

    [SerializeField] private Sprite image;

    [SerializeField] private string name;

    [TextArea(3, 10)]
    [SerializeField] private string description;

    [SerializeField] private DishQuality quality;

    public Sprite GetImage() { return image; }

    public string GetName() { return name; }

    public string GetDescription() { return description; }

    public DishQuality GetDishQuality() { return quality; }

    public string GetStringDishQuality()
    {
        string result = "";

        switch (quality)
        {
            case DishQuality.LOUSY:
                result = "паршивое";
                break;
            case DishQuality.NOT_FISH_NOT_MEAT:
                result = "не рыба не мясо";
                break;
            case DishQuality.FINE:
                result = "ничего так";
                break;
            case DishQuality.TOP_CLASS:
                result = "высший класс";
                break;
            case DishQuality.DIVINE:
                result = "божественно";
                break;
        }

        return result;
    }

}
