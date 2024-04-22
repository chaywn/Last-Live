using UnityEngine;

[CreateAssetMenu(fileName="GunData", menuName="ScriptableObject/Gun", order=2)]
public class GunData : ScriptableObject
{
    [Header("Gun Selection Menu")]
    public Vector3 localPositionToCamera;

    [Header("Sounds")]
    public AudioClip[] audioClipArray;

    [Header("Shooting")]
    public Vector2 damage;
    [Tooltip("in RPM")]public float fireRate;
    public float range;

    [Header("Aming")]
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public float smoothing;

    [Header("Recoil")]
    public float gunRecoil;
    public Vector2 cameraRecoilConstraint;

    [Header("Reloading")]
    public int magSize;
    public float reloadDuration;
}
