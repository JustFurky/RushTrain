using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TapticPlugin;

public class CharacterScript : MonoBehaviour
{
    public GameObject Canvas;
    [Range(1, 20)]
    public float MovSpeed;
    public float HorizontalSpeed;
    public int DoluKoltuk;
    public int maxKoltuk;
    public GameObject Trains;
    public GameObject Camera;
    public GameObject TrainInsıde;
    public bool levelfailed = false;
    public bool GameFinish = false;
    bool finalBool = true;

    public GameObject SolSinir;
    public GameObject SagSinir;

    public GameObject Train1;
    public GameObject Train2;
    public Transform TrainLastPoint;

    public bool Run = false;

    public bool trainGoTime = false;

    public Vector3 CameraLastPos;
    public Vector3 CameraLastRot;
    public FloatingJoystick MyJoystick;
    bool IsStart = false;
    Vector3 translation;
    Touch touch;
    public bool LeftFall = false;
    public bool RightFall = false;
    Rigidbody rb;
    public bool TutorialActive;
    bool RotateTime = false;
    bool IsStopTouch = false;
    GameObject OtherObject;
    bool Elindebirivar = false;
    Vector2 FirstPressPos;
    public bool tutoLevel = false;
    void Start()
    {
        Vector3 TrainInPos = new Vector3(TrainInsıde.transform.position.x, transform.position.y, TrainInsıde.transform.position.z);
        rb = transform.GetComponent<Rigidbody>();
        if (tutoLevel==false)
        {
            Invoke("StartRunFunc", 2f);
        }
    }
    public void StartRunFunc()
    {
        transform.GetComponent<Animator>().SetTrigger("Start");
        IsStart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && Elindebirivar == false)
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            //otherobject
            other.gameObject.tag = "RGEnemy";
            other.transform.GetComponent<Outline>().enabled = false;
            OtherObject = other.gameObject;
            OtherObject.transform.parent = gameObject.transform;
            OtherObject.transform.localPosition = new Vector3(0, 0.5f, 0.5f);
            OtherObject.transform.rotation = Quaternion.identity;
            OtherObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            OtherObject.GetComponent<enemyScript>().enabled = false;
            OtherObject.GetComponent<BoxCollider>().enabled = false;
            OtherObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            OtherObject.transform.GetComponent<Animator>().SetTrigger("RotateAnim");
            transform.GetComponent<Animator>().SetTrigger("TouchEnemy");
            //gameobject
            rb.detectCollisions = false;
            transform.GetComponent<BoxCollider>().enabled = false;
            IsStart = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            RotateTime = true;
            IsStopTouch = true;
            Time.timeScale = 0.5f;
            if (TutorialActive)
            {
                Canvas.transform.GetComponent<UIScript>().tutorialOpener();
                TutorialActive = false;
            }
        }
        if (other.gameObject.tag == "Finish")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            Camera.transform.GetComponent<SmoothCamera>().enabled = false;
            if (finalBool)
            {
                finalBool = false;
                GameFinish = true;
                if (DoluKoltuk <= maxKoltuk)
                {
                    StartCoroutine(Finish());
                }
                if (DoluKoltuk > maxKoltuk)
                {
                    StartCoroutine(Failed());
                }
                Camera.transform.DOMove(CameraLastPos, 2);
                Camera.transform.DORotate(CameraLastRot, 2);
            }
            if (other.tag == "LastStop")
            {
                IsStart = false;
                rb.velocity = Vector3.zero;
            }
        }
        if (other.tag == "Obstacle")
        {
            StartCoroutine(ObstacleTimer());
        }
    }

    void Update()
    {
        if (RotateTime)
        {
            transform.Rotate(0, -5, 0);
        }
        if (trainGoTime)
        {
            Train1.transform.position = Vector3.MoveTowards(Train1.transform.position, TrainLastPoint.transform.position, 10 * Time.deltaTime);
        }
        if (IsStart)
        {
            rb.velocity = new Vector3(0, 0, MovSpeed);
        }
        if (Input.GetMouseButtonUp(0) && IsStopTouch)
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            Time.timeScale = 1;
            transform.GetComponent<Animator>().SetTrigger("ReleaseEnemy");
            OtherObject.transform.parent = null;
            OtherObject.transform.GetComponent<BoxCollider>().enabled = true;
            OtherObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            OtherObject.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
            IsStopTouch = false;
            RotateTime = false;
            transform.rotation = Quaternion.identity;
            IsStart = true;
            rb.detectCollisions = true;
            MyJoystick.gameObject.SetActive(true);
            transform.GetComponent<BoxCollider>().enabled = true;
            Elindebirivar = false;
            if (Canvas.transform.GetComponent<UIScript>().Tutorial.activeSelf)
            {
                Canvas.transform.GetComponent<UIScript>().Tutorial.SetActive(false);
            }
            if (tutoLevel==true)
            {
                StartRunFunc();
                tutoLevel = false;
            }
        }

        if (IsStart && IsStopTouch == false)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {//hareket kısmı
                translation = new Vector3(Input.GetAxis("Mouse X"), 0, 0) * Time.deltaTime * HorizontalSpeed;
                transform.Translate(translation, Space.World);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, SolSinir.transform.position.x, SagSinir.transform.position.x), transform.localPosition.y, transform.localPosition.z);
            }
#elif UNITY_IOS || UNITY_ANDROID
           if (Input.touchCount > 0)
           {
               touch = Input.GetTouch(0);
               if (touch.phase == TouchPhase.Moved)
               {
                   transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x + touch.deltaPosition.x * 0.01f, SolSinir.transform.position.x, SagSinir.transform.position.x), transform.localPosition.y, transform.localPosition.z);
               }
               else if (touch.phase == TouchPhase.Began)
               {
                   //save began touch 2d point
                   FirstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
               }
           }
#endif
        }
    }

    IEnumerator ObstacleTimer()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        IsStart = false;
        transform.GetComponent<Animator>().SetTrigger("FallBack");
        rb.velocity = Vector3.zero;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 5), 1);
        yield return new WaitForSeconds(1f);
        IsStart = true;
        transform.GetComponent<Animator>().SetTrigger("NoShoulder");
    }
    IEnumerator Finish()
    {
        transform.GetComponent<Animator>().SetTrigger("NoShoulder");
        IsStart = false;
        transform.DOMove(TrainInsıde.transform.position, 2.5f);
        transform.GetComponent<Animator>().SetTrigger("Finish");
        yield return new WaitForSeconds(1.5f);
        Trains.GetComponent<Animator>().SetTrigger("Finish");
        yield return new WaitForSeconds(0.5f);
        Camera.transform.parent = null;
        trainGoTime = true;
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        Canvas.transform.GetComponent<UIScript>().LevelCompleted();
    }
    IEnumerator Failed()
    {
        transform.GetComponent<Animator>().SetTrigger("ReleaseEnemy");
        transform.GetComponent<Animator>().SetTrigger("NoShoulder");
        IsStart = false;
        Trains.GetComponent<Animator>().SetTrigger("Finish");
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<Animator>().SetTrigger("LevelFailed");
        transform.GetComponent<BoxCollider>().enabled = false;
        rb.velocity = new Vector3(0, 0, -3);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        trainGoTime = true;
        Canvas.transform.GetComponent<UIScript>().LevelFailed();
    }

}
