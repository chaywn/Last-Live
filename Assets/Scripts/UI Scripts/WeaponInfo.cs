using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponInfo : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void displayInfo(GameObject weapon)
    {
        string name = weapon.name;
        name = name.Remove(name.Length-7);
        
        GunData gunData = weapon.GetComponent<Gun>().gunData;
        float firerate = gunData.fireRate;
        int magSize = gunData.magSize;

        text.text = name + "\nFirerate: " + firerate + "\nMagazine: " + magSize;
    }
}
