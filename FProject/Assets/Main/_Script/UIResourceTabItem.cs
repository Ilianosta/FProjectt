using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResourceTabItem : MonoBehaviour
{
    public TMP_Text resName;
    public TMP_Text resAmount;

    [HideInInspector] public Building.Resources resource;
}
