using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tuzak : MonoBehaviour
{
    bool JumpOn = false;
    public BoxCollider NotTrigger;
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Enemy")
        {
            if (JumpOn)
            {
                transform.GetComponent<Animator>().SetTrigger("Jump");
                other.gameObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                other.gameObject.transform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-3,3), 15, Random.Range(-3,3));
                other.transform.GetComponent<Animator>().SetTrigger("Tuzak");
                other.transform.DORotate(new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)), 5);
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag=="Player")
        {
            JumpOn = true;
            NotTrigger.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            JumpOn = false;
            NotTrigger.enabled = false;
        }
    }
}
