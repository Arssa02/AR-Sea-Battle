using System.Collections.Generic;
using UnityEngine;

public class OceanBoard : MonoBehaviour
{
    public GameObject cellPrefab;

    public float cellSpacing = 0.1f;

    public int size = 5;

    private Cell[,] cells;

    public Player player;

    private bool boardCreated = false;


    public void InitializeBoard()
    {
        if (boardCreated)
            return;

        cells = new Cell[size, size];

        CreateGrid();

        boardCreated = true;
    }


    void CreateGrid()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                float offset = (size - 1) * cellSpacing / 2f;

                Vector3 position = new Vector3(
                    col * cellSpacing - offset,
                    0,
                    -row * cellSpacing + offset

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
        if (cells == null)
        {
            return null;
        }

        foreach (Cell cell in cells)
        {
            if (cell.coordinate == coordinate)
            {
                return cell;
            }
        }

        return null;
    }


    public bool PlaceShip(string startCoordinate, Direction direction, int shipSize, string shipName)
    {
        Cell startCell = GetCell(startCoordinate);

        if (startCell == null)
        {
            return false;
        }


        List<Cell> shipCells = new List<Cell>();


        int startRow = startCoordinate[0] - 'A';
        int startCol = int.Parse(startCoordinate[1].ToString()) - 1;


        for (int i = 0; i < shipSize; i++)
        {
            int row = startRow;
            int col = startCol;


            if (direction == Direction.Horizontal)
            {
                col += i;
            }
            else
            {
                row += i;
            }


            if (row >= size || col >= size)
            {
                return false;
            }


            Cell cell = cells[row, col];


            if (cell.hasShip)
            {
                return false;
            }


            shipCells.Add(cell);
        }


        foreach (Cell cell in shipCells)
        {
            cell.SetShip();
        }


        Ship ship = new Ship(
            shipName,
            shipSize
        );


        ship.positions = shipCells;


        if (player != null)
        {
            player.ships.Add(ship);
        }

        return true;
    }

    public ShotResult Shoot(string coordinate)
    {
        Cell cell = GetCell(coordinate);

        if (cell == null)
        {
            return ShotResult.Invalid;
        }

        if (cell.alreadyShot)
        {
            return ShotResult.AlreadyShot;
        }

        cell.alreadyShot = true;

        if (cell.hasShip)
        {
            cell.isHit = true;
            cell.SetHit();

            DamageShipAtCell(cell);

            return ShotResult.Hit;
        }

        cell.SetMiss();

        return ShotResult.Miss;
    }

    private void DamageShipAtCell(Cell hitCell)
    {
        if (player == null)
        {
            return;
        }

        foreach (Ship ship in player.ships)
        {
            if (ship.positions.Contains(hitCell))
            {
                ship.health--;

                if (ship.IsSunk())
                {
                    Debug.Log(ship.shipName + " was sunk.");
                }

                return;
            }
        }
    }



    public bool IsInitialized
    {
        get
        {
            return boardCreated && cells != null;
        }
    }

}