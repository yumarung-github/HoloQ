using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChatable {
    public string EnemyName { get; set; }
}

public class ChatBox : MonoBehaviour, IChatable
{
    [SerializeField]
    private string enemyName;
    public string EnemyName { 
        get
        {
            return enemyName;
        }
        set
        {
            enemyName = value;
        }
    }
    void Start()
    {

    }
    // Start is called before the first frame update
}
