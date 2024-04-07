using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingItem : MonoBehaviour
{
    [SerializeField] Building building;
    public Building GetBuilding => building;
    Button myButton;
    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => BuildingCreatorManager.instance.InitCreationOfBuilding(building));
    }
}
