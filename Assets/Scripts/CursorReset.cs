using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorReset : MonoBehaviour
{
    void Start()
    {
        // Unlock the cursor back which was locked in the game scene and make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
