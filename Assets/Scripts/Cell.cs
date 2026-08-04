using UnityEngine;

public class Cell : MonoBehaviour
{
    public string coordinate;
    public bool hasShip = false;
    public bool alreadyShot = false;
    public bool isHit = false;
    public Renderer cellRenderer;
    private Color waterColor;
    private void Awake() {
        cellRenderer = GetComponent<Renderer>();
        waterColor = cellRenderer.material.color;
    }

    public void SetCoordinate(string coord) {
        coordinate = coord;
        gameObject.name = "Cell_" + coordinate;
    }

    public void SetShip() {
        hasShip = true;

        cellRenderer.material.color = Color.green;
    }

    public void SetHit() {
        isHit = true;
        cellRenderer.material.color = Color.red;
    }

    public void SetMiss() {
        cellRenderer.material.color = Color.gray;
    }

    public void ShowShip() {
        if (hasShip) {
            cellRenderer.material.color = Color.green;
        }
    }

    public void HideShip() {
        if (hasShip && !isHit) {
            cellRenderer.material.color = waterColor;
        }
    }

}