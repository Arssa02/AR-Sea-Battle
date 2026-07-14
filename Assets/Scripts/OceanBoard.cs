using UnityEngine;

public class OceanBoard : MonoBehaviour
{
    public GameObject cellPrefab;

    public int size = 5;

    private Cell[,] cells;


    void Start()
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
}