using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GridElement currentSelectedGridElement;
    public GridElement currentHoveredGridElement;
    public GridElement[] grid;
    //public LayerMask gridElementLayer;
    public GameObject gridParent;
    public GridElement referenceGridElement;
    public Buildings buildings;
    [Header("Colors")]
    public Color colorOnHover = Color.white;
    public Color colorOnOccupied = Color.red;

    private GameObject currentCreatedBuildable;
    private bool buildInProgress;
    private RaycastHit mouseHit;
    private Color colorOnNormal;

    private void Awake()
    {
        colorOnNormal = grid[0].GetComponentInChildren<MeshRenderer>().material.color;
        //colorOnNormal = referenceGridElement.GetComponentInChildren<MeshRenderer>().material.color;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out mouseHit))
        {
            GridElement gE = mouseHit.transform.gameObject.GetComponent<GridElement>();

            if(gE == null)
            {
                if(currentHoveredGridElement)
                {
                    currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = colorOnNormal;
                    return;
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                currentSelectedGridElement = gE;
            }

            if(gE != currentHoveredGridElement)
            {
                if(!gE.occupied)
                {
                    mouseHit.transform.gameObject.GetComponent<MeshRenderer>().material.color = colorOnHover;
                }
                else
                {
                    mouseHit.transform.gameObject.GetComponent<MeshRenderer>().material.color = colorOnOccupied;
                }
            }

            if( currentHoveredGridElement && currentHoveredGridElement != gE)
            {
                currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = colorOnNormal;
            }

            currentHoveredGridElement = gE;
        }
        else
        {
            currentHoveredGridElement.GetComponent<MeshRenderer>().material.color = colorOnNormal;
        }

        MoveBuilding();
        PlaceBuilding();
    }

    public void OnButtonCreateBuilding(int id)
    {
        if(buildInProgress)
        {
            return;
        }

        GameObject g = null;
        foreach (GameObject gO in buildings.buildable)
        {
            Building b = gO.GetComponent<Building>();
            if(b.information.id == id)
            {
                g = b.gameObject;
            }
        }

        currentCreatedBuildable = Instantiate(g);
        currentCreatedBuildable.transform.rotation = Quaternion.Euler(0f, transform.rotation.y - 225f, 0f);
        buildInProgress = true;
    }

    public void MoveBuilding()
    {
        if(!currentCreatedBuildable)
        {
            return;
        }

        currentCreatedBuildable.gameObject.layer = 2;
        if(currentHoveredGridElement)
        {
            currentCreatedBuildable.transform.position = currentHoveredGridElement.transform.position;
        }

        if(Input.GetMouseButtonDown(1))
        {
            Destroy(currentCreatedBuildable);
            currentCreatedBuildable = null;
            buildInProgress = false;
        }

        if(Input.GetMouseButton(2))
        {
            currentCreatedBuildable.transform.Rotate(transform.up * 5);
        }
    }

    public void PlaceBuilding()
    {
        if(!currentCreatedBuildable || currentHoveredGridElement.occupied)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            buildings.builtObjects.Add(currentCreatedBuildable);
            currentHoveredGridElement.occupied = true;

            Building b = currentCreatedBuildable.GetComponent<Building>();
            currentHoveredGridElement.connectedBuilding = b;

            b.UpgradeBuilding();
            b.placed = true;
            b.information.connectedGridID = currentHoveredGridElement.gridID;
            b.information.yRotation = b.transform.localEulerAngles.y;

            currentCreatedBuildable = null;
            buildInProgress = false;
        }

        
    }
}
