using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAndCloseUnitPanel : MonoBehaviour
{
    public GameObject[] unitsBuyPanel;

    private Image btnImage;
    private void Start()
    {
        btnImage = GetComponent<Image>();
    }

    public void OpenOrClosePanel()
    {
        if (unitsBuyPanel[0].activeSelf)
        {
            unitsBuyPanel[0].SetActive(false);
            unitsBuyPanel[1].SetActive(false);
        }
        else
        {
            unitsBuyPanel[0].SetActive(true);
            unitsBuyPanel[1].SetActive(true);
        }
    }
}
