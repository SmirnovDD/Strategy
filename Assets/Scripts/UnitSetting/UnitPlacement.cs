using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlacement : MonoBehaviour
{
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

    public void PlaceUnit(int index)
    {
        foreach(RegisterUnitsInGrids rs in cellsUnitCheckScripts)
        {
            if(rs.collidingObject == null)
            {
                if(currentSelectedUnitType == 0)
                    Instantiate(pikemen[index], new Vector3(rs.transform.position.x, -0.1606905f, rs.transform.position.z), Quaternion.identity);
                else if(currentSelectedUnitType == 1)
                    Instantiate(swordsmen[index], new Vector3(rs.transform.position.x, -0.1606905f, rs.transform.position.z), Quaternion.identity);
                else if(currentSelectedUnitType == 2)
                    Instantiate(archers[index], new Vector3(rs.transform.position.x, -0.1606905f, rs.transform.position.z), Quaternion.identity);
                else
                    Instantiate(horsemen[index], new Vector3(rs.transform.position.x, -0.1606905f, rs.transform.position.z), Quaternion.identity);

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
            }
        }
        else if (typeIndex == 1)
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = swordsmenBtnSprites[i];
            }
        }
        else if (typeIndex == 2)
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = archersBtnSprites[i];
            }
        }
        else                   
        {
            for (int i = 0; i < buyUnitsButtonsImages.Length; i++)
            {
                buyUnitsButtonsImages[i].sprite = horsemenBtnSprites[i];
            }
        }
    }
}
