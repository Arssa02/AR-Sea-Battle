using System.Collections.Generic;
using UnityEngine;

public class OceanBoard : MonoBehaviour
{
    public GameObject cellPrefab;

    public int size = 5;

    private Cell[,] cells;

    public Player player;


void Start()
{
    InitializeBoard();
}


public void InitializeBoard()
{
    cells = new Cell[size, size];

    CreateGrid();
}


    void CreateGrid()
    {
        for(int row = 0; row < size; row++)
        {
            for(int col = 0; col < size; col++)
            {
                Vector3 position = new Vector3(
                    col,
                    0,
                    -row
                );


                GameObject cellObject =
                    Instantiate(
                        cellPrefab,
                        position,
                        Quaternion.identity,
                        transform
                    );


                string coordinate =
                    ((char)('A' + row)).ToString()
                    + (col + 1);


                Cell cell =
                    cellObject.GetComponent<Cell>();


                cell.SetCoordinate(coordinate);


                cells[row, col] = cell;
            }
        }
    }


    public Cell GetCell(string coordinate)
    {
        foreach(Cell cell in cells)
        {
            if(cell.coordinate == coordinate)
                return cell;
        }

        return null;
    }


public bool PlaceShip(string startCoordinate, Direction direction, int shipSize, string shipName)
{
    Cell startCell = GetCell(startCoordinate);

    if(startCell == null)
    {
        return false;
    }


    List<Cell> shipCells = new List<Cell>();


    int startRow = startCoordinate[0] - 'A';
    int startCol = int.Parse(startCoordinate[1].ToString()) - 1;


    for(int i = 0; i < shipSize; i++)
    {
        int row = startRow;
        int col = startCol;


        if(direction == Direction.Horizontal)
        {
            col += i;
        }
        else
        {
            row += i;
        }


        if(row >= size || col >= size)
        {
            return false;
        }


        Cell cell = cells[row, col];


        if(cell.hasShip)
        {
            return false;
        }


        shipCells.Add(cell);
    }


foreach(Cell cell in shipCells)
{
    cell.hasShip = true;
}


Ship ship = new Ship(
    shipName,
    shipSize
);


ship.positions = shipCells;


if(player != null)
{
    player.ships.Add(ship);
}


return true;
}
}