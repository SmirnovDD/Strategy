using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
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
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                Next();
    }
    public void Next()
    {
        switch(step)
        {
            case 1:
                text.text = "Number of units available";
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[1].SetActive(true);
                break;
            case 2:
                text.text = "Choose unit type here";
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[2].SetActive(true);
                break;
            case 3:
                text.text = "Choose soldier here";
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[3].SetActive(true);
                break;
            case 4:
                text.text = "Click on grid to place the soldier";
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[4].SetActive(true);
                break;
            case 5:
                text.text = "Press to start battle!";
                foreach (GameObject go in arrows)
                    go.SetActive(false);
                arrows[5].SetActive(true);
                break;
            case 6:
                PlayerPrefs.SetInt("checkedTutorial", 1);
                Destroy(gameObject);
                break;
        }

        step++;
    }
}
