using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndController : MonoBehaviour
{
    [Header("UI Color Dot Texts")]
    public TextMeshProUGUI redText;
    public TextMeshProUGUI greenText;
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI orangeText;
    public TextMeshProUGUI purpleText;

    [Header("Game Failed Panel")]
    public GameObject gameFailedPanel;

    [Header("Game Complated Panel")]
    public GameObject gameComplatedPanel;

    [Header("Level Text")]
    public TextMeshProUGUI gameComplatedLevelText;

    private bool _isGameOver = false;

    public static GameEndController instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        gameComplatedLevelText.text = SceneManager.GetActiveScene().name + "\n" + "COMPLETED";
    }
    public void Update()
    {
        if (ColorDotsManager.instance.movesCount <= 0)
        {
            _isGameOver = true;
            ColorDotsManager.instance.moveCountText.text = "0";
            gameFailedPanel.SetActive(true);
        }
    }

    public void ManageColorCount(int colorNumber)
    {

        if (ColorDotsManager.instance.isRedAvailableThisScene && colorNumber == 1 && !redText.text.Equals("√") && int.Parse(redText.text) > 0) redText.text = (int.Parse(redText.text) - 1).ToString() ;        
        else if (ColorDotsManager.instance.isGreenAvailableThisScene && colorNumber == 2 && !greenText.text.Equals("√") && int.Parse(greenText.text) > 0) greenText.text = (int.Parse(greenText.text) - 1).ToString();
        else if (ColorDotsManager.instance.isBlueAvailableThisScene && colorNumber == 3 && !blueText.text.Equals("√") && int.Parse(blueText.text) > 0) blueText.text = (int.Parse(blueText.text) - 1).ToString();
        else if (ColorDotsManager.instance.isOrangeAvailableThisScene && colorNumber == 4 && !orangeText.text.Equals("√") && int.Parse(orangeText.text) > 0) orangeText.text = (int.Parse(orangeText.text) - 1).ToString();
        else if (ColorDotsManager.instance.isPurpleAvailableThisScene && colorNumber == 5 && !purpleText.text.Equals("√") && int.Parse(purpleText.text) > 0) purpleText.text = (int.Parse(purpleText.text) - 1).ToString() ;

        if (ColorDotsManager.instance.isRedAvailableThisScene && !redText.text.Equals("√") && int.Parse(redText.text) == 0) redText.text = "√";
        if (ColorDotsManager.instance.isGreenAvailableThisScene && !greenText.text.Equals("√") && int.Parse(greenText.text) == 0) greenText.text = "√";
        if (ColorDotsManager.instance.isBlueAvailableThisScene && !blueText.text.Equals("√") && int.Parse(blueText.text) == 0) blueText.text = "√";
        if (ColorDotsManager.instance.isOrangeAvailableThisScene && !orangeText.text.Equals("√") && int.Parse(orangeText.text) == 0) orangeText.text = "√";
        if (ColorDotsManager.instance.isPurpleAvailableThisScene && !purpleText.text.Equals("√") && int.Parse(purpleText.text) == 0) purpleText.text = "√";

        CheckIfGameEnds();
    }

    public void CheckIfGameEnds()
    {
        if ((!ColorDotsManager.instance.isRedAvailableThisScene || redText.text.Equals("√")) &&
            (!ColorDotsManager.instance.isGreenAvailableThisScene || greenText.text.Equals("√")) &&
            (!ColorDotsManager.instance.isBlueAvailableThisScene || blueText.text.Equals("√")) &&
            (!ColorDotsManager.instance.isOrangeAvailableThisScene || orangeText.text.Equals("√")) &&
            (!ColorDotsManager.instance.isPurpleAvailableThisScene || purpleText.text.Equals("√")))
        {
            if (ColorDotsManager.instance.movesCount >= 0)
            {
                _isGameOver = true;
                gameComplatedPanel.SetActive(true);
                return;
            }
        }
    }

    public bool GetGameState()
    {
        return _isGameOver; 
    }
}
