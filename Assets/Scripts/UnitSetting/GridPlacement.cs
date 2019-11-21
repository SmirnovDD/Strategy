using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    public LayerMask layerMask;
    public RegisterUnitsInGrids[] allCellsScripts;

    private GameObject selectedUnit;
    private Transform selectedCellTr;

    private bool movedUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(!GameController.battleStarted)
       {
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
                if (hit.collider)
                {
                    int index = int.Parse(hit.collider.name);

                    if (allCellsScripts[index].collidingObject)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began) //|| Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            if (!selectedUnit)
                                selectedUnit = allCellsScripts[index].collidingObject;
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
}
