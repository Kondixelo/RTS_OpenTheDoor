using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{   
    [Header("Menus Section")]
    public GameObject gameControllerObject;
    public GameObject startMenu;
    public GameObject gameOnMenu;
    public GameObject gameOverMenu;
    public GameObject messageMenu;

    [Header("Menus additions")]
    [Tooltip("Black Image to make fading in/out effect")]
    public GameObject blackScreenObject;
    [Tooltip("How long fade in/out effect lasts")]
    public float fadingTime;

    [Space(5f)]

    [Header("Menus additions")]
    [Tooltip("Sound when message menu appears")]
    public AudioClip messageMenuSound;
    [Tooltip("Sound when any button is clicked")]
    public AudioClip buttonSound;
    [Tooltip("Sound when game is over")]
    public AudioClip gameOverSound;

    //Start Menu Elements
    private Button startButton, playAgainButton;
    private Text bestTimeStartMenuText, currentTimeGameOnText, currentTimeGameOverText, bestTimeGameOverText;


    //PlayerPrefs variables
    private float currentTime;
    private float bestTime;

    //UI variables 
    private float timerSeconds;
    private float timerMinutes;
    private bool countTime;

    private GameController gameController; //Script attached to game controller object

    private Image blackImage; //Black screen object Image component

    //Black Image colors used in fade in/out effects
    private Color blackOpaque = Color.black;
    private Color blackTransparent = new Color(0f, 0f,0f,0f);
    
    private bool playAgain; 

    private GameObject interactedObject; //Object which player has interactions
    private ObjectInteraction objectInteraction; //Script attached to interacted object

    //Message menu elements
    private GameObject textObject;
    private Text textMessage;
    private GameObject yesButton;
    private GameObject noButton;
    private GameObject okButton;
 

    void Start()
    {
        playAgain = false;

        gameController = gameControllerObject.GetComponent<GameController>();
        //Start Menu
        startButton = startMenu.transform.GetChild(1).gameObject.GetComponent<Button>();
        bestTimeStartMenuText = startMenu.transform.GetChild(2).gameObject.GetComponent<Text>();
        //Game ON Menu
        currentTimeGameOnText = gameOnMenu.transform.GetChild(1).gameObject.GetComponent<Text>();
        //Game Over Menu
        currentTimeGameOverText = gameOverMenu.transform.GetChild(1).gameObject.GetComponent<Text>();
        bestTimeGameOverText = gameOverMenu.transform.GetChild(2).gameObject.GetComponent<Text>();
        playAgainButton = gameOverMenu.transform.GetChild(3).gameObject.GetComponent<Button>();

        //Message menu objects and components 
        textObject = messageMenu.transform.GetChild(1).gameObject;
        textMessage = textObject.GetComponent<Text>();
        yesButton = messageMenu.transform.GetChild(2).gameObject;
        noButton = messageMenu.transform.GetChild(3).gameObject;
        okButton = messageMenu.transform.GetChild(4).gameObject;
        //Some ui scales;
        startMenu.transform.localScale = Vector3.zero;
        gameOverMenu.transform.localScale = Vector3.zero;
        currentTimeGameOnText.transform.localScale = Vector3.zero;
        //Turn of All Menus
        startMenu.SetActive(false);
        gameOnMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        blackScreenObject.SetActive(true);
        blackImage = blackScreenObject.GetComponent<Image>();

        PauseGame();
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

    public void StartButtonClick() //Start Button
    {
        SoundManager.PlaySound(buttonSound,1f);
        StartCoroutine(HideStartMenu());
    }

    public void PlayAgainButtonClick() //Play Again Button
    {
        SoundManager.PlaySound(buttonSound,1f);
        StartCoroutine(HideGameOverMenu());
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator ShowStartMenu() //Show Start Menu
    {
        startMenu.SetActive(true);
        Sequence seqShow = DOTween.Sequence()
            .Append(startMenu.transform.DOScale(Vector3.one, 0.5f));
        
        yield return seqShow.WaitForCompletion();
    }


    private IEnumerator HideStartMenu() //Hide Start Menu and show Game ON Menu
    {
        Sequence seqHide = DOTween.Sequence()
            .Append(startMenu.transform.DOScale(Vector3.zero, 0.5f));

        yield return seqHide.WaitForCompletion();
        startMenu.SetActive(false);
        StartCoroutine(ShowGameOnMenu());
    }

    private IEnumerator ShowGameOnMenu() //Show Game ON Menu (start game)
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

    private IEnumerator HideGameOnMenu() //Hide Game ON Menu nad show Game Over Menu
    {
        messageMenu.SetActive(false);
        PauseGame();
        Sequence seqHide = DOTween.Sequence()
            .Append(currentTimeGameOnText.transform.DOScale(Vector3.zero, 0.5f));
        
        yield return seqHide.WaitForCompletion();
        gameOnMenu.SetActive(false);
        StartCoroutine(ShowGameOverMenu());
    }

    private IEnumerator ShowGameOverMenu() //Show Game Over Menu
    {
        gameOverMenu.SetActive(true);

        Sequence seqShow = DOTween.Sequence()
            .Append(gameOverMenu.transform.DOScale(Vector3.one, 0.5f));

        yield return seqShow.WaitForCompletion();
    }

    private IEnumerator HideGameOverMenu() //Hide Game Over Menu
    {
        Sequence seqHide = DOTween.Sequence()
            .Append(gameOverMenu.transform.DOScale(Vector3.zero, 0.5f));
        
        yield return seqHide.WaitForCompletion();
        gameOverMenu.SetActive(false);
    }

    private IEnumerator FadeInBlackScreen() //Do fade IN black screen effect, Start fade OUT black screen effect and Prepare another game
    {
        blackImage.gameObject.SetActive(true);
        Sequence seqIn = DOTween.Sequence()
            .Append(blackImage.DOColor(blackOpaque, fadingTime));

        yield return seqIn.WaitForCompletion();
        playAgain = true;
        StartCoroutine(FadeOutBlackScreen());
        gameController.PrepareGame();
    }

    private IEnumerator FadeOutBlackScreen()//Do the fade OUT black screen effect, Show Game ON Menu or Strat Menu
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


    public void PauseGame() //When Game On Menu has been hide;
    {   
        CheckTimes();
        
        currentTimeGameOverText.text = "Current Score: \n" + string.Format("{0:00}:{1:00}", + timerMinutes, Mathf.Round(timerSeconds));

        float highScoreTime = PlayerPrefs.GetFloat("highscore");
        float bestMinutes = Mathf.Floor(highScoreTime/60);
        float bestSeconds = highScoreTime%60;
        
        bestTimeGameOverText.text = "HighScore: \n"  + string.Format("{0:00}:{1:00}", + bestMinutes, Mathf.Round(bestSeconds));
        bestTimeStartMenuText.text = "HighScore: \n"  + string.Format("{0:00}:{1:00}", + bestMinutes, Mathf.Round(bestSeconds));
        countTime = false;
    }

    private void CheckTimes() // Compare current time with highscore
    {
        if (PlayerPrefs.GetFloat("highscore") == 0)
        {
            PlayerPrefs.SetFloat("highscore", currentTime);
        }else
        {
            if (currentTime < PlayerPrefs.GetFloat("highscore"))
            {
                PlayerPrefs.SetFloat("highscore", currentTime);
            }
        }    
    }

    public void GameOver()
    {
        StartCoroutine(HideGameOnMenu());
        SoundManager.PlaySound(gameOverSound,0.5f);
    }


    public void PointedObjectMessage(GameObject pointedObject) //Set pointed object as a interacted object and do sth due to what is object
    {
        SoundManager.PlaySound(messageMenuSound,1f);
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
        objectInteraction = interactedObject.GetComponent<ObjectInteraction>();
        bool objectOpened = objectInteraction.GetOpenStatus();
        if (!objectOpened)
        {
            messageMenu.SetActive(true);

            textObject.SetActive(true);
            textMessage.text = message;

            yesButton.SetActive(true);
            noButton.SetActive(true);
            okButton.SetActive(false);
        }

    }

    public void InteractWithObject(){ //YES Button
        SoundManager.PlaySound(buttonSound,1f);
        switch (interactedObject.tag){
            case "Door":
                messageMenu.SetActive(false); 
                gameController.OpenDoor();
                break;
            case "Chest":
                messageMenu.SetActive(false);  
                gameController.OpenChest();      
                break;
            case "Item":
                gameController.AddItemtoInventory();
                ItemMessage("You picked up a key", false);
                break;
            default:
                messageMenu.SetActive(false);
                break;
        }
    }

    public void DoNotIntercatWithObject() //NO Button
    { 
        SoundManager.PlaySound(buttonSound,1f);
        messageMenu.SetActive(false);
    } 
            
    public void ConfirmMessage() // OK Button
    { 
        SoundManager.PlaySound(buttonSound,1f);
        messageMenu.SetActive(false); 
    }

        
    public void ItemMessage(string message, bool activeOkButton) //Active message menu, show message/show message and Ok button
    {
        SoundManager.PlaySound(messageMenuSound,1f);
        messageMenu.SetActive(true);
        textObject.SetActive(true);
        textMessage.text = message;

        yesButton.SetActive(false);
        noButton.SetActive(false);
        if (activeOkButton)
        {
            okButton.SetActive(true);
        }else
        {
            StartCoroutine(CloseMessageMenuAfter(3));
        }
    }

    public IEnumerator CloseMessageMenuAfter(int seconds) // Close Messange Menu after X Seconds
    {
        yield return new WaitForSeconds(seconds);
        messageMenu.SetActive(false);
    }
}
