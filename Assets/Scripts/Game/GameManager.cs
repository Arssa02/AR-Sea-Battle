using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public OceanBoard player1Board;
    public OceanBoard player2Board;

    public GameState currentState;


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


        PlaceInitialShips();


        currentState = GameState.Player1Placement;


        Debug.Log("Game started");
        Debug.Log(currentState);
    }


    void PlaceInitialShips()
    {
        player1.board.PlaceShip(
            "A1",
            Direction.Horizontal,
            3
        );

        player1.board.PlaceShip(
            "D4",
            Direction.Horizontal,
            2
        );


        player2.board.PlaceShip(
            "B2",
            Direction.Horizontal,
            3
        );

        player2.board.PlaceShip(
            "E1",
            Direction.Horizontal,
            2
        );
    }
}