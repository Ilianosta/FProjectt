using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<PlayerResources> playerResources = new List<PlayerResources>();
    private List<Building> myBuildings = new List<Building>();
    public void AddBuilding(Building building) => myBuildings.Add(building);
    public PlayerResources GetResource(Building.Resources resource) => playerResources.Find(x => x.resource == resource);
    private void Awake()
    {
        if (GameManager.instance) Destroy(gameObject);
        else GameManager.instance = this;
    }

    public void SetResource(Building.Resources resource, float resourceAmount)
    {
        PlayerResources playerResource = playerResources.Find(x => x.resource == resource);
        playerResource.amount += resourceAmount;
        UIManager.instance.SetResource(resource, playerResource.amount.ToString());
    }

    // UTILITY
    [ContextMenu("Generate base resources")]
    private void GenerateResourcesBase()
    {
        foreach (Building.Resources value in Enum.GetValues(typeof(Building.Resources)))
        {
            playerResources.Add(new PlayerResources(value, 0));
        }
    }
}
[System.Serializable]
public class PlayerResources
{
    public PlayerResources(Building.Resources resource, float amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
    public Building.Resources resource;
    public float amount;
}