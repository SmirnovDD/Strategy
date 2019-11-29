using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public Image speechImg;
    public Sprite[] speechSprites;
    public GameObject[] arrows;
    private int step = 1;
    private void Start()
    {
        if (PlayerPrefs.GetInt("checkedTutorial", 0) == 1)
            Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                Next();
        }
        else if (Input.GetMouseButtonDown(0))
            Next();
    }
    public void Next()
    {
        switch(step)
        {
            case 1:
                speechImg.sprite = speechSprites[0];
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[1].SetActive(true);
                break;
            case 2:
                speechImg.sprite = speechSprites[1];
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[2].SetActive(true);
                break;
            case 3:
                speechImg.sprite = speechSprites[2];
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[3].SetActive(true);
                break;
            case 4:
                speechImg.sprite = speechSprites[3];
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[4].SetActive(true);
                break;
            case 5:
                speechImg.sprite = speechSprites[4];
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[5].SetActive(true);
                break;
            case 6:
                Destroy(gameObject);
                break;
        }

        step++;
    }
}
