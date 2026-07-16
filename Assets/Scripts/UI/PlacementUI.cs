using TMPro;
using UnityEngine;

public class PlacementUI : MonoBehaviour
{
    public TMP_Dropdown letterDropdown;
    public TMP_Dropdown numberDropdown;
    public TMP_Dropdown directionDropdown;

    public GameManager gameManager;

    public void PlaceShip()
    {
        string coordinate =
            letterDropdown.options[letterDropdown.value].text +
            numberDropdown.options[numberDropdown.value].text;

        Direction direction =
            directionDropdown.value == 0
            ? Direction.Horizontal
            : Direction.Vertical;


        bool placed =
            gameManager.PlaceShipForCurrentPlayer(
                coordinate,
                direction,
                gameManager.GetCurrentShipSize(),
                gameManager.GetCurrentShipName()
            );


        if(placed)
        {
            gameManager.NextShip();
        }


        Debug.Log("Ship placed: " + placed);
    }
}