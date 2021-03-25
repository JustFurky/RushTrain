using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class enemyScript : MonoBehaviour
{
    bool FailBool = false;
    bool Goback = false;
    Rigidbody rb;
    public GameObject Character;
    public GameObject[] InsideTrainPoint;
    public GameObject BackPoint;
    public bool Metrout = false;
    public float StartOutTime;
    bool FinishDestroy = false;
    public float randomRunSpeed;
    public float SlapTampon;

    public Material GriMat;

    public bool TriggerOn = false;
    Vector3 EndRotate = new Vector3(0, 0, 0);

    public Transform[] TargetX;

    void Start()
    {
        randomRunSpeed = Random.Range(2.5f, 6f);
        rb = transform.GetComponent<Rigidbody>();
        if (Metrout)
        {
            transform.GetComponent<Outline>().enabled = false;
            int Current = Random.Range(0, 1);
            Transform currentxPos = TargetX[Current];
            StartCoroutine(MetroOut(currentxPos));
        }
        else
        {
            Invoke("StartRunFunc", 1.3f);
        }
    }
    IEnumerator MetroOut(Transform Target)
    {
        yield return new WaitForSeconds(StartOutTime);
        transform.DOMoveX(Target.transform.position.x, 5);
        yield return new WaitForSeconds(0.1f);
        transform.DORotate(EndRotate, 6);
        StartRunFunc();
        yield return new WaitForSeconds(5f);
        transform.GetComponent<Outline>().enabled = true;
    }
    public void StartRunFunc()
    {
        transform.GetComponent<Animator>().SetTrigger("Start");
        rb.velocity = new Vector3(0, 0, randomRunSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="RGEnemy")
        {
            gameObject.tag = "Run";
            transform.GetComponent<Animator>().SetTrigger("FallAnim");
            transform.GetChild(1).transform.GetComponent<SkinnedMeshRenderer>().material = GriMat;
            rb.velocity = Vector3.zero;
        }
        if (other.tag == "Finish")
        {
            transform.GetComponent<Outline>().enabled = false;
            transform.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Finish());
        }
        if (other.gameObject.tag == "Obstacle")
        {
            if (other.transform.position.x == transform.position.x + (transform.localScale.x / 3))
            {
              StartCoroutine(ObstacleTimer());
            }
        }
    }
    private void Update()
    {
        if (Goback)
        {
            transform.position = Vector3.MoveTowards(transform.position, BackPoint.transform.position, 3 * Time.deltaTime);
        }
        if (FailBool == true)
        {
            transform.DOMove(InsideTrainPoint[Random.Range(0, 4)].transform.position, 2);
            FailBool = false;
        }
        if (Character.transform.GetComponent<CharacterScript>().trainGoTime && FinishDestroy)
        {
            Destroy(gameObject, 0.2f);
        }
    }
    IEnumerator ObstacleTimer()
    {
        rb.detectCollisions = false;
        transform.GetComponent<Animator>().SetTrigger("Obstacle");
        rb.velocity = Vector3.zero;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 3), 1);
        yield return new WaitForSeconds(2.5f);
        rb.velocity = new Vector3(0, 0, randomRunSpeed);
        yield return new WaitForSeconds(1.5f);
        rb.detectCollisions = true;
    }
    IEnumerator Finish()
    {
        if (Character.transform.GetComponent<CharacterScript>().GameFinish == true)
        {
            transform.GetComponent<Animator>().SetTrigger("LevelFailed");
            BackPoint.transform.parent = null;
            rb.velocity = Vector3.zero;
            Goback = true;
            yield return new WaitForSeconds(0.7f);
            Goback = false;
        }
        else
        {
            Character.GetComponent<CharacterScript>().DoluKoltuk++;
            FinishDestroy = true;
            rb.velocity = Vector3.zero;
            FailBool = true;
            yield return new WaitForSeconds(2.5f);
            FailBool = false;
            transform.GetComponent<Animator>().SetTrigger("Finish");
        }
    }
}
