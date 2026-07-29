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

    public void ShowPassPhonePanel(string message)
    {
        passMessage.text = message;

        placementPanel.SetActive(false);
        passPhonePanel.SetActive(true);
    }

    public void HidePassPhonePanel()
    {
        passPhonePanel.SetActive(false);
        placementPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        gameManager.ContinueAfterPass();
    }

    public void SetPassMessage(string message)
    {
        passMessage.text = message;
    }

    public void SetPhaseTitle(string title)
    {
        phaseTitle.text = title;
    }

    public void SetFeedback(string message)
    {
        feedbackText.text = message;
    }


}