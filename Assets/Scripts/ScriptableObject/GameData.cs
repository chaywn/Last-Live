using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[CreateAssetMenu(fileName="GameData", menuName="ScriptableObject/Game", order=1)]
public class GameData : ScriptableObject
{
    public int waveSurvived;

    [Header("Gun Selection")]
    [SerializeField] private int _primaryGunIndex;

    public GameObject[] gunPrefabs;

    public int PrimaryGunIndex
    {
        get {
            return _primaryGunIndex;
        }
        set {
            _primaryGunIndex = value;
            #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
            #endif
        }
    } 
}
