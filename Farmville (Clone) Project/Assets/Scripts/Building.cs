using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PriceTag
{
    public float woodPrice;
    public float stonePrice;
    public float foodPrice;
}

[System.Serializable]
public class BuildingInformation
{
    public int id;
    public float level = 0;
    public float yRotation = 0;
    public int connectedGridID;
}

public class Building : MonoBehaviour
{
    public BuildingInformation information;
    public PriceTag price;

    public string objectName;
    public bool placed;
    public int baseResourceGain = 1;

    private Resources resources;

    private void Awake()
    {
        resources = FindObjectOfType<Resources>();
    }

    private void Update()
    {
        if (placed)
        {
            switch (information.id)
            {
                case 1:
                    resources.wood += baseResourceGain * information.level * Time.deltaTime;
                    break;
                case 2:
                    resources.stones += baseResourceGain * information.level * Time.deltaTime;
                    break;
                case 3:
                    resources.food += baseResourceGain * information.level * Time.deltaTime;
                    break;
            }
        }
    }

    public void UpgradeBuilding()
    {
        information.level++;

        resources.wood -= price.woodPrice;
        resources.stones -= price.stonePrice;
        resources.food -= price.foodPrice;
    }
}
