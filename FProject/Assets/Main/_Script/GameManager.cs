using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<PlayerResources> playerResources = new List<PlayerResources>();
    private void Awake()
    {
        if (GameManager.instance) Destroy(gameObject);
        else GameManager.instance = this;
    }

    public void SetResource(BuildingConstruction.Resources resource, float resourceAmount)
    {
        PlayerResources playerResource = playerResources.Find(x => x.resource == resource);
        playerResource.amount += resourceAmount;
        UIManager.instance.SetResource(resource, playerResource.amount.ToString());
    }

    // UTILITY
    [ContextMenu("Generate base resources")]
    private void GenerateResourcesBase()
    {
        foreach (BuildingConstruction.Resources value in Enum.GetValues(typeof(BuildingConstruction.Resources)))
        {
            playerResources.Add(new PlayerResources(value, 0));
        }
    }
}
[System.Serializable]
public class PlayerResources
{
    public PlayerResources(BuildingConstruction.Resources resource, float amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
    public BuildingConstruction.Resources resource;
    public float amount;
}