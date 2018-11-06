using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid:MonoBehaviour
{
    public GameObject r_tile1; //set to floating tile object
    public Grid r_grid; //set to self

    float c_tileDimension=1.0098f; //the physical width/height of a tile
    Vector3 c_gridSpawnOffset=new Vector3(); //initialised later to half of the tile dimension,
                                             //so things spawn in the middle

    int[] c_gridDimension=new int[2]{10,10}; //width/height of the map, in number of tiles,
                                             //not physical dimension

    void Start()
    {
        c_gridSpawnOffset.Set(c_tileDimension/2,0,c_tileDimension/2);

        for (int x=0;x<c_gridDimension[0];x++)
        {
            for (int y=0;y<c_gridDimension[1];y++)
            {
                Instantiate(r_tile1,r_grid.CellToWorld(new Vector3Int(x,y,0))+c_gridSpawnOffset,Quaternion.Euler(-90,0,0));
            }
        }
    }
}
