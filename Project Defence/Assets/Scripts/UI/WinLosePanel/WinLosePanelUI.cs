using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLosePanelUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button nextOrRestartButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private float panelAppearSpeed = 3f;
    [SerializeField] private float panelDisappearSpeed = 3f;
    private bool isWin;
    private void OnEnable()
    {
        GameManager.OnGameEnd += GameManager_OnGameEnd;
        nextOrRestartButton.onClick.AddListener(ButtonFunction);
    }
    private void GameManager_OnGameEnd(bool isWin)
    {
        this.isWin = isWin;
        OpenPanel();
    }
    private void OnDisable()
    {
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
        nextOrRestartButton.onClick.RemoveAllListeners();
    }
    private void OpenPanel()
    {
        if (isWin)
        {
            buttonText.text = "Next Level";
        }
        else
        {
            buttonText.text = "Restart";
        }
        StartCoroutine(OpenPanelVisual());
    }
    private IEnumerator OpenPanelVisual()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, panelAppearSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

    }
    private IEnumerator ClosePanelVisual()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, panelDisappearSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

    }
    private void ButtonFunction()
    {
        if (isWin)
        {
            LevelLoader.Instance.LoadNextLevel();
        }
        else
        {
            LevelLoader.Instance.RestartLevel();
        }
        StartCoroutine(ClosePanelVisual());
    }

}
