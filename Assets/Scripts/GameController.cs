using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GameAnalyticsSDK;

public class GameController : MonoBehaviour
{
    public int MoneyAmount
    {
        get { return moneyAmount; }
        set
        {
            moneyAmount = value;
            moneyAmountText.text = moneyAmount.ToString();
        }
    }
    public int UnitLimit
    {
        get { return unitLimit; }
        set
        {
            unitLimit = value;
            unitLimitAmountText.text = unitLimit.ToString();
        }
    }
    private int moneyAmount;
    private int unitLimit;

    public int availableUnitsUpgrades;

    public TextMeshProUGUI moneyAmountText, unitLimitAmountText;
    public GameObject battleEndedCanvas;
    public static GameObject battleEndedCanvasStatic;
    public TextMeshProUGUI teamWonText;
    public static TextMeshProUGUI teamWonTextStatic;
    public Image BGImage;
    public static Image BGImageStatic;
    public Sprite bgImageSprite;
    public static Sprite bgImageSpriteStatic;
    public GameObject joysticCanvas;
    public static GameObject joystickCanvasStatic;

    public Button loadNextLevelBtn;
    public Button loadPreviousLevelBtn;
    public TextMeshProUGUI levelNumberText;

    public TextMeshProUGUI battleEndedBtnText;
    public static TextMeshProUGUI battleEndedBtnTextStatic;

    public static bool battleEnded = false;
    public static bool battleStarted = false;
    public static bool enteredScene = false;

    public delegate void BattleStarted();
    public static BattleStarted OnBattleStarted;

    public GameObject grid;

    private static AudioSource cameraAudioS;
    public AudioClip[] winAndLooseClips;
    private static AudioClip[] winAndLooseClipsStatic;
    private static bool won;

    private void Start()
    {
        battleStarted = false;
        battleEnded = false;
        enteredScene = false;
        cameraAudioS = GetComponent<AudioSource>();

        levelNumberText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1).ToString();

        MoneyAmount = LevelsData.levelsMoneyLimits[SceneManager.GetActiveScene().buildIndex]; // !!!!!!!!!!!!!!!!!!! BUILD INDEX - 1 ЕСЛИ БУДЕТ МЕНЮ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        UnitLimit = LevelsData.levelsUnitsLimits[SceneManager.GetActiveScene().buildIndex]; // !!!!!!!!!!!!!!!!!!! BUILD INDEX - 1 ЕСЛИ БУДЕТ МЕНЮ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        availableUnitsUpgrades = LevelsData.levelsUnitsUpgradesAvailable[SceneManager.GetActiveScene().buildIndex];

        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Application.version, "Level: " + SceneManager.GetActiveScene().buildIndex.ToString());

    }
    public static bool BattleEnded
    {
        get { return battleEnded; }
        set
        {
            battleEnded = value;
            if(joystickCanvasStatic)
                joystickCanvasStatic.SetActive(false);
            //Time.timeScale = 0;
            if (battleEndedCanvasStatic)
                battleEndedCanvasStatic.SetActive(true);

            if (AllUnitsList.allAllies.Count == 0)
            {
                foreach (Transform tr in AllUnitsList.allEnemies)
                {
                    Animator anim = tr.gameObject.GetComponent<Animator>();
                    anim.SetBool("isMoving", false);
                    anim.SetBool("attack", false);
                }

                teamWonTextStatic.text = "Defeat!";
                battleEndedBtnTextStatic.text = "RESTART";
                teamWonTextStatic.color = Color.red;

                if (BGImageStatic)
                    BGImageStatic.sprite = bgImageSpriteStatic;

                won = false;
                cameraAudioS.clip = winAndLooseClipsStatic[1];
                cameraAudioS.volume = 0.5f;
                cameraAudioS.Play();
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, Application.version, "Level: " + SceneManager.GetActiveScene().buildIndex.ToString());
            }
            else
            {
                foreach (Transform tr in AllUnitsList.allAllies)
                {
                    Animator anim = tr.gameObject.GetComponent<Animator>();
                    anim.SetBool("isMoving", false);
                    anim.SetBool("attack", false);
                }

                teamWonTextStatic.text = "Victory!";
                battleEndedBtnTextStatic.text = "NEXT";

                won = true;
                cameraAudioS.clip = winAndLooseClipsStatic[0];
                cameraAudioS.Play();
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Application.version, "Level: " + SceneManager.GetActiveScene().buildIndex.ToString());
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        battleEndedCanvasStatic = battleEndedCanvas;
        teamWonTextStatic = teamWonText;
        BGImageStatic = BGImage;
        bgImageSpriteStatic = bgImageSprite;
        joystickCanvasStatic = joysticCanvas;
        battleEndedBtnTextStatic = battleEndedBtnText;
        winAndLooseClipsStatic = winAndLooseClips;

        if (SceneManager.GetActiveScene().buildIndex == 0 && loadPreviousLevelBtn)
            loadPreviousLevelBtn.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 59 && loadNextLevelBtn) //TEMP
            loadNextLevelBtn.gameObject.SetActive(false);

            battleStarted = false;
        battleEnded = false;
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < 59) //TEMP
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }
    public void LoadPreviousLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        else
            SceneManager.LoadScene(0);
    }

    public void LoadNextLevelOrLeloadOnBattleEnded()
    {
        if (won)
            LoadNextLevel();
        else
            ReloadScene();
    }
    public void StartBattle()
    {
        battleStarted = true;
        grid.SetActive(false);

        OnBattleStarted?.Invoke();        
    }

    public void EnterScene()
    {
        enteredScene = true;
    }
}
