using UnityEngine;

public class OceanBoard : MonoBehaviour
{
    public GameObject cellPrefab;

    public int size = 5;

    void Start()
    {
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
                    Instantiate(cellPrefab, position, Quaternion.identity, transform);


                string letter = ((char)('A' + row)).ToString();
                string number = (col + 1).ToString();

                cellObject.GetComponent<Cell>()
                    .SetCoordinate(letter + number);
            }
        }
    }
}