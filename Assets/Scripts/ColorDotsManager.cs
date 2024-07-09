using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorDotsManager : MonoBehaviour
{
    [Header("Red Color Controller")]
    public GameObject redColorObj;
    public int redNumber;
    public bool isRedAvailableThisScene;

    [Header("Green Color Controller")]
    public GameObject greenColorObj;
    public int greenNumber;
    public bool isGreenAvailableThisScene;

    [Header("Blue Color Controller")]
    public GameObject blueColorObj;
    public int blueNumber;
    public bool isBlueAvailableThisScene;

    [Header("Orange Color Controller")]
    public GameObject orangeColorObj;
    public int orangeNumber;
    public bool isOrangeAvailableThisScene;

    [Header("Purple Color Controller")]
    public GameObject purpleColorObj;
    public int purpleNumber;
    public bool isPurpleAvailableThisScene;

    [Header("Move Count Controller")]
    public TextMeshProUGUI moveCountText;
    public GameObject movesCountObj;
    public int movesCount;

    public static ColorDotsManager instance { get; private set; }

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

}

#if UNITY_EDITOR


[CustomEditor(typeof(ColorDotsManager))]
public class ColorDotsEditor : Editor
{
    SerializedProperty redColorObjEditor;
    SerializedProperty greenColorObjEditor;
    SerializedProperty blueColorObjEditor;
    SerializedProperty orangeColorObjEditor;
    SerializedProperty purpleColorObjEditor;

    SerializedProperty movesCountObjEditor;

    private void OnEnable()
    {
        redColorObjEditor = serializedObject.FindProperty("redColorObj");
        greenColorObjEditor = serializedObject.FindProperty("greenColorObj");
        blueColorObjEditor = serializedObject.FindProperty("blueColorObj");
        orangeColorObjEditor = serializedObject.FindProperty("orangeColorObj");
        purpleColorObjEditor = serializedObject.FindProperty("purpleColorObj");

        movesCountObjEditor = serializedObject.FindProperty("movesCountObj");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ColorDotsManager demoScript = (ColorDotsManager)target;

        serializedObject.Update();

        GameObject red = redColorObjEditor.objectReferenceValue as GameObject;
        GameObject green = greenColorObjEditor.objectReferenceValue as GameObject;
        GameObject blue = blueColorObjEditor.objectReferenceValue as GameObject;
        GameObject orange = orangeColorObjEditor.objectReferenceValue as GameObject;
        GameObject purple = purpleColorObjEditor.objectReferenceValue as GameObject;

        GameObject moves = movesCountObjEditor.objectReferenceValue as GameObject;

        red.SetActive(demoScript.isRedAvailableThisScene);
        green.SetActive(demoScript.isGreenAvailableThisScene);
        blue.SetActive(demoScript.isBlueAvailableThisScene);
        orange.SetActive(demoScript.isOrangeAvailableThisScene);
        purple.SetActive(demoScript.isPurpleAvailableThisScene);

        red.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.redNumber + "";
        green.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.greenNumber + "";
        blue.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.blueNumber + "";
        orange.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.orangeNumber + "";
        purple.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.purpleNumber + "";

        moves.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = demoScript.movesCount + "";

        serializedObject.ApplyModifiedProperties();
    }
}
#endif