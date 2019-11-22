using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAndCloseUnitPanel : MonoBehaviour
{
    public GameObject unitsBuyPanel;
    public Sprite[] btnSprites;
    private Image btnImage;
    private void Start()
    {
        btnImage = GetComponent<Image>();
    }

    public void OpenOrClosePanel()
    {
        if (unitsBuyPanel.activeSelf)
        {
            unitsBuyPanel.SetActive(false);

            btnImage.sprite = btnSprites[0];
        }
        else
        {
            unitsBuyPanel.SetActive(true);

            btnImage.sprite = btnSprites[1];
        }
    }
}
