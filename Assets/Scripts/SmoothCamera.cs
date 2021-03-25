using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothCamera : MonoBehaviour
{
    public Transform Player;
    public float SmoothSpeed = 0.125f;

    public Vector3 Offset;

    private void FixedUpdate()
    {
              Vector3 DesiredPosition = Player.position + Offset;
              Vector3 SmoothedPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed * Time.deltaTime);
              transform.position = SmoothedPosition;
             // transform.LookAt(Player);
    }
   
  // public IEnumerator ShakeCamera(float duration, float magnitude)
  // {
  //     Vector3 originalPos = transform.position;
  //     float elapsed = 0.0f;
  //     while (elapsed<duration)
  //     {
  //         float x = Random.Range(-1, 1) * magnitude;
  //         float y = Random.Range(-1, 1) * magnitude;
  //         transform.localPosition = new Vector3(x, y, originalPos.z);
  //         elapsed += Time.deltaTime;
  //         yield return null;
  //     }
  //     transform.localPosition = originalPos;
  // }
}
