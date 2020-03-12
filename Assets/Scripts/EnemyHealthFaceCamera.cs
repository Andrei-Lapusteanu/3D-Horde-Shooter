using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthFaceCamera : MonoBehaviour
{
    private GameObject targetCamera;

    private void Start()
    {
        targetCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCameraPos = targetCamera.transform.position;
        float distanceToCam = (targetCameraPos - transform.position).magnitude;
        transform.LookAt(targetCameraPos);
    }
}
