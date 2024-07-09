using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CountDownTimer;


public class Trail : MonoBehaviour
{
    [HideInInspector]
    public Transform iconPos = null;

    [Header("Timer")]
    public Timer flyTimer;

    private Camera cam;

    private Vector3 startPos;

    private int _number;

    public void Start()
    {
        startPos = transform.position;
        cam = Camera.main;
    }

    void Update()
    {
        flyTimer.UpdateTimer();
        if (flyTimer.Done())
        {
            GameEndController.instance.ManageColorCount(_number);
            Destroy(this.gameObject);
        }
        this.gameObject.transform.position = Vector3.Lerp(startPos, GetIconPosition(startPos), flyTimer.NormalizedTime);       
    }

    public Vector3 GetIconPosition(Vector3 target)
    {
        Vector3 uiPos = iconPos.position;
        uiPos.z = (target - cam.transform.position).z;

        Vector3 result = cam.ScreenToWorldPoint(uiPos);

        return result;
    }

    public void SetNumber(int nbr)
    {
        _number = nbr;
    }
}
