using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{
    [Header("Dot Prefab")]
    public GameObject dot;

    [Header("Dot Stopper Floor")]
    public GameObject floorWall;

    [Header("Matrix Row And Column")]
    public int matrixRow;
    public int matrixColumn;

    [Header("Dots Scale Down Value Used When Matrix Gets Bigger")]
    public float scaleDownValue;

    [Header("Dot Colors")]
    public Color redColor;
    public Color greenColor;
    public Color blueColor;
    public Color orangeColor;
    public Color purpleColor;
    public Color blackColor;

    [Header("Dot GameObject List")]
    public List<GameObject> dotsList = new List<GameObject>();

    private int[,] matrix;
    private GameObject[,] dotsArray;

    public static GameBoardManager instance { get; private set; }

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

    public void StartGame()
    {
        Debug.Log(Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)));

        matrix = new int[matrixRow, matrixColumn];
        dotsArray = new GameObject[matrixRow, matrixColumn];

        float distanceBetweenTwoDots = (Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x * 2f) / (matrixColumn);
        Vector2 firstPos = new Vector2(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x + (distanceBetweenTwoDots * 0.5f), 20);
        float row = firstPos.x;
        float column = firstPos.y;

        Vector2 wallPos = floorWall.transform.position;
        if (matrixColumn > 6)
            floorWall.transform.position = new Vector3(wallPos.x,  -1 *matrixColumn / 2);

        for (int i = 0; i < matrixRow; i++)
        {
            row = firstPos.x;
            if (i != 0) column -= 2f;
            for (int j = 0; j < matrixColumn; j++)
            {
                matrix[i, j] = Random.Range(1, 6);
                GameObject dotObj = Instantiate(dot, new Vector2(row, column), Quaternion.identity);
                dotObj.name = i + " , " + j;
                Vector3 scale = dotObj.transform.localScale;
                dotsArray[i, j] = dotObj;
                dotsList.Add(dotObj);

                ColorManager(matrix[i, j], dotsArray[i, j]);

                if (matrixColumn > 4)
                {
                    dotObj.transform.localScale = new Vector3(scale.x * scaleDownValue * 4 / matrixColumn,
                                                              scale.y * scaleDownValue * 4 / matrixColumn,
                                                              scale.z * scaleDownValue * 4 / matrixColumn);
                }
                row += distanceBetweenTwoDots;
            }
        }
        FindMatchingNumbersAllMatrixAtStart();
    }

    private void FindMatchingNumbersAllMatrixAtStart()
    {
        // Check rows for matches
        for (int i = 0; i < matrixRow; i++)
        {
            for (int j = 0; j < matrixColumn - 2; j++)
            {
                if (matrix[i, j] == matrix[i, j + 1] && matrix[i, j] == matrix[i, j + 2])
                {
                    Debug.Log("Number is row: " + matrix[i, j]);
                    int matchingNumber = matrix[i, j];
                    matrix[i, j] = Random.Range(1, 6);
                    while (matrix[i, j] == matchingNumber)
                    {
                        matrix[i, j] = Random.Range(1, 6);
                    }
                    ColorManager(matrix[i, j], dotsArray[i, j]);
                }
            }
        }

        // Check columns for matches
        for (int i = 0; i < matrixRow - 2; i++)
        {
            for (int j = 0; j < matrixColumn; j++)
            {
                if (matrix[i, j] == matrix[i + 1, j] && matrix[i, j] == matrix[i + 2, j])
                {
                    Debug.Log("Number is column: " + matrix[i, j]);
                    int matchingNumber = matrix[i, j];
                    matrix[i, j] = Random.Range(1, 6);
                    while (matrix[i, j] == matchingNumber)
                    {
                        matrix[i, j] = Random.Range(1, 6);
                    }
                    ColorManager(matrix[i, j], dotsArray[i, j]);
                }
            }
        }
    }

    public void ColorManager(int i, GameObject sprite)
    {
        sprite.GetComponent<Dot>().SetNumber(i);
        sprite.GetComponent<Dot>().numberText.text = i.ToString();

        if (i == 1) sprite.GetComponent<SpriteRenderer>().color = redColor;
        else if (i == 2) sprite.GetComponent<SpriteRenderer>().color = greenColor;
        else if (i == 3) sprite.GetComponent<SpriteRenderer>().color = blueColor;
        else if (i == 4) sprite.GetComponent<SpriteRenderer>().color = orangeColor;
        else if (i == 5) sprite.GetComponent<SpriteRenderer>().color = purpleColor;
        else if (i == 6) sprite.GetComponent<SpriteRenderer>().color = blackColor;
    }

    public void ClearDots()
    {
        for (int i = 0; i < dotsList.Count; i++)
        {
            DestroyImmediate(dotsList[i]);
        }
        dotsList.Clear();
    }

    public int[,] GetMatrix()
    {
        return matrix;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(GameBoardManager))]
public class GameBoardManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GameBoardManager demoScript = (GameBoardManager)target;
        
        if (GUILayout.Button("Add Dots", GUILayout.Width(90f)))
        {
            demoScript.StartGame();
        }
        if (GUILayout.Button("Clear Dots", GUILayout.Width(90f)))
        {
            demoScript.ClearDots();
        }

        EditorGUILayout.LabelField("Make Dots Up");
        if (GUILayout.Button("Up", GUILayout.Width(90f)))
        {
            if (demoScript.dotsList.Count > 0)
            {
                foreach (GameObject dot in demoScript.dotsList)
                {
                    dot.transform.position += new Vector3(0, 0.5f, 0);
                }
            }
        }
        EditorGUILayout.LabelField("Make Dots Down");
        if (GUILayout.Button("Down", GUILayout.Width(90f)))
        {
            if (demoScript.dotsList.Count > 0)
            {
                foreach (GameObject dot in demoScript.dotsList)
                {
                    dot.transform.position -= new Vector3(0, 0.5f, 0);
                }
            }
        }

        EditorUtility.SetDirty(demoScript);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif

