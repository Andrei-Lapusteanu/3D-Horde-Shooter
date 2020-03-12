using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextUpdate : MonoBehaviour
{
    TextMeshProUGUI hpText;
    TextMeshProUGUI clipText;
    TextMeshProUGUI leftoverBulletsText;
    TextMeshProUGUI killsText;

    PlayerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        hpText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("ClipText").GetComponent<TextMeshProUGUI>();
        leftoverBulletsText = GameObject.Find("LeftoverBulletsText").GetComponent<TextMeshProUGUI>();
        killsText = GameObject.Find("KillsText").GetComponent<TextMeshProUGUI>();

        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject dummy = GameObject.Find("HealthText");

        hpText.text = playerCtrl.GetHealthPoints().ToString();
        clipText.text = playerCtrl.GetClipCount().ToString();
        leftoverBulletsText.text = playerCtrl.GetLeftoverBulletsCount().ToString();
        killsText.text = playerCtrl.GetKillCount().ToString();
    }
}
