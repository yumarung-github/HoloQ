using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : SingleTon<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    protected void Start()
    {
        if(instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update

    /*
     protected void Start()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)//스태틱 instance에 저장된 게임매니저가 자기자신이 아니면
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
     */
}
