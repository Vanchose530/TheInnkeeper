using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnManager : MonoBehaviour
{
    public static InnManager instance { get; private set; }

    public List<SeatingPosition> allSeatingPositions { get; private set; }
    public List<SeatingPosition> availableSeatingPositions { get; private set; }

    public int allFurnitureCount { get; private set; }
    public int droppedFurnitureCount { get; private set; }
    public bool innClean { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find more than one Inn Manager in scene");
        }

        allSeatingPositions = new List<SeatingPosition>();
        availableSeatingPositions = new List<SeatingPosition>();

        instance = this;

        allFurnitureCount = 0;
        droppedFurnitureCount = 0;
    }

    

    private void Update()
    {
        CheckAvailableSeatingPositions();
        innClean = (droppedFurnitureCount == 0);
    }

    private void CheckAvailableSeatingPositions()
    {
        foreach(var position in allSeatingPositions)
        {
            if (!availableSeatingPositions.Contains(position) && (!position.chosenByGuest && !position.occupied)) availableSeatingPositions.Add(position);
            else if (availableSeatingPositions.Contains(position) && (position.chosenByGuest || position.occupied)) availableSeatingPositions.Remove(position);
        }
    }

    public void CountNewFurniture()
    {
        allFurnitureCount++;
    }

    public void CountNewDroppedFurniture()
    {
        droppedFurnitureCount++;
    }

    public void CountNewUndroppedFurniture()
    {
        droppedFurnitureCount--;
    }

    public void AddSeatingPosition(SeatingPosition seatingPosition)
    {
        allSeatingPositions.Add(seatingPosition);
    }

    public void RemoveSeatingPosition(SeatingPosition seatingPosition)
    {
        allSeatingPositions.Remove(seatingPosition);
    }
}
