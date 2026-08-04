using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject placementPanel;
    public GameObject passPhonePanel;
    public TMP_Text passMessage;
    public TMP_Text phaseTitle;
    public TMP_Text feedbackText;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    public GameObject directionDropdown;
    public GameObject placeButton;
    public GameObject shootButton;
    public void ShowPassPhonePanel(string message) {
        passMessage.text = message;

        placementPanel.SetActive(false);
        passPhonePanel.SetActive(true);
    }

    public void HidePassPhonePanel() {
        passPhonePanel.SetActive(false);
        placementPanel.SetActive(true);
    }

    public void ContinueGame() {
        gameManager.ContinueAfterPass();
    }

    public void SetPassMessage(string message) {
        passMessage.text = message;
    }

    public void SetPhaseTitle(string title) {
        phaseTitle.text = title;
    }

    public void SetFeedback(string message) {
        feedbackText.text = message;
    }

    public void ShowGameOver(string winnerName) {
        HideActionControls();

        placementPanel.SetActive(false);
        passPhonePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        winnerText.text = winnerName + " WINS!";
    }

    public void ShowPlacementControls() {
        directionDropdown.SetActive(true);
        placeButton.SetActive(true);
        shootButton.SetActive(false);
    }

    public void ShowBattleControls() {
        Debug.Log("Switching UI to battle controls");
        
        directionDropdown.SetActive(false);
        placeButton.SetActive(false);
        shootButton.SetActive(true);
    }

    public void HideActionControls() {
        directionDropdown.SetActive(false);
        placeButton.SetActive(false);
        shootButton.SetActive(false);
    }

}