using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerResources : MonoBehaviour
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Iron,
        Gold
    }
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

    public void ModifyResource(ResourceType type, int amount, bool add)
    {
        switch (type)
        {
            case ResourceType.Wood:
               Wood = add ? Wood + amount : Wood - amount;
                break;
            case ResourceType.Stone:
                Stone = add ? Stone + amount : Stone - amount;
                break;
            case ResourceType.Iron:
                Iron = add ? Iron + amount : Iron - amount;
                break;
            case ResourceType.Gold:
                Gold = add ? Gold + amount : Gold - amount;
                break;
            default:
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
