using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOrSlap : MonoBehaviour
{
    public GameObject Character;
    void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Enemy"&&gameObject.tag=="Run")
        {
            Time.timeScale = 0.75f;
            Character.transform.GetComponent<CharacterScript>().Run = false;
            //StartCoroutine(EnemyTouchTimer());
        }
    }
}
