using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {

    public int gridID;
    public bool occupied;
    public Building connectedBuilding;

    private void Awake()
    {
        Build b = FindObjectOfType<Build>();
        for(int i=0; i<b.grid.Length; i++)
        {
            if(b.grid[i].transform == this.transform)
            {
                gridID = i;
                break;
            }
        }
    }
}
