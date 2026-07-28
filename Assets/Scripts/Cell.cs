using UnityEngine;

public class Cell : MonoBehaviour
{
    public string coordinate;

    public bool hasShip = false;
    public bool alreadyShot = false;
    public bool isHit = false;
    public Renderer cellRenderer;

    void Awake()
    {
        cellRenderer = GetComponent<Renderer>();
    }

    public void SetCoordinate(string coord)
    {
        coordinate = coord;
        gameObject.name = "Cell_" + coordinate;
    }

    public void SetShip()
    {
        hasShip = true;

        cellRenderer.material.color = Color.green;
    }

    public void SetHit()
    {
        isHit = true;
        cellRenderer.material.color = Color.red;
    }

    public void SetMiss()
    {
        cellRenderer.material.color = Color.gray;
    }

}