using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerResources : MonoBehaviour
{
    ResourceTypes.Resources resourceTypes;
    public int Wood { get; private set; }
    public int Stone { get; private set; }
    public int Iron { get; private set; }
    public int Gold { get; private set; }


    public int woodMax;
    public int stoneMax;
    public int ironMax;
    public int goldMax;

    public GameObject resourcepanel;

    public void Start()
    {
        Wood = 0;
        Stone = 0;
        Iron = 0;
        Gold = 0;
        UpdateResources();
    }

    public void Update()
    {
        if (Wood > woodMax)
        {
            Wood = woodMax;
        }
        if (Stone > stoneMax)
        {
            Stone = stoneMax;
        }
        if (Iron > ironMax)
        {
            Iron = ironMax;
        }
        if (Gold > goldMax)
        {
            Gold = goldMax;
        }
    }

    public void ModifyResource(ResourceTypes.Resources resource, int amount)
    {
        switch (resource)
        {
            case ResourceTypes.Resources.Wood:
                Wood += amount;
                break;
            case ResourceTypes.Resources.Stone:
                Stone += amount;
                break;
            case ResourceTypes.Resources.Food:
                Iron += amount;
                break;
            case ResourceTypes.Resources.Gold:
                Gold += amount;
                break;
        }
        UpdateResources();
    }
    public void UpdateResources()
    {
        resourcepanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Wood: " + Wood + "/" + woodMax;
        resourcepanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Stone: " + Stone + "/" + stoneMax;
        resourcepanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Iron: " + Iron + "/" + ironMax;
        resourcepanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Gold: " + Gold + "/" + goldMax;
    }
    
}
