using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GridPlacement : MonoBehaviour
{
    public GameObject removeUnitButton;
    public GameObject unitToPlace;// выставляется из UnitPlacement скрипта при нажатии кнопки, ставим на свободную клетку
    public VirtualJoystick movementJoystick;
    public LayerMask layerMask;
    public RegisterUnitsInGrids[] allCellsScripts;

    [HideInInspector]
    public GameObject selectedUnit;
    private Transform selectedCellTr;
    [HideInInspector]
    public bool movedUnit;
    private GameObject lastSelectedUnit;
    [HideInInspector]
    public int unitPlaceButtonIndex; //из UnitPlacement
    [HideInInspector]
    public int unitLimit; //из UnitPlacement

    private GameController gc;
    private UnitPlacement up;
    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<GameController>();
        up = GetComponent<UnitPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
       if(!GameController.battleStarted)
       {
            if (lastSelectedUnit == null)
            {
                removeUnitButton.gameObject.SetActive(false);
            }
            else
            {
                removeUnitButton.gameObject.SetActive(true);
            }

            //#if UNITY_EDITOR
            //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //            RaycastHit hit;
            //            Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
            //            if (hit.collider)
            //            {
            //                int index = int.Parse(hit.collider.name);

            //                if (allCellsScripts[index].collidingObject)
            //                {
            //                    if (Input.GetMouseButtonDown(0))
            //                    {
            //                        if (!selectedUnit)
            //                            selectedUnit = allCellsScripts[index].collidingObject;
            //                        else
            //                            selectedUnit = null;
            //                    }
            //                }
            //                else
            //                {
            //                    if(selectedUnit && !allCellsScripts[index].collidingObject)
            //                    {
            //                        selectedCellTr = allCellsScripts[index].gameObject.transform;
            //                        selectedUnit.transform.position = new Vector3(selectedCellTr.position.x, selectedUnit.transform.position.y, selectedCellTr.position.z);
            //                    }
            //                }
            //            }
            //#elif UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
                if (hit.collider && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    int index = int.Parse(hit.collider.name);

                    if (allCellsScripts[index].collidingObject)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began) //|| Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            if (!selectedUnit)
                            {
                                selectedUnit = allCellsScripts[index].collidingObject;
                                lastSelectedUnit = selectedUnit;
                            }
                            else
                                selectedUnit = null;
                        }
                        else if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                        {
                            if (movedUnit)
                            {
                                selectedUnit = null;
                                movedUnit = false;
                            }
                        }
                    }
                    else
                    {
                        if (selectedUnit && !allCellsScripts[index].collidingObject && Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            selectedCellTr = hit.collider.transform;
                            selectedUnit.transform.position = new Vector3(selectedCellTr.position.x, selectedUnit.transform.position.y, selectedCellTr.position.z);
                            selectedUnit = null;
                        }
                        else if (selectedUnit && !allCellsScripts[index].collidingObject && Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            movedUnit = true;
                            selectedCellTr = hit.collider.transform;
                            selectedUnit.transform.position = new Vector3(selectedCellTr.position.x, selectedUnit.transform.position.y, selectedCellTr.position.z);
                        }
                        else
                        {
                            if (Input.GetTouch(0).phase == TouchPhase.Began && unitToPlace)
                            {
                                selectedCellTr = hit.collider.transform;
                                GameObject newUnit = Instantiate(unitToPlace, new Vector3(selectedCellTr.position.x, -0.1606905f, selectedCellTr.position.z), Quaternion.identity);

                                UnitCost unitCost = newUnit.GetComponent<UnitCost>();
                                unitCost.cost = up.unitsCosts[unitPlaceButtonIndex];
                                unitCost.limitCost = unitLimit;

                                gc.MoneyAmount -= up.unitsCosts[unitPlaceButtonIndex];
                                gc.UnitLimit -= unitLimit;

                                up.placedUnits.Add(newUnit);
                                selectedUnit = newUnit;
                                lastSelectedUnit = newUnit;
                                unitToPlace = null;

                                foreach (Animator anim in up.placeUnitsButtonsAnimators)
                                {
                                    anim.SetBool("flicker", false);
                                }

                                up.removeAllUnitsButton.GetComponent<Button>().interactable = true;
                                removeUnitButton.GetComponent<Button>().interactable = true;

                                up.SelectUnitType(0);
                                up.placingUnit = false;
                            }
                        }
                    }
                }
                else
                {
                    if(selectedUnit && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
                    {
                        selectedUnit = null;
                        movedUnit = false;
                    }
                }
            }
//#endif
        }
    }

    public void RemoveUnit()
    {
        UnitCost unitCost = lastSelectedUnit.GetComponent<UnitCost>();
        gc.MoneyAmount += unitCost.cost;
        gc.UnitLimit += unitCost.limitCost;
        up.placedUnits.Remove(lastSelectedUnit);
        Destroy(lastSelectedUnit);
        selectedUnit = null;
        movedUnit = false;
        up.SelectUnitType(0);
    }
}
