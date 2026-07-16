using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public OceanBoard player1Board;
    public OceanBoard player2Board;

    public GameState currentState;

    private int currentShipIndex = 0;

    private readonly string[] shipNames = 
    {
        "Destroyer",
        "Patrol Boat"
    };

    private readonly int[] shipSizes = 
    {
        3,
        2
    };


    void Start()
    {
        InitializeGame();
    }


    void InitializeGame()
    {
        player1 = new Player("Player 1");
        player2 = new Player("Player 2");


        player1.board = player1Board;
        player2.board = player2Board;


        player1Board.player = player1;
        player2Board.player = player2;


        player1Board.InitializeBoard();
        player2Board.InitializeBoard();


        currentState = GameState.Player1Placement;


        Debug.Log("Game started");
        Debug.Log(currentState);
    }


    public bool PlaceShipForCurrentPlayer(
    string startCoordinate,
    Direction direction,
    int shipSize,
    string shipName)
    {
    Player currentPlayer =
        currentState == GameState.Player1Placement
        ? player1
        : player2;

    return currentPlayer.board.PlaceShip(
        startCoordinate,
        direction,
        shipSize,
        shipName
    );
    }
    public int GetCurrentShipSize()
    {
        return shipSizes[currentShipIndex];
    }

    public string GetCurrentShipName()
    {
        return shipNames[currentShipIndex];
    
    }
    public void NextShip()
    {
        currentShipIndex++;
    }
}
