using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public FirstPersonController playerController;

    AudioSource audioSource;

    float timeBetweenBullets;

    [Header("Aim")]
    public Crosshair crosshair;

    bool aiming;

    Ray ray;

    [Header("Ammo")]
    public int currentAmmo;
    public AmmoText ammoText;

    bool shooting = false;
    bool reloading = false;

    [Header("Recoil")]
    public bool cameraRecoil;

    // Start is called before the first frame update
    void Start()
    {
        playerController = Player.instance.GetComponent<FirstPersonController>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f;

        ammoText = GameObject.Find("AmmoText").GetComponent<AmmoText>();
        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();

        timeBetweenBullets = 1/(gunData.fireRate/60);
        currentAmmo = gunData.magSize;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (!reloading && !GamePause.gameIsPaused) {
            if (Input.GetMouseButton(0) && !shooting && currentAmmo>0) {
                shooting = true;
                StartCoroutine(Shoot());
            }
            else if (Input.GetMouseButtonDown(0) && currentAmmo==0) {
                StartCoroutine(ShootEmpty());
            }
            else if (Input.GetKeyDown(KeyCode.R) && currentAmmo<gunData.magSize) {
                reloading= true;
                StartCoroutine(Reload());
            }
        }

        ammoText.UpdateAmmo(reloading, currentAmmo, gunData.magSize);

        if (Physics.Raycast(ray, gunData.range, 1 << LayerMask.NameToLayer("Enemy"))) {
            crosshair.ColorRed();
        }
        else {
            crosshair.ColorWhite();
        }

        ToggleSprint();

        Debug.DrawRay(ray.origin, ray.direction * gunData.range, Color.green);
    }
 
    void Aim()
    {   
        Vector3 target;

        if (Input.GetMouseButton(1)) {
            aiming = true;
            target = gunData.aimingLocalPosition;
        }
        else {
            aiming = false;
            target = gunData.normalLocalPosition;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * gunData.smoothing);
    }

    IEnumerator Reload()
    {
        audioSource.clip = gunData.audioClipArray[1];
        audioSource.volume = 0.75f;
        audioSource.PlayDelayed(0.25f);

        yield return new WaitForSeconds(gunData.reloadDuration);
        audioSource.volume = 0.3f;
        currentAmmo = gunData.magSize;
        reloading = false;
    }

    IEnumerator Shoot() 
    {
        yield return new WaitForSeconds(timeBetweenBullets);

        audioSource.PlayOneShot(gunData.audioClipArray[0]);
        currentAmmo-- ;

        Recoil();

        if (Physics.Raycast(ray, out RaycastHit hit, gunData.range, 1 << LayerMask.NameToLayer("Enemy"))) {
            Zombie zombie = hit.transform.GetComponent<Zombie>();

            int damage = Mathf.RoundToInt(Random.Range(gunData.damage.x, gunData.damage.y));

            zombie.TakeDamage(damage);
        }

        shooting = false;
    }

    IEnumerator ShootEmpty()
    {
        // Playing part of a clip to act as an empty ammo sound
        audioSource.volume = 0.75f;
        audioSource.PlayOneShot(gunData.audioClipArray[2]);

        yield return new WaitForSeconds(0.127f);
        audioSource.Stop();
        audioSource.volume = 0.3f;
    }

    void Recoil()
    {
        // Gun recoil
        transform.localPosition -= Vector3.forward * gunData.gunRecoil;

        // Camera recoil
        if (cameraRecoil) {
            float xRecoil = Random.Range(-gunData.cameraRecoilConstraint.x, gunData.cameraRecoilConstraint.x);
            float yRecoil = Random.Range(-gunData.cameraRecoilConstraint.y, gunData.cameraRecoilConstraint.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

            transform.parent.localRotation = Quaternion.AngleAxis(-recoil.x, Vector3.right);
            transform.parent.localRotation = Quaternion.AngleAxis(recoil.y, Vector3.up);
        }
    }

    void ToggleSprint()
    {
        playerController.canRun = !(shooting || aiming);
    }
}
