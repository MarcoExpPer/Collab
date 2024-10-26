using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public enum RewardType
{
    Fixed,
    RandomRange,
    Probabilistic
}

public enum RewardObject
{
    Seed, 
    Crystal, 
    Bomb, 
    Key, 
    HealthPack,
    KeyDoor
}

public enum RewardProbability
{
    Low,
    Medium,
    High
}

public class SpawnReward : MonoBehaviour
{
    [SerializeField] private RewardItems rewardItems;
    [SerializeField] private RewardType rewardType;
    [SerializeField] private RewardProbability rewardProbability;
    [SerializeField] private RewardObject objectReward;
    [SerializeField] private int maxRange;
    private int numOfRewards;

    private int distance;

    private void Start()
    {
        switch (rewardProbability)
        {
            case RewardProbability.Low:
                distance = 10;
                break;
            case RewardProbability.Medium:
                distance = 25;
                break;
            case RewardProbability.High:
                distance = 50;
                break;
        }
    }

    public void Spawn()
    {
        int numSpawns = SpawnableObjects();
        for(int i = 0; i < numSpawns; i++)
        {
            GameObject rewardObject = objectToSpawn();

            if (rewardObject != null)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
                Instantiate(rewardObject, spawnPosition, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    
    }

    private GameObject objectToSpawn()
    {
        switch (objectReward)
        {
            case RewardObject.Seed:
                return rewardItems.objetosReward[0];
            case RewardObject.Crystal:
                return rewardItems.objetosReward[1];
            case RewardObject.Bomb:
                return rewardItems.objetosReward[2];
            case RewardObject.Key:
                return rewardItems.objetosReward[3];
            case RewardObject.HealthPack:
                return rewardItems.objetosReward[4];
            case RewardObject.KeyDoor:
                return rewardItems.objetosReward[5];
            default:
                return null;
        }
    
    }

    private int SpawnableObjects()
    {
        numOfRewards = 0;
        switch (rewardType)
        {
            case RewardType.Fixed:
                numOfRewards = 1;
                return numOfRewards;
            case RewardType.RandomRange:
                for (int i = 0; i < maxRange; i++)
                {
                    if(RandomSpawn())
                    {
                        numOfRewards++;
                    }
                }
                return numOfRewards;
            case RewardType.Probabilistic:
                if(RandomSpawn())
                {
                    numOfRewards = 1;
                }
                else
                {
                    numOfRewards = 0;
                }
                return numOfRewards;
            default:
                return 0;
        }
    }

    private bool RandomSpawn()
    {
        int randomNum = Random.Range(0, 100);
        return randomNum <= distance;
    }
}
