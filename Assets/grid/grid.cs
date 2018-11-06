using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid:MonoBehaviour
{
    public GameObject _tile1; //set to floating tile object
    public Grid _grid; //set to self

    public GameObject _tileConfigs; //set to tile configs world object

    float c_tileDimension=1.0098f; //the physical width/height of a tile
    Vector3 c_gridSpawnOffset=new Vector3(); //initialised later to half of the tile dimension,
                                             //so things spawn in the middle

    int[] c_gridDimension=new int[2]{10,10}; //width/height of the map, in number of tiles,
                                             //not physical dimension

    GameObject[,] _tiles; //the tile gameobjects instatiated, initialised
                          //to grid dimension size in start function

    void Start()
    {
        c_gridSpawnOffset.Set(c_tileDimension/2,0,c_tileDimension/2);
        _tiles=new GameObject[c_gridDimension[0],c_gridDimension[1]];

        for (int x=0;x<c_gridDimension[0];x++)
        {
            for (int y=0;y<c_gridDimension[1];y++)
            {
                _tiles[x,y]=Instantiate(_tile1,_grid.CellToWorld(new Vector3Int(x,y,0))+c_gridSpawnOffset,Quaternion.Euler(-90,0,0));
            }
        }

        Transform[] gridConfigs=_tileConfigs.transform.GetComponentsInChildren<Transform>();
        Vector3Int gridPos=new Vector3Int();

        for (int x=1;x<gridConfigs.Length;x++)
        {
            gridPos=_grid.WorldToCell(gridConfigs[x].transform.position);

            _tiles[gridPos[0],gridPos[1]].GetComponent<tile>().setColour(gridConfigs[x].GetComponent<tileconfig>().colour);

        }

        _tileConfigs.SetActive(false);
    }
}
