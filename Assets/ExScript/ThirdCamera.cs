using UnityEngine;
using System.Collections;

public class ThirdCamera : MonoBehaviour
{
    public float chatSmooth;
    public float smooth;
    [SerializeField]
       Transform standardPos;         
    //Vector3 standardPos;

    [SerializeField]
   Transform ChatPos;
  //  Vector3 chatPos;


    void Start()
    {
        standardPos = GameManager.Instance.cameraTrans;
        

        transform.position = standardPos.position;
        transform.forward = standardPos.forward;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.enemy != null)
            ChatPos = GameManager.Instance.enemy.transform.Find("ChatPos").transform;
        if(GameManager.Instance.player != null)
        {
            if (TalkManager.Instance.cameraOn)
            {
                setChatCameraPositionView();
            }
            else
            {
                setCameraPositionNormalView();
            }
        }
    }

    void setCameraPositionNormalView()
    {
        transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);
        transform.forward = Vector3.Slerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);

    }
    void setChatCameraPositionView()
    {
        transform.position = Vector3.Lerp(transform.position, ChatPos.position, Time.fixedDeltaTime * chatSmooth);
        transform.forward = Vector3.Slerp(transform.forward, ChatPos.forward, Time.fixedDeltaTime * chatSmooth);
    }
}