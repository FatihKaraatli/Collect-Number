using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using CountDownTimer;

public class Dot : MonoBehaviour
{
    [Header("Dot Animation Timer")]
    public Timer animationTimer;

    [Header("Number Text")]
    public TextMeshProUGUI numberText;

    [Header("Dot Trail Prefab")]
    public GameObject trailEffect;

    [Header("Dot Colors")]
    public Color redColor;
    public Color greenColor;
    public Color blueColor;
    public Color orangeColor;
    public Color purpleColor;
    public Color blackColor;

    [Header("Dot Number")]
    public int _currentNumber;

    [Header("Dot Layer Mask")]
    public LayerMask dotLayerMask;

    [Header("Dot GameObject Himself ")]
    public GameObject obj;
    

    private GameObject iconPosParent;
    private GameObject[] iconPosArray;

    private HashSet<GameObject> destroyedDotsRight = new HashSet<GameObject>();
    private HashSet<GameObject> destroyedDotsLeft = new HashSet<GameObject>();
    private HashSet<GameObject> destroyedDotsUp = new HashSet<GameObject>();
    private HashSet<GameObject> destroyedDotsDown = new HashSet<GameObject>();
    private HashSet<GameObject> destroyedDotsRightAndLeft = new HashSet<GameObject>();
    private HashSet<GameObject> destroyedDotsUpAndDown = new HashSet<GameObject>();
    
    private HashSet<GameObject> destroyedDotsColorful = new HashSet<GameObject>();

    private bool _isbuttonClicked = false;

    private Vector2 startScale;
    private Vector2 startColliderScale;

    private bool _wasMoving;
    private bool _isMoving;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startScale = transform.localScale;
        startColliderScale = this.gameObject.GetComponent<BoxCollider2D>().size;
        
        iconPosParent = GameObject.FindGameObjectWithTag("Icon");
        iconPosArray = new GameObject[iconPosParent.transform.childCount];

        for (int i = 0; i < iconPosParent.transform.childCount; i++)
        {
            iconPosArray[i] = iconPosParent.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (_isbuttonClicked)
        {
            if (animationTimer.Done())
            {
                ButtonAnimationTurnNormalSize();
                ClickedDot();
                animationTimer.ResetTimer();
                _isbuttonClicked = false;
            }
            else
            {
                animationTimer.UpdateTimer();
                this.transform.localScale = Vector2.Lerp(startScale, startScale / 1.5f, animationTimer.NormalizedTime);
                this.gameObject.GetComponent<BoxCollider2D>().size = Vector2.Lerp(startColliderScale, startColliderScale * 1.5f, animationTimer.NormalizedTime);
            }
        }
    }

    void FixedUpdate()
    {
        float velocityMagnitude = rb.velocity.magnitude;
        _isMoving = velocityMagnitude > 0;

        if (_wasMoving && !_isMoving)
        {
            FindMatches();
        }

        _wasMoving = _isMoving;
    }

    public void NumberIncreaseButton()
    {
        if (_currentNumber < 5)
        {
            _currentNumber++;
            numberText.text = _currentNumber.ToString();
            GameBoardManager.instance.ColorManager(_currentNumber, this.gameObject);

            ColorDotsManager.instance.movesCount -= 1;
            ColorDotsManager.instance.moveCountText.text = ColorDotsManager.instance.movesCount.ToString();
        }
        else if(_currentNumber == 5)
        {
            _currentNumber++;
            GameBoardManager.instance.ColorManager(_currentNumber, this.gameObject);
            numberText.text = "";

            ColorDotsManager.instance.movesCount -= 1;
            ColorDotsManager.instance.moveCountText.text = ColorDotsManager.instance.movesCount.ToString();
        }
    }

    public void SetNumber(int number)
    {
        _currentNumber = number;
        if(number <= 5)
            numberText.text = number.ToString();
        else 
            numberText.text = " ";
    }
    public int GetNumber()
    {
        return _currentNumber;
    }

