using System.Collections;
using UnityEngine;

public class PlayerAbilityInvisible : MonoBehaviour
{
	[SerializeField] private Collider ability;
	[SerializeField] private GameObject playerModel;
	//[SerializeField] private GameObject playerShadow;
	[SerializeField] private Material abilityMaterial;
	[SerializeField] private float abilityActiveTime = 5;
	[SerializeField] private float abilityCooldownTime = 30;


	public delegate void AbilityActiveTimeRemainsEventHandler(float activeTime);
	public delegate void AbilityCooldownTimeRemainsEventHandler(float cooldownTime);
	public delegate void AbilityReadyEventHandler();

	public static event AbilityActiveTimeRemainsEventHandler AbilityActiveTimeRemainsEvent;
	public static event AbilityCooldownTimeRemainsEventHandler AbilityCooldownTimeRemainsEvent;
	public static event AbilityReadyEventHandler AbilityReadyEvent;


	private Material defaultPlayerMaterial;
	private Material defaultWeaponMaterial;
	private Renderer[] playerRenderer;
	//private Renderer playerShadowRenderer;
	private PlayerWeaponVer2 playerWeapon;

	private float abilityActiveTimeRemains;
	private float abilityCooldownTimeRemains;
	private bool firstUpdate = true;

	private void OnEnable()
	{
		abilityActiveTimeRemains = 0;
		abilityCooldownTimeRemains = 0;
		AbilityReadyEvent?.Invoke();
	}

	private void OnDisable()
	{
		DisableAbiblity();
	}

	private void Start()
	{
		playerRenderer = playerModel.GetComponentsInChildren<Renderer>();
		//playerShadowRenderer = playerShadow.GetComponent<Renderer>();
		defaultPlayerMaterial = playerRenderer[0].sharedMaterial;
		AbilityReadyEvent?.Invoke();
	}

	private void Update()
	{
		if (firstUpdate)
		{
			firstUpdate = false;
			playerWeapon = PlayerList.PL.PlayersList[0].playerWeapon;
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			TryEnableAbility();
		}
	}

	private void TryEnableAbility()
	{
		if (abilityCooldownTimeRemains > 0 || abilityActiveTimeRemains > 0)
			return;

		EnableAbiblity();
	}

	private IEnumerator ActiveAbilityTime()
	{
		abilityActiveTimeRemains = abilityActiveTime;

		while (abilityActiveTimeRemains > 0)
		{
			abilityActiveTimeRemains -= Time.deltaTime;

			if (abilityActiveTimeRemains < 0)
			{
				abilityActiveTimeRemains = 0;
			}

			AbilityActiveTimeRemainsEvent?.Invoke(abilityActiveTimeRemains);
			yield return new WaitForEndOfFrame();
		}

		DisableAbiblity();
	}

	private IEnumerator AbilityCooldownTime()
	{
		abilityActiveTimeRemains = 0;
		abilityCooldownTimeRemains = abilityCooldownTime;

		while (abilityCooldownTimeRemains > 0)
		{
			abilityCooldownTimeRemains -= Time.deltaTime;

			if (abilityCooldownTimeRemains < 0)
			{
				abilityCooldownTimeRemains = 0;
			}

			AbilityCooldownTimeRemainsEvent?.Invoke(abilityCooldownTimeRemains);
			yield return new WaitForEndOfFrame();
		}

		AbilityReadyEvent?.Invoke();
	}

	private void DisableAbiblity()
	{
		if (!ability.enabled)
			return;

		StopAllCoroutines();
		StartCoroutine(AbilityCooldownTime());

		ability.enabled = false;
		//playerRenderer.sharedMaterial = defaultPlayerMaterial;
		//playerShadowRenderer.sharedMaterial = defaultPlayerMaterial;
		DisablePlayerAbilityMaterial();
		DisableWeaponAbilityMaterial();

		PlayerWeaponVer2.SwitchStartEvent -= DisableWeaponAbilityMaterial;
		PlayerWeaponVer2.SwitchEndEvent -= EnableWeaponAbilityMaterial;
		WeaponVer2.FireEvent -= DisableAbiblity;
	}

	private void EnableAbiblity()
	{
		StartCoroutine(ActiveAbilityTime());

		ability.enabled = true;
		//playerRenderer.sharedMaterial = abilityMaterial;
		//playerShadowRenderer.sharedMaterial = abilityMaterial;
		EnablePlayerAbilityMaterial();
		EnableWeaponAbilityMaterial();

		PlayerWeaponVer2.SwitchStartEvent += DisableWeaponAbilityMaterial;
		PlayerWeaponVer2.SwitchEndEvent += EnableWeaponAbilityMaterial;
		WeaponVer2.FireEvent += DisableAbiblity;
	}

	private void EnableWeaponAbilityMaterial()
	{
		defaultWeaponMaterial = playerWeapon.GetCurrentWeapon().GetComponentInChildren<Renderer>().sharedMaterial;

		foreach (Renderer r in playerWeapon.GetCurrentWeapon().GetComponentsInChildren<Renderer>())
		{
			r.sharedMaterial = abilityMaterial;
		}
	}

	private void DisableWeaponAbilityMaterial()
	{
		foreach (Renderer r in playerWeapon.GetCurrentWeapon().GetComponentsInChildren<Renderer>())
		{
			r.sharedMaterial = defaultWeaponMaterial;
		}
	}

	private void EnablePlayerAbilityMaterial()
	{
		foreach (Renderer r in playerRenderer)
		{
			r.sharedMaterial = abilityMaterial;
		}
	}

	private void DisablePlayerAbilityMaterial()
	{
		foreach (Renderer r in playerRenderer)
		{
			r.sharedMaterial = defaultPlayerMaterial;
		}
	}
}
