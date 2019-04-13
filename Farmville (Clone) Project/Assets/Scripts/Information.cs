using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    public Text nameText;
    public Button destroyButton;
    public Button upgradeButton;

    private Build build;
    private Resources resources;
    private Building selectedBuilding;

    private void Awake()
    {
        build = FindObjectOfType<Build>();
        resources = FindObjectOfType<Resources>();
    }

    private void Update()
    {
        if (build.currentSelectedGridElement != null && build.currentSelectedGridElement.connectedBuilding != null)
        {
            selectedBuilding = build.currentSelectedGridElement.connectedBuilding;
            if (resources.wood >= selectedBuilding.price.woodPrice
                && resources.stones >= selectedBuilding.price.stonePrice
                && resources.food >= selectedBuilding.price.foodPrice)
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }

            nameText.text = selectedBuilding.objectName + "\nLevel: " + selectedBuilding.information.level;
        }
        else
        {
            nameText.text = "No Building Selected";
            selectedBuilding = null;
            upgradeButton.interactable = false;
            destroyButton.interactable = false;
        }

        destroyButton.interactable = selectedBuilding;
    }

    public void OnButtonUpgrade()
    {
        if (selectedBuilding)
        {
            selectedBuilding.UpgradeBuilding();
        }
    }

    public void OnButtonDestroy()
    {
        if(selectedBuilding)
        {
            build.currentSelectedGridElement.occupied = false;
            build.buildings.builtObjects.Remove(selectedBuilding.gameObject);
            Destroy(selectedBuilding.gameObject);
        }
    }
}
