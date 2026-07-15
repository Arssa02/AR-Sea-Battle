using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public string playerName;

    public OceanBoard board;

    public List<Ship> ships;


    public Player(string name)
    {
        playerName = name;
        ships = new List<Ship>();
    }
}