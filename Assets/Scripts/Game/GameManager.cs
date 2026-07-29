using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;

    public OceanBoard player1Board;
    public OceanBoard player2Board;

    public GameState currentState;

    public GameUI gameUI;
    private int currentShipIndex = 0;
    private bool placementTransitionInProgress = false;

    [SerializeField]
    private float finalShipDisplayDelay = 3f;

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

        gameUI.SetPhaseTitle("PLAYER 1 PLACEMENT");

        gameUI.SetFeedback(
            "Place your " +
            GetCurrentShipName() +
            " (size " +
            GetCurrentShipSize() +
            ")."
        );

        gameUI.ShowPlacementControls();
    }


    public bool PlaceShipForCurrentPlayer(
        string startCoordinate,
        Direction direction,
        int shipSize,
        string shipName)
    {
        Player currentPlayer;

        if (placementTransitionInProgress)
        {
            Debug.LogWarning("Please wait for the placement phase to finish.");
            return false;
        }

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
            gameUI.SetFeedback(
                "Place your " +
                GetCurrentShipName() +
                " (size " +
                GetCurrentShipSize() +
                ")."
            );

            return;
        }

        currentShipIndex = 0;

        if (currentState == GameState.Player1Placement)
        {
            StartCoroutine(FinishPlayer1Placement());
        }
        else if (currentState == GameState.Player2Placement)
        {
            StartCoroutine(FinishPlayer2Placement());
        }
    }

    private IEnumerator FinishPlayer1Placement()
    {
        placementTransitionInProgress = true;
        currentState = GameState.PassToPlayer2;

        gameUI.ShowPassPhonePanel(
            "Player 1 has placed all ships.\n\n" +
            "Pass the phone to Player 2 in 3..."
        );

        yield return new WaitForSeconds(1f);

        gameUI.SetPassMessage(
            "Player 1 has placed all ships.\n\n" +
            "Pass the phone to Player 2 in 2..."
        );

        yield return new WaitForSeconds(1f);

        gameUI.SetPassMessage(
            "Player 1 has placed all ships.\n\n" +
            "Pass the phone to Player 2 in 1..."
        );

        yield return new WaitForSeconds(1f);

        player1Board.HideShips();

        gameUI.SetPassMessage(
            "Player 1's ships are now hidden.\n\n" +
            "Pass the phone to Player 2.\n\n" +
            "When Player 2 is ready, press Continue."
        );

        placementTransitionInProgress = false;
    }

    private IEnumerator FinishPlayer2Placement()
    {
        placementTransitionInProgress = true;
        currentState = GameState.PassToPlayer1;

        gameUI.ShowPassPhonePanel(
            "Player 2 has placed all ships.\n\n" +
            "The battle begins in 3..."
        );

        yield return new WaitForSeconds(1f);

        gameUI.SetPassMessage(
            "Player 2 has placed all ships.\n\n" +
            "The battle begins in 2..."
        );

        yield return new WaitForSeconds(1f);

        gameUI.SetPassMessage(
            "Player 2 has placed all ships.\n\n" +
            "The battle begins in 1..."
        );

        yield return new WaitForSeconds(1f);

        player2Board.HideShips();

        gameUI.SetPassMessage(
            "Both players' ships are hidden.\n\n" +
            "Exchange your physical board markers.\n\n" +
            "Pass the phone back to Player 1.\n\n" +
            "Press Continue to begin the battle."
        );

        placementTransitionInProgress = false;
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

        string sunkShipName = defendingPlayer.board.LastSunkShipName;

        if (result == ShotResult.Hit)
        {
            gameUI.SetFeedback("Hit at " + coordinate + "!");
        }
        else if (result == ShotResult.Miss)
        {
            gameUI.SetFeedback("Miss at " + coordinate + ".");
        }
        else if (result == ShotResult.AlreadyShot)
        {
            gameUI.SetFeedback(
                coordinate + " was already targeted. Choose another cell."
            );
        }
        else
        {
            gameUI.SetFeedback("That shot could not be made.");
        }

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

                gameUI.ShowGameOver(attackingPlayer.playerName);

                Debug.Log(attackingPlayer.playerName + " wins!");

                return result;
            }
        }

        if (result == ShotResult.Hit || result == ShotResult.Miss)
        {
            SwitchTurn(result, coordinate, sunkShipName);
        }

        return result;
    }

    private void SwitchTurn(ShotResult result, string coordinate, string sunkShipName)
    {
        string resultMessage;

        if (!string.IsNullOrEmpty(sunkShipName))
        {
            resultMessage =
                "Hit at " +
                coordinate +
                "!\n" +
                sunkShipName +
                " sunk!";
        }
        else if (result == ShotResult.Hit)
        {
            resultMessage = "Hit at " + coordinate + "!";
        }
        else
        {
            resultMessage = "Miss at " + coordinate + ".";
        }

        if (currentState == GameState.Player1Turn)
        {
            currentState = GameState.Player2Turn;

            gameUI.SetPhaseTitle("PLAYER 2 TURN");
            gameUI.SetFeedback(
                resultMessage +
                "\nPlayer 2: choose a coordinate on Player 1's board."
            );
        }
        else if (currentState == GameState.Player2Turn)
        {
            currentState = GameState.Player1Turn;

            gameUI.SetPhaseTitle("PLAYER 1 TURN");
            gameUI.SetFeedback(
                resultMessage +
                "\nPlayer 1: choose a coordinate on Player 2's board."
            );
        }
    }

    public void ContinueAfterPass()
    {
        if (placementTransitionInProgress)
        {
            Debug.LogWarning("Please wait for the countdown to finish.");
            return;
        }
        if (currentState == GameState.PassToPlayer2)
        {
            currentState = GameState.Player2Placement;

            gameUI.HidePassPhonePanel();
            gameUI.ShowPlacementControls();

            gameUI.SetPhaseTitle("PLAYER 2 PLACEMENT");

            gameUI.SetFeedback(
                "Place your " +
                GetCurrentShipName() +
                " (size " +
                GetCurrentShipSize() +
                ")."
            );

            Debug.Log("Player 2 can now place ships.");
        }
        else if (currentState == GameState.PassToPlayer1)
        {
            currentState = GameState.Player1Turn;

            gameUI.HidePassPhonePanel();
            gameUI.ShowBattleControls();

            gameUI.SetPhaseTitle("PLAYER 1 TURN");
            gameUI.SetFeedback("Choose a coordinate and shoot.");

            Debug.Log("Placement finished. Player 1 starts.");
        }
        else
        {
            Debug.LogWarning(
                "Continue cannot be used during state: " + currentState
            );
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

}
