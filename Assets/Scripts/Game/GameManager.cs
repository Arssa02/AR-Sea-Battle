using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public OceanBoard player1Board;
    public OceanBoard player2Board;

    public GameState currentState;

    public GameUI gameUI;
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
        Player currentPlayer;

        if (currentState == GameState.Player1Placement)
        {
            currentPlayer = player1;
        }
        else if (currentState == GameState.Player2Placement)
        {
            currentPlayer = player2;
        }
        else
        {
            Debug.LogWarning(
                "Ships cannot be placed during state: " + currentState
            );

            return false;
        }

        if (currentPlayer == null || currentPlayer.board == null)
        {
            Debug.LogError("Current player or board is not configured.");
            return false;
        }

        if (!currentPlayer.board.IsInitialized)
        {
            Debug.LogWarning(
                currentPlayer.playerName +
                "'s marker must be scanned before placing a ship."
            );

            return false;
        }

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

        if (currentShipIndex < shipSizes.Length)
        {
            return;
        }

        currentShipIndex = 0;

        if (currentState == GameState.Player1Placement)
        {
            player1Board.HideShips();

            currentState = GameState.PassToPlayer2;

            gameUI.ShowPassPhonePanel(
                "Player 1 has finished placing ships.\n\n" +
                "Pass the phone to Player 2.\n\n" +
                "When Player 2 is ready, press Continue."
            );

            Debug.Log("Waiting to pass the phone to Player 2.");
        }
        else if (currentState == GameState.Player2Placement)
        {
            player2Board.HideShips();

            currentState = GameState.PassToPlayer1;

            gameUI.ShowPassPhonePanel(
                "Player 2 has finished placing ships.\n\n" +
                "Exchange your physical board markers.\n\n" +
                "Pass the phone back to Player 1.\n\n" +
                "Press Continue to begin the battle."
            );

            Debug.Log("Waiting to begin the battle.");
        }
    }

    public ShotResult ShootForCurrentPlayer(string coordinate)
    {
        Player attackingPlayer;
        Player defendingPlayer;

        if (currentState == GameState.Player1Turn)
        {
            attackingPlayer = player1;
            defendingPlayer = player2;
        }
        else if (currentState == GameState.Player2Turn)
        {
            attackingPlayer = player2;
            defendingPlayer = player1;
        }
        else
        {
            Debug.LogWarning(
                "A shot cannot be made during state: " + currentState
            );

            return ShotResult.Invalid;
        }

        if (defendingPlayer == null || defendingPlayer.board == null)
        {
            Debug.LogError("Defending player or board is not configured.");
            return ShotResult.Invalid;
        }

        if (!defendingPlayer.board.IsInitialized)
        {
            Debug.LogWarning(
                defendingPlayer.playerName +
                "'s marker must be scanned before shooting."
            );

            return ShotResult.Invalid;
        }

        ShotResult result = defendingPlayer.board.Shoot(coordinate);

        Debug.Log(
            attackingPlayer.playerName +
            " fired at " +
            coordinate +
            ": " +
            result
        );

        if (result == ShotResult.Hit)
        {
            if (defendingPlayer.AllShipsSunk())
            {
                currentState = GameState.GameOver;

                Debug.Log(attackingPlayer.playerName + " wins!");

                return result;
            }
        }

        if (result == ShotResult.Hit || result == ShotResult.Miss)
        {
            SwitchTurn();
        }

        return result;
    }

    private void SwitchTurn()
    {
        if (currentState == GameState.Player1Turn)
        {
            currentState = GameState.Player2Turn;
            Debug.Log("Player 2's turn.");
        }
        else if (currentState == GameState.Player2Turn)
        {
            currentState = GameState.Player1Turn;
            Debug.Log("Player 1's turn.");
        }
    }

    public void ContinueAfterPass()
    {
        if (currentState == GameState.PassToPlayer2)
        {
            currentState = GameState.Player2Placement;

            gameUI.HidePassPhonePanel();

            Debug.Log("Player 2 can now place ships.");
        }
        else if (currentState == GameState.PassToPlayer1)
        {
            currentState = GameState.Player1Turn;

            gameUI.HidePassPhonePanel();

            Debug.Log("Placement finished. Player 1 starts.");
            Debug.Log("Player 1 should scan Player 2's marker.");
        }
        else
        {
            Debug.LogWarning(
                "Continue cannot be used during state: " + currentState
            );
        }
    }

}
