using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponVer2 : MonoBehaviour
{
	[Header("Main Properties")]
	[SerializeField] private FireModes FireMode;
	[SerializeField] private PlayerWeaponVer2.AmmoType ammoType;
	[SerializeField] private int damage = 1;
	[SerializeField] private float fireRate;
	[SerializeField] private float accuracy;
	[SerializeField] private int bulletsCount = 1;
	[SerializeField] private int magazineCapacity;
	[SerializeField] private float reloadTime;
	[SerializeField] private float verticalRecoil;
	[SerializeField] private float horizontalRecoil;

	[Header("Description")]
	[SerializeField] private string description = "None";

	[Header("Audio")]
	[SerializeField] private AudioSource fireAudio;
	[SerializeField] private AudioSource reloadAudio;

	[Header ("Other objects refernces")]
	[SerializeField] private Transform endPoint;
	[SerializeField] private Bullet bullet;
	[SerializeField] private GameObject muzzle;
	[SerializeField] private GameObject weaponInHands;
	[SerializeField] private GameObject droppedWeapon;
	[SerializeField] private Light muzzleFlahsLight;


	public int Damage { get { return damage; } }
	public float FireRate { get { return fireRate; } }
	public float Accuracy { get { return accuracy; } }
	public int BulletsCount { get { return bulletsCount; } }
	public int MagazineCapacity { get { return magazineCapacity; } }
	public float ReloadTime { get { return reloadTime; } }
	public float Recoil { get { return verticalRecoil; } }
	public string Description { get { return description; } }


	public delegate void WeaponReadyEventHandler();
	public delegate void FireEventHandler();
	public delegate void ReloadStartEventHandler();
	public delegate void ReloadEndEventHandler();
	public delegate void ReloadAbortEventHandler();
	public delegate void BulletsCountChangedEventHundler(int bulletsInMagazine, int AllBullets, PlayerWeaponVer2.AmmoType ammoType);

	public static event WeaponReadyEventHandler WeaponReadyEvent;
	public static event FireEventHandler FireEvent;
	public static event ReloadStartEventHandler ReloadStartEvent;
	public static event ReloadEndEventHandler ReloadEndEvent;
	public static event ReloadAbortEventHandler ReloadAbortEvent;
	public static event BulletsCountChangedEventHundler BulletsCountChangedEvent;


	private PlayerWeaponVer2 PW;
	private PlayerLook PL;
	private Player player;

	private int bulletsInMagazine;
	private bool isDropped = false;
	private WeaponsStates weaponsState;

	public int BulletsInMagazine
	{
		get { return bulletsInMagazine; }
		set 
		{ 
			bulletsInMagazine = value;
			BulletsCountChangedEvent?.Invoke(bulletsInMagazine, PW.GetBulletsByType(ammoType), ammoType);
		}
	}

	public bool IsDropped
	{
		get { return isDropped; }
		set
		{
			isDropped = value;
			if (value)
			{
				weaponInHands.SetActive(false);
				droppedWeapon.SetActive(true);
			}
			else
			{
				weaponInHands.SetActive(true);
				droppedWeapon.SetActive(false);
			}
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

	private void Start()
	{
		if (isDropped)
			return;

		PW = PlayerWeaponVer2.PW;
		PL = PlayerLook.PL;
		player = PlayerList.PL.PlayersList[0];
		bulletsInMagazine = magazineCapacity;
	}

	private void OnEnable()
	{
		if (isDropped)
			return;

		muzzleFlahsLight.enabled = false;
		weaponsState = WeaponsStates.Switch;

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.01f);
			player.playerAnim.UpWeaponAnimation();
			BulletsInMagazine = BulletsInMagazine;

			yield return new WaitForSeconds(player.animator.GetCurrentAnimatorStateInfo(2).length);
			weaponsState = WeaponsStates.Idle;
			WeaponReadyEvent?.Invoke();
		}
	}

	private void OnDisable()
	{
		if (isDropped)
			return;

		StopAllCoroutines();
		muzzleFlahsLight.enabled = false;
		player.playerAnim.StopAnimation();
		ReloadAbortEvent?.Invoke();

		foreach(Transform t in endPoint)
		{
			Destroy(t.gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (Time.timeScale != 1 || isDropped)
			return;

		FireWeaponInput();
		ReloadWeaponInput();
	}

	private void FireWeaponInput()
	{
		if (FireMode == FireModes.Auto)
		{
			if (Input.GetKey(PlayerInput.Fire) || Input.GetKey(KeyCode.Joystick1Button5))
			{
				FireEvent?.Invoke();
				TryFire();
			}
		}

		if (FireMode == FireModes.Semi)
		{
			if (Input.GetKeyDown(PlayerInput.Fire) || Input.GetKeyDown(KeyCode.Joystick1Button5))
			{
				FireEvent?.Invoke();
				TryFire();
			}
		}
	}

	private void ReloadWeaponInput()
	{
		if (Input.GetKey(PlayerInput.Reload) || Input.GetKeyDown(KeyCode.Joystick1Button4))
		{
			TryReload();
		}
	}

	private void TryFire()
	{
		if (weaponsState != WeaponsStates.Idle || player.playerMove.isRunning)
			return;

		if (bulletsInMagazine <= 0)
		{
			TryReload();
			return;
		}

		Fire();
		weaponsState = WeaponsStates.Fire;
		BulletsInMagazine--;
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSeconds(1f / fireRate);
			weaponsState = WeaponsStates.Idle;

			if (bulletsInMagazine <= 0)
			{
				TryReload();
			}
		}
	}

	private void Fire()
	{
		PlayerAnim.PA.FireAnimation();

		fireAudio.pitch = Random.Range(0.99f, 1.01f);
		fireAudio.PlayOneShot(fireAudio.clip);

		for (int i = 0; i < bulletsCount; i++)
		{
			float randomX = Random.Range(-accuracy, accuracy);
			float randomY = Random.Range(-accuracy, accuracy);
			Quaternion rotation = Quaternion.Euler(endPoint.rotation.eulerAngles.x + randomX, endPoint.rotation.eulerAngles.y + randomY, endPoint.rotation.eulerAngles.z);
			Instantiate(bullet, endPoint.position, rotation).Damage = damage;
			MuzzleFlashLight();
		}

		Instantiate(muzzle, endPoint.position, endPoint.rotation, endPoint);
		PL.ShakeCameraByRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));
	}

	public void TryReload()
	{
		if (weaponsState == WeaponsStates.Switch || weaponsState == WeaponsStates.Reload || weaponsState == WeaponsStates.Fire || BulletsInMagazine == magazineCapacity || PW.GetBulletsByType(ammoType) == 0)
			return;

		ReloadStartEvent?.Invoke();
		weaponsState = WeaponsStates.Reload;

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			PlayerAnim.PA.ReloadAnimation();
			reloadAudio.Play();
			yield return new WaitForSeconds(reloadTime);
			Reload();
			weaponsState = WeaponsStates.Idle;
		}
	}

	private void Reload()
	{
		int bullets = PW.GetBulletsByType(ammoType) + bulletsInMagazine;
		int bulletsInMagazineTemp = magazineCapacity;
		bullets -= magazineCapacity;

		if(bullets < 0)
		{
			bulletsInMagazineTemp += bullets;
			//await Task.Run(() => PW.SetBulletByType(0, ammoType));
			PW[ammoType] = 0;
		}
		else
		{
			//await Task.Run(() => PW.SetBulletByType(bullets, ammoType));
			PW[ammoType] = bullets;
		}

		BulletsInMagazine = bulletsInMagazineTemp;
		ReloadEndEvent?.Invoke();
	}

	private void MuzzleFlashLight()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			muzzleFlahsLight.enabled = true;
			yield return new WaitForSeconds(0.1f);
			muzzleFlahsLight.enabled = false;
		}
	}

	public PlayerWeaponVer2.AmmoType GetAmmoType()
	{
		return ammoType;
	}

	public float GetRealodTime()
	{
		return reloadTime;
	}

	public int GetMagazineCapacity()
	{
		return magazineCapacity;
	}
}