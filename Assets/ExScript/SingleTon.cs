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
        else if (instance != this)//����ƽ instance�� ����� ���ӸŴ����� �ڱ��ڽ��� �ƴϸ�
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
     */
}
