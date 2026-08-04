using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public string playerName;
    public OceanBoard board;
    public List<Ship> ships;
    public Player(string name) {
        playerName = name;
        ships = new List<Ship>();
    }

    public bool AllShipsSunk() {
        if (ships.Count == 0) {
            return false;
        }

        foreach (Ship ship in ships) {
            if (!ship.IsSunk()) {
                return false;
            }
        }

        return true;
    }
}