using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{   
    public GameObject gameControllerObject;
    private GameController gameController;
    public GameObject startMenu;
    public GameObject gameOnMenu;
    public GameObject gameOverMenu;
    public GameObject messageMenu; 

    //Start Menu Elements
    private Button startButton, playAgainButton;
    private Text bestTimeStartMenuText, currentTimeGameOnText, currentTimeGameOverText, bestTimeGameOverText;


    private bool countTime;

    //PlayerPrefs variable
    private float currentTime;
    private float bestTime;

    //UI variables 
    private float timerSeconds;
    private float timerMinutes;


    public float fadingTime;
    public GameObject blackScreenObject;
    private Image blackImage;
    private Image logoImage;

    private Color blackOpaque = Color.black;
    private Color blackTransparent = Color.black;
    
    private bool playAgain;

    private GameObject interactedObject;

    void Start()
    {
        playAgain = false;
        gameController = gameControllerObject.GetComponent<GameController>();
        startMenu.transform.localScale = Vector3.zero;
        gameOverMenu.transform.localScale = Vector3.zero;

        

        startButton = startMenu.transform.GetChild(0).gameObject.GetComponent<Button>();
        bestTimeStartMenuText = startMenu.transform.GetChild(1).gameObject.GetComponent<Text>();
        
        currentTimeGameOnText = gameOnMenu.transform.GetChild(0).gameObject.GetComponent<Text>();

        currentTimeGameOverText = gameOverMenu.transform.GetChild(0).gameObject.GetComponent<Text>();
        bestTimeGameOverText = gameOverMenu.transform.GetChild(1).gameObject.GetComponent<Text>();
        playAgainButton = gameOverMenu.transform.GetChild(2).gameObject.GetComponent<Button>();

        currentTimeGameOnText.transform.localScale = Vector3.zero;
        PauseGame();
        startMenu.SetActive(false);
        gameOnMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        blackImage = blackScreenObject.GetComponent<Image>();
        blackTransparent.a = 0f;
        StartCoroutine(FadeOutBlackScreen());
    }

    void Update()
    {
        if (countTime)
        {
            timerSeconds += Time.deltaTime;
            currentTime += Time.deltaTime;
            if (timerSeconds >= 60f)
            {
                timerMinutes += 1;
                timerSeconds = 0;
            }

            currentTimeGameOnText.text = "Time: " + string.Format("{0:00}:{1:00}", + timerMinutes, Mathf.Round(timerSeconds));
        }
    }

    public void StartButtonClick()
    {
        StartCoroutine(HideStartMenu());
    }

        public void PlayAgainButtonClick()
    {
        StartCoroutine(HideGameOverMenu());
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator ShowStartMenu() //
    {
        startMenu.SetActive(true);
        Sequence seqShow = DOTween.Sequence()
            .Append(startMenu.transform.DOScale(Vector3.one, 0.5f));
        
        yield return seqShow.WaitForCompletion();
    }


    private IEnumerator HideStartMenu() //hide start menu and show GAME ON menu
    {
        Sequence seqHide = DOTween.Sequence()
            .Append(startMenu.transform.DOScale(Vector3.zero, 0.5f));

        yield return seqHide.WaitForCompletion();
        startMenu.SetActive(false);
        StartCoroutine(ShowGameOnMenu());
    }

    private IEnumerator ShowGameOnMenu()
    {
        gameOnMenu.SetActive(true);
        Sequence seqShow = DOTween.Sequence()
            .Append(currentTimeGameOnText.transform.DOScale(Vector3.one, 0.5f));

        yield return seqShow.WaitForCompletion();   
        countTime = true;

        timerSeconds = 0f;
        timerMinutes = 0f;
        currentTime = 0f;
        gameController.StartGame();

    }

    private IEnumerator HideGameOnMenu()
    {
        messageMenu.SetActive(false);
        PauseGame();
        Sequence seqHide = DOTween.Sequence()
            .Append(currentTimeGameOnText.transform.DOScale(Vector3.zero, 0.5f));
        
        yield return seqHide.WaitForCompletion();
        gameOnMenu.SetActive(false);
        StartCoroutine(ShowGameOverMenu());
    }

    private IEnumerator ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);

        Sequence seqShow = DOTween.Sequence()
            .Append(gameOverMenu.transform.DOScale(Vector3.one, 0.5f));

        yield return seqShow.WaitForCompletion();
    }

    private IEnumerator HideGameOverMenu()
    {
        Sequence seqHide = DOTween.Sequence()
            .Append(gameOverMenu.transform.DOScale(Vector3.zero, 0.5f));
        
        yield return seqHide.WaitForCompletion();
        gameOverMenu.SetActive(false);
    }



    private IEnumerator FadeOutBlackScreen()
    {
        Sequence seqOut = DOTween.Sequence()
           .Append(blackImage.DOColor(blackTransparent, fadingTime))
           .SetDelay(1f);

        yield return seqOut.WaitForCompletion();
        blackImage.gameObject.SetActive(false);
        if (playAgain)
        {
            StartCoroutine(ShowGameOnMenu());
            playAgain = false;
                    
        }else
        {
            StartCoroutine(ShowStartMenu());
        }

    }

    private IEnumerator FadeInBlackScreen()
    {
        blackImage.gameObject.SetActive(true);
        Sequence seqIn = DOTween.Sequence()
            .Append(blackImage.DOColor(blackOpaque, fadingTime));

        yield return seqIn.WaitForCompletion();
        playAgain = true;
        StartCoroutine(FadeOutBlackScreen());
        gameController.PrepareGame();

    }
    
    public void PauseGame() 
    {   
        CheckTimes();
        
        currentTimeGameOverText.text = "Current Score: " + string.Format("{0:00}:{1:00}", + timerMinutes, Mathf.Round(timerSeconds));

        float highScoreTime = PlayerPrefs.GetFloat("highscore");
        float bestMinutes = Mathf.Floor(highScoreTime/60);
        float bestSeconds = highScoreTime%60;
        
        bestTimeGameOverText.text = "HighScore: "  + string.Format("{0:00}:{1:00}", + bestMinutes, Mathf.Round(bestSeconds));
        bestTimeStartMenuText.text = "HighScore: "  + string.Format("{0:00}:{1:00}", + bestMinutes, Mathf.Round(bestSeconds));
        countTime = false;
    }

    private void CheckTimes()
    {
        if (currentTime < PlayerPrefs.GetFloat("highscore"))
        {
            PlayerPrefs.SetFloat("highscore", currentTime);
        }
    }

    public void GameOver()
    {
        StartCoroutine(HideGameOnMenu());
    }


    public void PointedObjectMessage(GameObject pointedObject)
    {
        interactedObject = pointedObject;
        gameController.SetInteractedObject(interactedObject);
        string message;
        switch (interactedObject.tag){
            case "Door":
            case "Chest":
                message = "Open?";
                break;
            case "Item":
                message = "Take?";
                break;
            default:
                message = "unknown command";
                break;
        }

        messageMenu.SetActive(true);

        GameObject textObject = messageMenu.transform.GetChild(0).gameObject;
        textObject.SetActive(true);
        Text textMessage = textObject.GetComponent<Text>();
        textMessage.text = message;

        GameObject yesButton = messageMenu.transform.GetChild(1).gameObject;
        yesButton.SetActive(true);
        GameObject noButton = messageMenu.transform.GetChild(2).gameObject;
        noButton.SetActive(true);
        GameObject okButton = messageMenu.transform.GetChild(3).gameObject;
        okButton.SetActive(false);
    }

    public void InteractWithObject(){ //kliknieto YES
        switch (interactedObject.tag){
            case "Door":
                gameController.OpenDoor();  
                break;
            case "Chest":
                gameController.OpenChest();
                messageMenu.SetActive(false);        
                break;
            case "Item":
                gameController.AddItemtoInventory();
                StartCoroutine(ItemMessage("You picked up a key"));
                break;
            default:
                messageMenu.SetActive(false);
                break;
        }
    }

    public void DoNotIntercatWithObject(){
        messageMenu.SetActive(false);
    }

    public void ConfirmMessage(){
        messageMenu.SetActive(false);
    }

    
    public IEnumerator ItemMessage(string message){
        messageMenu.SetActive(true);
        GameObject textObject = messageMenu.transform.GetChild(0).gameObject;
        textObject.SetActive(true);
        Text textMessage = textObject.GetComponent<Text>();
        textMessage.text = message;


        GameObject yesButton = messageMenu.transform.GetChild(1).gameObject;
        yesButton.SetActive(false);
        GameObject noButton = messageMenu.transform.GetChild(2).gameObject;
        noButton.SetActive(false);
        GameObject okButton = messageMenu.transform.GetChild(3).gameObject;
        okButton.SetActive(false);

        Sequence seqShow = DOTween.Sequence()
            .SetDelay(2f)
            .Append(textMessage.DOFade(0f, 0f));

        yield return seqShow.WaitForCompletion();
        messageMenu.SetActive(false);
        playAgain = true;
    }

}
