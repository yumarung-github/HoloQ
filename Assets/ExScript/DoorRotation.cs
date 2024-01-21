using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    private int num;
    private bool doorOn;
    private string doorName;
    // Start is called before the first frame update
    void Start()
    {
        num = int.Parse(transform.parent.name);
        doorOn = false;
        switch (num)
        {
            case 1:
                doorName = "시작";
                break;
            case 2:
                doorName = "라플라스다크니스";
                break;
            case 3:
                doorName = "하쿠이코요리";
                break;
            case 4:
                doorName = "사카마타클로에";
                break;
            case 5:
                doorName = "카자마이로하";
                break;
            case 6:
                doorName = "타카네루이";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!doorOn)
        {
            if (GameManager.Instance.isClear[doorName])
            {
                if(transform.name == "Left")
                {
                    transform.Rotate(0, -90f, 0);
                    if (AudioManager.Instance.doorSound == null)
                        AudioManager.Instance.doorSound 
                            = AudioManager.Instance.PopBgm(AudioManager.Instance.doorClip, false, transform);
                }
                else
                {
                    transform.Rotate(0, 90f, 0);
                }
                doorOn=true;
            }
        }
        
    }
}
