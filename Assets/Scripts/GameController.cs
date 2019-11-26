using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

    public TextMeshProUGUI moneyAmountText, unitLimitAmountText;
    public GameObject battleEndedCanvas;
    public static GameObject battleEndedCanvasStatic;
    public TextMeshProUGUI teamWonText;
    public static TextMeshProUGUI teamWonTextStatic;


    public static bool battleEnded = false;
    public static bool battleStarted = false;
    public static bool enteredScene = false;

    public delegate void BattleStarted();
    public static BattleStarted OnBattleStarted;

    public GameObject grid;

    private void Start()
    {
        battleStarted = false;
        battleEnded = false;
        enteredScene = false;

        MoneyAmount = LevelsData.levelsMoneyLimits[SceneManager.GetActiveScene().buildIndex]; // !!!!!!!!!!!!!!!!!!! BUILD INDEX - 1 ЕСЛИ БУДЕТ МЕНЮ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        UnitLimit = LevelsData.levelsUnitsLimits[SceneManager.GetActiveScene().buildIndex]; // !!!!!!!!!!!!!!!!!!! BUILD INDEX - 1 ЕСЛИ БУДЕТ МЕНЮ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }
    public static bool BattleEnded
    {
        get { return battleEnded; }
        set
        {
            battleEnded = value; Time.timeScale = 0;
            if(battleEndedCanvasStatic)
                battleEndedCanvasStatic.SetActive(true);
            if (AllUnitsList.allAllies.Count == 0)
                teamWonTextStatic.text = "YOUR TEAM LOST!";
            else
                teamWonTextStatic.text = "YOUR TEAM WON!";
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
        battleStarted = false;
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
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
