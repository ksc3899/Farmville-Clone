using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public int connectedBuildingID;
    [HideInInspector] public Building connectedBuilding;
    public Text resourcesText;

    private Button button;
    private Resources resources;

    private void Awake()
    {
        button = GetComponent<Button>();
        resources = FindObjectOfType<Resources>();

        Buildings buildings = FindObjectOfType<Buildings>();
        foreach(GameObject gO in buildings.buildable)
        {
            if(gO.GetComponent<Building>().information.id == connectedBuildingID)
            {
                connectedBuilding = gO.GetComponent<Building>();
                break;
            }
        }

        resourcesText.text = connectedBuilding.price.woodPrice + " Wo. | " + connectedBuilding.price.stonePrice + " St. | " +
                                                                             connectedBuilding.price.foodPrice + " Fo.";
    }

    private void Update()
    {
        bool result = false;

        if(resources.wood>=connectedBuilding.price.woodPrice && resources.stones>=connectedBuilding.price.stonePrice
            && resources.food>=connectedBuilding.price.foodPrice)
        {
            result = true;
        }

        button.interactable = result;
    }
}
