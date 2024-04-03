using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UIResourceTabItem resourceTabItemObj;
    public Transform resourceParent;
    List<UIResourceTabItem> resourceTabItems = new List<UIResourceTabItem>();

    private void Awake()
    {
        if (UIManager.instance) Destroy(gameObject);
        else UIManager.instance = this;
    }

    public void SetResource(BuildingConstruction.Resources resource, string resourceAmount)
    {
        UIResourceTabItem tabItem = resourceTabItems.Find(x => x.resource == resource);
        if (tabItem)
        {
            tabItem.resName.text = resource.ToString();
            tabItem.resAmount.text = resourceAmount;
        }
        else
        {
            UIResourceTabItem newItem = Instantiate(resourceTabItemObj, resourceParent).GetComponent<UIResourceTabItem>();
            newItem.resource = resource;
            newItem.resName.text = resource.ToString();
            newItem.resAmount.text = resourceAmount;
            resourceTabItems.Add(newItem);
        }
    }
}
