using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlacement : MonoBehaviour
{
    public GameObject removeAllUnitsButton;

    public Animator[] placeUnitsButtonsAnimators;
    public Button[] allUnitBuyButtons;

    public GameObject[] pikemen;
    public GameObject[] swordsmen;
    public GameObject[] archers;
    public GameObject[] horsemen;

    public Image[] buyUnitsButtonsImages;

    public Sprite[] pikemenBtnSprites;
    public Sprite[] swordsmenBtnSprites;
    public Sprite[] archersBtnSprites;
    public Sprite[] horsemenBtnSprites;

    public RegisterUnitsInGrids[] cellsUnitCheckScripts; //скрипты со всех клеточек, начиная с первой

    private int currentSelectedUnitType;

    private GameController gc;
    [HideInInspector]
    public int[] unitsCosts = { 25, 50, 75, 100, 150 };
    [HideInInspector]
    public List<GameObject> placedUnits = new List<GameObject>();

    public bool placingUnit;
    private GridPlacement gp;
    private void Start()
    {
        gc = GetComponent<GameController>();
        gp = GetComponent<GridPlacement>();
        SelectUnitType(0);
    }

    private void Update()
    {
        if (!GameController.battleStarted)
        {
            if (placedUnits.Count > 0)
                removeAllUnitsButton.SetActive(true);
            else
                removeAllUnitsButton.SetActive(false);
        }
    }
        public void PlaceUnit(int index)
    {
        foreach(RegisterUnitsInGrids rs in cellsUnitCheckScripts)
        {
            if(rs.collidingObject == null)
            {
                removeAllUnitsButton.GetComponent<Button>().interactable = false;
                gp.removeUnitButton.GetComponent<Button>().interactable = false;

                foreach(Button btn in allUnitBuyButtons)
                {
                    btn.interactable = false;
                }
                placingUnit = true;

                if (currentSelectedUnitType == 0)
                {
                    placeUnitsButtonsAnimators[index].SetBool("flicker", true);
                    //GameObject newUnit = Instantiate(pikemen[index], new Vector3(rs.transform.position.x, -0.1606905f, rs.transform.position.z), Quaternion.identity);

                    //UnitCost unitCost = newUnit.GetComponent<UnitCost>();
                    //unitCost.cost = unitsCosts[index];
                    //unitCost.limitCost = 1;

                    //placedUnits.Add(newUnit);

                    gp.unitToPlace = pikemen[index];
                    gp.unitPlaceButtonIndex = index;
                    gp.unitLimit = 1;

                    //SelectUnitType(0);
                }
                else if (currentSelectedUnitType == 1)
                {
                    placeUnitsButtonsAnimators[index].SetBool("flicker", true);

                    gp.unitToPlace = swordsmen[index];
                    gp.unitPlaceButtonIndex = index;
                    gp.unitLimit = 1;

                    //SelectUnitType(1);
                }
                else if (currentSelectedUnitType == 2)
                {
                    placeUnitsButtonsAnimators[index].SetBool("flicker", true);

                    gp.unitToPlace = archers[index];
                    gp.unitPlaceButtonIndex = index;
                    gp.unitLimit = 1;

                    //SelectUnitType(2);
                }
                else
                {
                    placeUnitsButtonsAnimators[index].SetBool("flicker", true);

                    gp.unitToPlace = horsemen[index];
                    gp.unitPlaceButtonIndex = index;
                    gp.unitLimit = 2;

                    //SelectUnitType(3);
                }
                break;
            }
        }
    }
    public void SelectUnitType(int typeIndex)
    {
        currentSelectedUnitType = typeIndex;

        if(typeIndex == 0)
        {
            for(int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = pikemenBtnSprites[i];
                if(gc.MoneyAmount < unitsCosts[i] || gc.UnitLimit < 1)
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
        else if (typeIndex == 1)
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = swordsmenBtnSprites[i];
                if (gc.MoneyAmount < unitsCosts[i] || gc.UnitLimit < 1)
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
        else if (typeIndex == 2)
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = archersBtnSprites[i];
                if (gc.MoneyAmount < unitsCosts[i] || gc.UnitLimit < 1)
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
        else                   
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = horsemenBtnSprites[i];
                if (gc.MoneyAmount < unitsCosts[i] || gc.UnitLimit < 2)
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    buyUnitsButtonsImages[i].gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void RemoveAllUnits()
    {
        for(int i = 0; i < placedUnits.Count; i++)
        {
            UnitCost unitCost = placedUnits[i].GetComponent<UnitCost>();
            gc.MoneyAmount += unitCost.cost;
            gc.UnitLimit += unitCost.limitCost;
            Destroy(placedUnits[i]);
        }

        placedUnits.Clear();

        if(gp.selectedUnit)
            gp.selectedUnit = null;
        gp.movedUnit = false;

        gp.allParticleSystems.Clear();
        SelectUnitType(0);
    }
}
