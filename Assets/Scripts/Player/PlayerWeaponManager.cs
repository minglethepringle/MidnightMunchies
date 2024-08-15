using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{

    public static GameObject pistol;

    public static GameObject assaultRifle;

    public static GameObject shotgun;

    public static GameObject rocketLauncher;

    public static GameObject currentWeapon;

    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        AssignWeapons();
        SwitchToPistol();
    }

    void AssignWeapons()
    {
        mainCamera = Camera.main.gameObject;
        pistol = mainCamera.transform.Find("Pistol").gameObject;
        assaultRifle = mainCamera.transform.Find("Assault Rifle").gameObject;
        shotgun = mainCamera.transform.Find("Shotgun").gameObject;
        rocketLauncher = mainCamera.transform.Find("Rocket Launcher").gameObject;
    }

    public static void SwitchToPistol()
    {
        pistol.SetActive(true);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
        rocketLauncher.SetActive(false);

        currentWeapon = pistol;
    }

    public static void SwitchToAssaultRifle()
    {
        assaultRifle.SetActive(true);
        currentWeapon = assaultRifle;
        currentWeapon.SetActive(true);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        rocketLauncher.SetActive(false);
    }

    public static void SwitchToShotgun()
    {
        shotgun.SetActive(true);
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        rocketLauncher.SetActive(false);

        currentWeapon = shotgun;
    }

    public static void SwitchToRocketLauncher()
    {
        rocketLauncher.SetActive(true);
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
        currentWeapon = rocketLauncher;
    }

    public static float CurrentWeaponDamage()
    {
        switch (currentWeapon.name)
        {
            case "Pistol":
                return 10f;
            case "AssaultRifle":
                return 20f;
            case "Shotgun":
                return 50f;
            case "Rocket Launcher":
                return 500f;
            case "Grenades":
                return 100f;
            case "Airstrikes":
                return 1000f;
            default:
                return 0f;
        }
    }
}
