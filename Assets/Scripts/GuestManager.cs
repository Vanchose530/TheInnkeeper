using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestManager : MonoBehaviour
{
    public static GuestManager instance { get; private set; }

    [Header("Guests")]
    public List<GameObject> guestPrefabs;

    [Header("Spawn Interval")]
    public float minSpawnInterval;
    public float maxSpawnInterval;
    float spawnInterval;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find more than one Guest Manager in scene");
        }
        instance = this;

        CountSpawnInterval();
    }

    private void Update()
    {
        if (InnManager.instance.innClean)
        {
            if (spawnInterval <= 0) SpawnGuest();
            CountSpawnInterval();
        }
    }

    private void SpawnGuest()
    {
        CountSpawnInterval();
        try
        {
            int index = Random.Range(0, guestPrefabs.Count);
            Instantiate(guestPrefabs[index]);
            guestPrefabs[index].transform.position = Entrance.instance.transform.position;
            FrontDoor.instance.OpenDoor();
            guestPrefabs.RemoveAt(index);
        }
        catch
        {

        }
 
    }

    private void CountSpawnInterval()
    {
        if(spawnInterval <= 0) spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        spawnInterval -= Time.deltaTime;
    }
}
