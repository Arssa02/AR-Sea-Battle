using System.Collections.Generic;

[System.Serializable]
public class Ship
{
    public string shipName;

    public int size;

    public List<Cell> positions;

    public int health;


    public Ship(string name, int shipSize)
    {
        shipName = name;

        size = shipSize;

        health = shipSize;

        positions = new List<Cell>();
    }


    public bool IsSunk()
    {
        return health <= 0;
    }
}