    public void FindMatches()
    {
        //Check Right
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, 100, dotLayerMask);
        if (hitsRight.Length > 1)
        {
            for (int i = 1; i < hitsRight.Length; i++)
            {
                if (hitsRight[i].collider.gameObject.GetComponent<Dot>().GetNumber() == _currentNumber)
                {
                    destroyedDotsRight.Add(hitsRight[i].collider.gameObject);
                }
                else
                    break;
            }
        }

        //Check Left
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 100, dotLayerMask);
        if (hitsLeft.Length > 1)
        {
            for (int i = 1; i < hitsLeft.Length; i++)
            {
                if (hitsLeft[i].collider.gameObject.GetComponent<Dot>().GetNumber() == _currentNumber)
                {
                    destroyedDotsLeft.Add(hitsLeft[i].collider.gameObject);
                }
                else
                    break;
            }
        }

        //Check Up
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 100, dotLayerMask);
        if (hitUp.Length > 1)
        {
            for (int i = 1; i < hitUp.Length; i++)
            {
                if (hitUp[i].collider.gameObject.GetComponent<Dot>().GetNumber() == _currentNumber)
                {
                    destroyedDotsUp.Add(hitUp[i].collider.gameObject);
                }
                else
                    break;
            }
        }

        //Check Down
        RaycastHit2D[] hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down, 100, dotLayerMask);
        if (hitsDown.Length > 1)
        {
            for (int i = 1; i < hitsDown.Length; i++)
            {
                if (hitsDown[i].collider.gameObject.GetComponent<Dot>().GetNumber() == _currentNumber)
                {
                    destroyedDotsDown.Add(hitsDown[i].collider.gameObject);
                }
                else
                    break;
            }
        }

        destroyedDotsRightAndLeft.UnionWith(destroyedDotsRight);
        destroyedDotsRightAndLeft.UnionWith(destroyedDotsLeft);
        destroyedDotsUpAndDown.UnionWith(destroyedDotsUp);
        destroyedDotsUpAndDown.UnionWith(destroyedDotsDown);

        bool rightAndLeft = DestoyDots(destroyedDotsRightAndLeft);
        bool upAndDown = DestoyDots(destroyedDotsUpAndDown);

        if (destroyedDotsColorful.Count > 0)
        {
            foreach (GameObject dot in destroyedDotsColorful)
            {
                if (dot != null && dot.GetComponent<Dot>())
                {
                    dot.GetComponent<Dot>().CreateNewDot(dot);
                }
            }
        }

        ClearArrays();

        if (rightAndLeft || upAndDown)
        {
            CreateNewDot(this.gameObject);
        } 
    }

    public bool DestoyDots(HashSet<GameObject> dotsSet)
    {
        if (dotsSet.Count >= 2)
        {
            foreach (GameObject dot in dotsSet)
            {
                if (dot != null && dot.GetComponent<Dot>())
                {
                    if (dot.GetComponent<Dot>().GetNumber() == 6)
                    {
                        dotsSet.Add(this.gameObject);
                        DestoyDotsWithColorful(dotsSet);
                        return false;
                    }
                    else
                    {
                        dot.GetComponent<Dot>().CreateNewDot(dot);
                    }
                }
            }
            dotsSet.Clear();
            return true;
        }
        else
        {
            dotsSet.Clear();
            return false;
        }
    }

    public void ClearArrays()
    {
        destroyedDotsRightAndLeft.Clear();
        destroyedDotsUpAndDown.Clear();
        destroyedDotsLeft.Clear();
        destroyedDotsRight.Clear();
        destroyedDotsUp.Clear();
        destroyedDotsDown.Clear();
        destroyedDotsColorful.Clear();
    }

    public bool DestoyDotsWithColorful(HashSet<GameObject> dotsSet)
    {
        foreach (GameObject dot in dotsSet)
        {
            //Check Right
            RaycastHit2D[] hitsRight = Physics2D.RaycastAll(dot.transform.position, Vector2.right, 100, dotLayerMask);
            if (hitsRight.Length > 1)
            {
                for (int i = 0; i < hitsRight.Length; i++)
                {
                    if (hitsRight[i].collider.gameObject.GetComponent<Dot>())
                    {
                        destroyedDotsColorful.Add(hitsRight[i].collider.gameObject);
                    }
                    else
                        break;
                }
            }

            //Check Left
            RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(dot.transform.position, Vector2.left, 100, dotLayerMask);
            if (hitsLeft.Length > 1)
            {
                for (int i = 0; i < hitsLeft.Length; i++)
                {
                    if (hitsLeft[i].collider.gameObject.GetComponent<Dot>())
                    {
                        destroyedDotsColorful.Add(hitsLeft[i].collider.gameObject);
                    }
                    else
                        break;
                }
            }

            //Check Up
            RaycastHit2D[] hitUp = Physics2D.RaycastAll(dot.transform.position, Vector2.up, 100, dotLayerMask);
            if (hitUp.Length > 1)
            {
                for (int i = 0; i < hitUp.Length; i++)
                {
                    if (hitUp[i].collider.gameObject.GetComponent<Dot>())
                    {
                        destroyedDotsColorful.Add(hitUp[i].collider.gameObject);
                    }
                    else
                        break;
                }
            }

            //Check Down
            RaycastHit2D[] hitsDown = Physics2D.RaycastAll(dot.transform.position, Vector2.down, 100, dotLayerMask);
            if (hitsDown.Length > 1)
            {
                for (int i = 0; i < hitsDown.Length; i++)
                {
                    if (hitsDown[i].collider.gameObject.GetComponent<Dot>())
                    {
                        destroyedDotsColorful.Add(hitsDown[i].collider.gameObject);
                    }
                    else
                        break;
                }
            }
        }
        dotsSet.Clear();
        return true;
    }

    public void ClickedDot()
    {
        NumberIncreaseButton();
        FindMatches();
    }

    public void CreateNewDot(GameObject dot)
    {
        GameObject dotObj = Instantiate(obj, new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity);
        int newNumber = Random.Range(1, 6);

        dotObj.transform.localScale = dot.transform.localScale;
        dotObj.GetComponent<Dot>().SetNumber(newNumber);
        dotObj.name = "New Dot";

        if (_currentNumber < 6)
        {
            GameObject effect = Instantiate(trailEffect, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            effect.GetComponent<Trail>().iconPos = iconPosArray[_currentNumber - 1].transform;
            effect.GetComponent<Trail>().SetNumber(_currentNumber) ;

            var main = effect.gameObject.GetComponent<ParticleSystem>().main;
            main.startColor = dot.GetComponent<SpriteRenderer>().color;
        }


        GameBoardManager.instance.ColorManager(newNumber, dotObj);

        AudioController.instance.StartAudio();

        Destroy(dot);
    }

    public void SetClicked(bool tmp)
    {
        _isbuttonClicked = tmp;
    }

    public void ButtonAnimationTurnNormalSize()
    {
        this.transform.localScale = startScale;
        this.gameObject.GetComponent<BoxCollider2D>().size = startColliderScale;        
    }

    public void ColorManager()
    {
        if (_currentNumber == 1) this.gameObject.GetComponent<SpriteRenderer>().color = redColor;
        else if (_currentNumber == 2) this.gameObject.GetComponent<SpriteRenderer>().color = greenColor;
        else if (_currentNumber == 3) this.gameObject.GetComponent<SpriteRenderer>().color = blueColor;
        else if (_currentNumber == 4) this.gameObject.GetComponent<SpriteRenderer>().color = orangeColor;
        else if (_currentNumber == 5) this.gameObject.GetComponent<SpriteRenderer>().color = purpleColor;
        else if (_currentNumber == 6) this.gameObject.GetComponent<SpriteRenderer>().color = blackColor;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Dot))]
public class DotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        Dot demoScript = (Dot)target;

        serializedObject.Update();

        demoScript.SetNumber(demoScript._currentNumber);
        demoScript.ColorManager();

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
