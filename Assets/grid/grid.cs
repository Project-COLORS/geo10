using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid:MonoBehaviour
{
    public GameObject _tile1; //set to floating tile object
    public Grid _grid; //set to self

    public GameObject _tileConfigs; //set to tile configs world object
    public GameObject _charactersHolder; //set to character objects

    float c_tileDimension=1.0098f; //the physical width/height of a tile
    Vector3 c_gridSpawnOffset=new Vector3(); //initialised later to half of the tile dimension,
                                             //so things spawn in the middle

    int[] c_gridDimension=new int[2]{10,10}; //width/height of the map, in number of tiles,
                                             //not physical dimension

    GameObject[,] _tiles; //the tile gameobjects instatiated, initialised
                          //to grid dimension size in start function

    tile[,] _tilesTiles; //the actual tile component of the tile objects

    Stack<tile> _selectedTiles=new Stack<tile>(); //collected stack of selected tiles

    void Start()
    {
        //set the main tile array and spawn offset
        c_gridSpawnOffset.Set(c_tileDimension/2,0,c_tileDimension/2);
        _tiles=new GameObject[c_gridDimension[0],c_gridDimension[1]];
        _tilesTiles=new tile[c_gridDimension[0],c_gridDimension[1]];

        //create tile gameobjects and put into the array
        for (int x=0;x<c_gridDimension[0];x++)
        {
            for (int y=0;y<c_gridDimension[1];y++)
            {
                _tiles[x,y]=Instantiate(_tile1,_grid.CellToWorld(new Vector3Int(x,y,0))+c_gridSpawnOffset,Quaternion.Euler(-90,0,0));
                _tilesTiles[x,y]=_tiles[x,y].GetComponent<tile>();
                _tilesTiles[x,y].initialise(x,y);
            }
        }

        //go over grid configs and apply them
        tileconfig[] gridConfigs=_tileConfigs.transform.GetComponentsInChildren<tileconfig>();
        Vector3Int gridPos;

        for (int x=0;x<gridConfigs.Length;x++)
        {
            gridPos=_grid.WorldToCell(gridConfigs[x].transform.position);

            _tilesTiles[gridPos[0],gridPos[1]].processTileConfig(gridConfigs[x].GetComponent<tileconfig>());
        }

        //go over characters and place them in their nearest tiles
        character[] characters=_charactersHolder.transform.GetComponentsInChildren<character>();

        for (int x=0;x<characters.Length;x++)
        {
            //get the grid position the character should be in
            gridPos=_grid.WorldToCell(characters[x].transform.position);

            relocateChar(characters[x],_tilesTiles[gridPos[0],gridPos[1]]);
        }

        _tileConfigs.SetActive(false);
    }

    //begin a movecalc tile selection with the given position and spaces
    //give it selectCharacterTiles and it will select tiles with characters in them
    public void moveCalc(Vector3 position,int spaces,bool selectCharacterTiles=false)
    {
        Vector3Int pos=_grid.WorldToCell(position);

        moveCalc2(pos[0],pos[1],spaces,selectCharacterTiles);
    }

    //2nd part of move calc
    void moveCalc2(int xpos,int ypos,int spaces,bool selectCharacterTiles)
    {
        //if out of move spaces or out of range
        if (spaces<0 || xpos<0 || ypos<0 || ypos>=c_gridDimension[1] || xpos>=c_gridDimension[0])
        {
            return;
        }

        if (_tilesTiles[xpos,ypos].isObstruction)
        {
            return;
        }

        spaces--;

        if (!_tilesTiles[xpos,ypos].cannotBeSelected && (_tilesTiles[xpos,ypos].currentCharacter==null || selectCharacterTiles))
        {
            _tilesTiles[xpos,ypos].markSelected();
        }

        _selectedTiles.Push(_tilesTiles[xpos,ypos]);

        moveCalc2(xpos+1,ypos,spaces,selectCharacterTiles);
        moveCalc2(xpos-1,ypos,spaces,selectCharacterTiles);
        moveCalc2(xpos,ypos+1,spaces,selectCharacterTiles);
        moveCalc2(xpos,ypos-1,spaces,selectCharacterTiles);
    }

    //snap a character to a tile on the grid
    public void relocateChar(character thecharacter,tile thetile)
    {
        thetile.currentCharacter=thecharacter.gameObject;

        if (thecharacter.currentTile!=null)
        {
            thecharacter.currentTile.currentCharacter=null;
        }

        thecharacter.currentTile=thetile;

        Vector3 charPos=thetile.transform.position;
        charPos.y=thecharacter.stats.charHeight+thetile.tileHeight;
        thecharacter.transform.position=charPos;
    }

    //clear all selected tiles
    public void clearSelectedTiles()
    {
        tile currentTile;
        while (_selectedTiles.Count>0)
        {
            currentTile=_selectedTiles.Pop();

            currentTile.unselect();
        }
    }

    public tile getTile(int xpos,int ypos)
    {
        return _tilesTiles[xpos,ypos];
    }
}
