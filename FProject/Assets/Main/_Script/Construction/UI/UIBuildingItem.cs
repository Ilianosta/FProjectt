using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingItem : MonoBehaviour
{
    [SerializeField] private Image overlappingImg;
    [SerializeField] Building building;
    public Building GetBuilding => building;
    Button myButton;
    private bool CanBuy() => GameManager.instance.GetResource(building.Cost.resource).amount >= building.Cost.amount;
    private void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(HandleClick);
    }
    private void Update()
    {
        if (!CanBuy()) overlappingImg.color = BuildingCreatorManager.instance.cantBuyColor;
        else overlappingImg.color = Color.clear;
    }
    private void HandleClick()
    {
        if (CanBuy())
        {
            BuildingCreatorManager.instance.InitCreationOfBuilding(building);
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(overlappingImg.DOColor(BuildingCreatorManager.instance.flickBuyColor, 0.35f));
            sequence.Append(overlappingImg.DOColor(BuildingCreatorManager.instance.cantBuyColor, 0.35f));
            sequence.Append(overlappingImg.DOColor(BuildingCreatorManager.instance.flickBuyColor, 0.35f));
            sequence.Append(overlappingImg.DOColor(BuildingCreatorManager.instance.cantBuyColor, 0.35f));
        }
    }
}
