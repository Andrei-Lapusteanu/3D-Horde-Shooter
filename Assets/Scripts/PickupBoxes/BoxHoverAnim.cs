using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHoverAnim : MonoBehaviour
{
    private float hoverHeightMin;
    private float hoverHeightMax;
    private float hoverMidPoint;
    private float normRange;
    private float hoverRange = 0.3f;
    public float hoverSpeed;

    private void Start()
    {
        hoverHeightMin = transform.position.y;
        hoverHeightMax = transform.position.y + hoverRange;
        hoverMidPoint = (hoverHeightMin + hoverHeightMax) / 2.0f;
        normRange = (hoverHeightMax - hoverHeightMin) / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            hoverMidPoint + Mathf.Cos(Time.time * hoverSpeed) * normRange,
            transform.position.z);
    }
}
