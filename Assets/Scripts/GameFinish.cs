using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup winGroup;
    [SerializeField]
    private Button winButton;

    [SerializeField]
    private CanvasGroup looseGroup;
    [SerializeField]
    private Button looseButton;

    [SerializeField]
    private BotMovement bot;

    [SerializeField]
    private float fadeInDuration;

    private bool isShowing = false;
    public void OnWin()
    {
        if (isShowing)
            return;
       
        Show(winGroup, winButton);
    }

    public void OnLoose()
    {
        if (isShowing)
            return;
        Show(looseGroup, looseButton);
    }

    public void OnWinButtonPressed()
    {
        var ind = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(ind);
    }

    public void OnLooseButtonPress()
    {

        SceneManager.LoadScene(0);
    }

    private void Show(CanvasGroup group, Button button)
    {
        isShowing = true;
        bot.enabled = false;
        bot.GetComponent<Rigidbody2D>().isKinematic = true;
        group.gameObject.SetActive(true);
        button.interactable = false;
        group.alpha = 0;
        DOTween.Sequence()
            .Append(DOTween.To(() => group.alpha, (a) => group.alpha = a, 1, fadeInDuration).SetUpdate(true))
            .AppendCallback(() => button.interactable = true);
    }
}
