using UnityEngine;

public class Cell : MonoBehaviour
{
    public string coordinate;

    public bool hasShip = false;
    public bool alreadyHit = false;

    public void SetCoordinate(string coord)
    {
        coordinate = coord;
        gameObject.name = "Cell_" + coordinate;
    }
}