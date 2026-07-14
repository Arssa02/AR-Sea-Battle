using UnityEngine;

public class Cell : MonoBehaviour
{
    public string coordinate;

    public bool hasShip = false;
    public bool alreadyShot = false;
    public bool isHit = false;

    public void SetCoordinate(string coord)
    {
        coordinate = coord;
        gameObject.name = "Cell_" + coordinate;
    }
}