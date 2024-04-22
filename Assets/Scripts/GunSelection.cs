using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelection : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    public PlayerPreferences playerPref;

    public WeaponInfo weaponInfo;

    public float speed;

    public GameObject[] guns;

    [Header("Selection")]
    public int currentSelection;
    public int nextSelection;
    public int previousSelection;

    [Header("Selected Gun")]
    public GameObject gunSelected;
    public GunData gunData;

    [Header("Position to Move")]
    public int selectionToMove;
    public Vector3 posToCam;
    public Vector3 posAwayCam;
    public Vector3 distanceToMove;
    

    bool canMove = false;

    void Start()
    {
        // Get the current selection from the game data (0 by default)
        currentSelection = playerPref.LoadPrefs("Gun");

        // Add enough elements for the list of guns to be instantiated into later
        guns = new GameObject[gameData.gunPrefabs.Length];
        
        SpawnGuns();
        UpdateSelectedGun();
        weaponInfo.displayInfo(gunSelected);
        gunSelected.transform.localPosition = gunData.localPositionToCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Previous();
        }

        if (canMove) {
            MoveGunPosition(currentSelection, posToCam);
            MoveGunPosition(selectionToMove, posAwayCam);
        }
    }

    void SpawnGuns()
    {
        for (int i = 0; i < gameData.gunPrefabs.Length; i++) {
            GameObject gun = Instantiate(gameData.gunPrefabs[i], transform);

            // Disable the gun scripts as we won't be using it to prevent errors
            gun.GetComponent<Gun>().enabled = false;
            gun.GetComponent<WeaponSway>().enabled = false;
            gun.layer = LayerMask.NameToLayer("Ignore Post Processing");

            // Insert the gun into the guns list
            guns[i] = gun;
        }
    }

    void UpdateSelectedGun()
    {
        gunSelected = guns[currentSelection];
        gunData = gunSelected.GetComponent<Gun>().gunData;

        nextSelection = currentSelection + 1 == gameData.gunPrefabs.Length ? 0 : currentSelection + 1;
        previousSelection = currentSelection - 1 < 0 ? gameData.gunPrefabs.Length - 1 : currentSelection - 1;
    }

    public void Next()
    {
        currentSelection = nextSelection;
        UpdateSelectedGun();

        // After updating the selected gun, the old 'currentSelection' has become the new 'previousSelection'
        // Therefore, we need to move the old selection (which is now the new previousSelection) outside the screen 
        // and the new selection (the new currentSelection) into the screen
        selectionToMove = previousSelection;

        // Set the starting position for the new currentSelection gun
        gunSelected.transform.localPosition = gunData.localPositionToCamera - distanceToMove; 

        // Set the end position for the old currentSelection(aka the new previousSelection) gun
        posAwayCam = GetNewPosFromDis(selectionToMove, distanceToMove);

        OnSelect();
    }

    public void Previous()
    {
        currentSelection = previousSelection;
        UpdateSelectedGun();

        // Vice versa when selecting from the opposite order
        selectionToMove = nextSelection;
 
        gunSelected.transform.localPosition = gunData.localPositionToCamera + distanceToMove; 

        posAwayCam = GetNewPosFromDis(selectionToMove, -distanceToMove);

        OnSelect();
    }

    void OnSelect()
    {
        weaponInfo.displayInfo(gunSelected);
        
        posToCam = gunData.localPositionToCamera;

        if (!canMove) canMove = true;
    }

    Vector3 GetNewPosFromDis(int index, Vector3 distance)
    {   
        Vector3 newPos = guns[index].transform.localPosition + distance;

        return newPos;
    }

    void MoveGunPosition(int index, Vector3 newPos)
    {
        guns[index].transform.localPosition = Vector3.Lerp(guns[index].transform.localPosition, newPos, Time.deltaTime * speed);
    }


    public void Confirm()
    {
        // Save the new current selection into the player preferences
        playerPref.SavePrefs("Gun", currentSelection);
    }

}
