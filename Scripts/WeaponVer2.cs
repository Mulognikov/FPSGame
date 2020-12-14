using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponVer2 : MonoBehaviour
{
	[SerializeField] private FireModes FireMode;
	[SerializeField] private PlayerWeaponVer2.AmmoType ammoType;
	[SerializeField] private float fireRate;
	[SerializeField] private float accuracy;
	[SerializeField] private int magazineCapacity;
	[SerializeField] private float reloadTime;
	[SerializeField] private float verticalRecoil;
	[SerializeField] private float horizontalRecoil;
	[SerializeField] private Transform endPoint;
	[SerializeField] private GameObject bullet;
	[SerializeField] private GameObject muzzle;


	public delegate void FireEventHandler();
	public delegate void ReloadStartEventHandler();
	public delegate void ReloadEndEventHandler();
	public delegate void BulletsCountChangedHundler(int bulletsInMagazine, int AllBullets, PlayerWeaponVer2.AmmoType ammoType);

	public static event FireEventHandler FireEvent;
	public static event ReloadStartEventHandler ReloadStartEvent;
	public static event ReloadEndEventHandler ReloadEndEvent;
	public static event BulletsCountChangedHundler BulletsCountChanged;


	private PlayerWeaponVer2 PW;
	private PlayerInput PI;
	private PlayerLook PL;

	private int bulletsInMagazine;
	private WeaponsStates weaponsState;

	private int BulletsInMagazine
	{
		get { return bulletsInMagazine; }
		set 
		{ 
			bulletsInMagazine = value;
			BulletsCountChanged(bulletsInMagazine, PW.GetBulletsByType(ammoType), ammoType);
		}
	}

	private enum WeaponsStates
	{
		Idle,
		Fire,
		Reload,
		Switch
	}

	private enum FireModes
	{
		Semi,
		Auto
	}

	private void Awake()
	{

	}

	private void Start()
	{
		PI = PlayerInput.PI;
		PW = PlayerWeaponVer2.PlayerWeapon;
		PL = PlayerLook.PL;
		bulletsInMagazine = magazineCapacity;
	}

	private void OnEnable()
	{
		weaponsState = WeaponsStates.Switch;

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.01f);
			BulletsInMagazine = BulletsInMagazine;

			yield return null;
			weaponsState = WeaponsStates.Idle;
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void Update()
	{
		FireWeaponInput();
		ReloadWeaponInput();
	}

	private void FireWeaponInput()
	{
		if (FireMode == FireModes.Auto)
		{
			if (Input.GetKey(PI.Fire))
			{
				FireEvent?.Invoke();
				TryFire();
			}
		}

		if (FireMode == FireModes.Semi)
		{
			if (Input.GetKeyDown(PI.Fire))
			{
				FireEvent?.Invoke();
				TryFire();
			}
		}
	}

	private void ReloadWeaponInput()
	{
		if (Input.GetKey(PI.Reload))
		{
			ReloadStartEvent?.Invoke();
			TryReload();
		}
	}

	private void TryFire()
	{
		if (weaponsState != WeaponsStates.Idle || bulletsInMagazine <= 0)
			return;

		Fire();
		weaponsState = WeaponsStates.Fire;
		BulletsInMagazine--;
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSeconds(1 / fireRate);
			weaponsState = WeaponsStates.Idle;
		}
	}

	private void Fire()
	{
		float randomX = Random.Range(-accuracy, accuracy);
		float randomY = Random.Range(-accuracy, accuracy);
		Quaternion rotation = Quaternion.Euler(endPoint.rotation.eulerAngles.x + randomX, endPoint.rotation.eulerAngles.y + randomY, endPoint.rotation.eulerAngles.z);
		Instantiate(bullet, endPoint.position, rotation);
		Instantiate(muzzle, endPoint.position, endPoint.rotation, endPoint);
		PL.ShakeCameraByRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
	}

	public void TryReload()
	{
		if (weaponsState == WeaponsStates.Switch || weaponsState == WeaponsStates.Reload || BulletsInMagazine == magazineCapacity || PW.GetBulletsByType(ammoType) == 0)
			return;

		weaponsState = WeaponsStates.Reload;

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(reloadTime);
			Reload();
			weaponsState = WeaponsStates.Idle;
		}
	}

	private async void Reload()
	{
		int bullets = PW.GetBulletsByType(ammoType) + bulletsInMagazine;
		int bulletsInMagazineTemp = magazineCapacity;
		bullets -= magazineCapacity;

		if(bullets < 0)
		{
			bulletsInMagazineTemp += bullets;
			await Task.Run(() => PW.SetBulletByType(0, ammoType));
		}
		else
		{
			await Task.Run(() => PW.SetBulletByType(bullets, ammoType));
		}

		BulletsInMagazine = bulletsInMagazineTemp;
		ReloadEndEvent?.Invoke();
	}

	public PlayerWeaponVer2.AmmoType GetAmmoType()
	{
		return ammoType;
	}
}