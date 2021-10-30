using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private int currentPage = 1;

    [Header("Pages")] 
    
    [SerializeField] private CanvasGroup intro;
    
    [SerializeField] private CanvasGroup pageOne;
    [SerializeField] private CanvasGroup pageTwo;
    [SerializeField] private CanvasGroup pageThree;
    [SerializeField] private CanvasGroup youWin;

    [Header("Buttons")] 
    
    [SerializeField] private Button previous;
    [SerializeField] private Button next;
    [SerializeField] private Button start;

    protected override void Awake()
    {
        start.enabled = true;
    }

    public void TurnNextPage()
    {
        currentPage++;
        RefreshPage();
        StartCoroutine(RefreshButtons());
        
        //Turn page sound
        AudioManager.Instance.Play("Turn Page");
    }

    public void TurnPreviousPage()
    {
        currentPage--;
        RefreshPage();
        StartCoroutine(RefreshButtons());
        
        //Turn page sound
        AudioManager.Instance.Play("Turn Page");
    }

    public void StartGame()
    {
        intro.DOFade(0, 0.5f);
        GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().movementEnabled = true;
        
        //Start music
        GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().StartBackgroundMusic();
    }

    public void GameOver()
    {
        //Disable buttons
        previous.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        
        youWin.DOFade(1, 0.5f);
        GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().movementEnabled = false;
        GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().StartEndingMusic();
        
        

        //Start music
        GameManager.Instance.player.GetComponentInChildren<PlayerMovement>().StartBackgroundMusic();
    }

    private void RefreshPage()
    {
        switch (currentPage)
        {
            case 1:
                pageOne.DOFade(1, 0.5f).SetDelay(0.5f);
                pageTwo.DOFade(0, 0.5f);
                pageThree.DOFade(0, 0.5f);
                break;
            case 2:
                pageOne.DOFade(0, 0.5f);
                pageTwo.DOFade(1, 0.5f).SetDelay(0.5f);
                pageThree.DOFade(0, 0.5f);
                break;
            case 3:
                pageOne.DOFade(0, 0.5f);
                pageTwo.DOFade(0, 0.5f);
                pageThree.DOFade(1, 0.5f).SetDelay(0.5f);
                break;
        }
    }

    private IEnumerator RefreshButtons()
    {
        ButtonInteraction(false);
        yield return new WaitForSeconds(0.5f);
        
        switch (currentPage)
        {
            case 1:
                previous.gameObject.SetActive(false);
                next.gameObject.SetActive(true);
                start.gameObject.SetActive(false);
                ButtonInteraction(true);
                break;
            case 2:
                previous.gameObject.SetActive(true);
                next.gameObject.SetActive(true);
                start.gameObject.SetActive(false);
                ButtonInteraction(true);
                break;
            case 3:
                previous.gameObject.SetActive(true);
                next.gameObject.SetActive(false);
                start.gameObject.SetActive(true);
                ButtonInteraction(true);
                break;
        }
    }

    private void ButtonInteraction(bool status)
    {
        previous.enabled = status;
        next.enabled = status;
        start.enabled = status;
    }
}
