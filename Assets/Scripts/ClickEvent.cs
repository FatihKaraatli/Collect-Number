using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began && !GameEndController.instance.GetGameState())
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);

                if (hitInfo)
                {
                    if (hitInfo.transform.gameObject.GetComponent<Dot>())
                    {
                        hitInfo.transform.gameObject.GetComponent<Dot>().SetClicked(true);
                    }
                }
            }
        }


        if (Input.GetMouseButtonDown(0) && !GameEndController.instance.GetGameState())
        {
            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

            if (hitInfo)
            {
                if (hitInfo.transform.gameObject.GetComponent<Dot>())
                {
                    hitInfo.transform.gameObject.GetComponent<Dot>().SetClicked(true);
                }
            }
        }
    }
}
