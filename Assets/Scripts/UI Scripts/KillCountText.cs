using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCountText : MonoBehaviour
{

    TextMeshProUGUI text;
    int targetKillCount;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetTargetKill(int target)
    {
        targetKillCount = target;
    }

    public void Clear()
    {
        text.text = "Kills: - ";
    }

    public void UpdateKillCount()
    {
        text.text = "Kills: " + GameManager.zombieKilled + "/" + targetKillCount;
    }
}
