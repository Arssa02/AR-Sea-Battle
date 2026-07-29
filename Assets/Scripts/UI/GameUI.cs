using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject placementPanel;
    public GameObject passPhonePanel;

    public TMP_Text passMessage;

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
}