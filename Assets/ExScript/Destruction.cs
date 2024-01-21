using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player testPlayer))
        {
            testPlayer.agent.isStopped = true;
            gameObject.SetActive(false);
        }
    }
}
