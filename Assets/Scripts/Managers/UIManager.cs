using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonPersistent<UIManager>
{
    [SerializeField]private GameObject mainCanvas;
    [SerializeField]private GameObject levelTextObject;
    [SerializeField]private GameObject progressionBar;
    private int initialGarbages;
    private int remainGarbages;
    public float progressionPercent;
    [SerializeField]private GameObject[] garbages;
    [SerializeField]private GameObject[] stars;
    [Header("Battery")]
    [SerializeField]private GameObject batteryBar;
    [SerializeField]private int initialBatteryCapacity;
    [SerializeField]private int remainBatteryCapacity;
    [SerializeField]private float batteryPercent;

    [Header("End Level Menu")]
    [SerializeField]private GameObject nextLevelButton;
    [SerializeField]private GameObject tryAgainButton;
    [SerializeField]private GameObject mainMenuButton;
    [SerializeField]private GameObject levelFailedImage;
    [SerializeField]private GameObject levelCopmletedImage;

    [SerializeField]private GameObject tutorials;
    [SerializeField]private GameObject mainMenu;

    [SerializeField]private GameObject musicButton;
    [SerializeField]private GameObject soundButton;
    [SerializeField]private Sprite musicSprite;
    [SerializeField]private Sprite musicMuteSprite;
    [SerializeField]private Sprite soundSprite;
    [SerializeField]private Sprite soundMuteSprite;

    [SerializeField]private bool music = true;
    [SerializeField]private bool sound=true;

    //MANAGERS
    private GameObject soundManager;
    private GameObject gameManager;
    private GameObject levelManager;

    void Start(){
        soundManager = GameObject.FindWithTag("SoundManager");
        gameManager = GameObject.FindWithTag("GameManager");
        levelManager = GameObject.FindWithTag("LevelManager");
        DontDestroyOnLoad(mainCanvas);
    }

    void Update(){
        if(levelManager.GetComponent<LevelManager>().currentLevelIndex!=0){
        CalculateProgression();
        SetProgressionPercent();
        CheckStars();
        CalculateBattery();
        SetBatteryPercent();
        }
    }

#region HEADER BAR
    void SetLevelText(int level){
        levelTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + (level).ToString();
    }

    void InitiateGarbages(){
        garbages = GameObject.FindGameObjectsWithTag("Garbage");
        initialGarbages = garbages.Length;
        remainGarbages = initialGarbages;
    }
    void SetProgressionPercent(){
        progressionBar.GetComponent<Slider>().value = progressionPercent;
    }

    float CalculateProgression(){
        progressionPercent = ((float)initialGarbages - (float)remainGarbages)/(float)initialGarbages;
        return progressionPercent;
    }

    void ReduceRemainGarbages(){
        remainGarbages--;
    }

    void CheckStars(){
        if(progressionPercent>.5f && !stars[0].activeInHierarchy){
            stars[0].SetActive(true);
            SoundManager.sfxInstance.audioSource.PlayOneShot(SoundManager.sfxInstance.starSound, .5f);
        }
        if(progressionPercent>.725f && !stars[1].activeInHierarchy){
            stars[1].SetActive(true);
            SoundManager.sfxInstance.audioSource.PlayOneShot(SoundManager.sfxInstance.starSound,.5f);
        }
        if(progressionPercent>.945f && !stars[2].activeInHierarchy){
            stars[2].SetActive(true);
            SoundManager.sfxInstance.audioSource.PlayOneShot(SoundManager.sfxInstance.starSound,.5f);
        }
    }
#endregion
#region BATERY BAR
    void SetInıtialBatteryCapacity(int tempBattery){
        initialBatteryCapacity = tempBattery;
        remainBatteryCapacity = tempBattery;
    }

    void CalculateBattery(){
        batteryPercent = 1 -((float)initialBatteryCapacity - (float)remainBatteryCapacity)/(float)initialBatteryCapacity;
    }

    void ReduceRemainBattery(){
        remainBatteryCapacity--;
    }
    void SetBatteryPercent(){
        batteryBar.GetComponent<Slider>().value = batteryPercent;
    }
#endregion
#region GAME MANAGER
    void LevelFailed(float waitTime){
        StartCoroutine(LevelFailedCoroutine( waitTime));
    }

    private IEnumerator LevelFailedCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        levelFailedImage.SetActive(true);
        tryAgainButton.SetActive(true);
        mainMenuButton.SetActive(true);
        yield return new WaitForSeconds(.5f);
        SoundManager.sfxInstance.audioSource.PlayOneShot(SoundManager.sfxInstance.failedSound,.7f);
    }

    void LevelCompleted(){
        SoundManager.sfxInstance.audioSource.PlayOneShot(SoundManager.sfxInstance.victorySound,.5f);
        levelCopmletedImage.SetActive(true);
        nextLevelButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
#endregion
#region BUTTON METHODS
    public void TryAgain(){
        levelManager.SendMessage("ReloadLevel");
    }

    public void NextLevel(){
        levelManager.SendMessage("LoadNextLevel");
    }

    public void MainMenu(){
        mainMenu.SetActive(true);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void StartGame(){
        levelManager.SendMessage("StartGame");
        mainMenu.SetActive(false);
    }

    public void MusicMute(){
        if(music){
            soundManager.SendMessage("StopMusic");
            musicButton.GetComponent<Image>().sprite = musicMuteSprite;
            music =false;
        }else{
            musicButton.GetComponent<Image>().sprite = musicSprite;
            soundManager.SendMessage("PlayMusic");
            music =true;
        }
    }

    public void SoundMute(){
        if(sound){
            soundManager.SendMessage("StopSound");
            soundButton.GetComponent<Image>().sprite = soundMuteSprite;
            sound =false;
        }else{
            soundButton.GetComponent<Image>().sprite = soundSprite;
            soundManager.SendMessage("PlaySound");
            sound =true;
        }
    }
#endregion
#region LEVEL MANAGER
    public void ResetUI(){
        levelCopmletedImage.SetActive(false);
        nextLevelButton.SetActive(false);
        mainMenuButton.SetActive(false);
        levelFailedImage.SetActive(false);
        tryAgainButton.SetActive(false);
        mainMenuButton.SetActive(false);
        stars[0].SetActive(false);
        stars[1].SetActive(false);
        stars[2].SetActive(false);
        InitiateGarbages();
    }
    void ActivateTutorials(){
        tutorials.SetActive(true);
    }
    void SetActiveMainMenu(bool trueorfalse){
        mainMenu.SetActive(trueorfalse);
    }
#endregion
}
