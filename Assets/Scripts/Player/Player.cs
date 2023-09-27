using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	#region PublicVariables
	public static Player instance;
	#endregion

	#region PrivateVariables
	private PlayerInput input;
	private PlayerMove move;
	private PlayerAim aim;
	private PlayerDash dash;
	private PlayerWarp warp;
	private PlayerBulletTime bulletTime;
	private PlayerHp hp;
	private PlayerDeadEye deadEye;
	private Body body;
	private Bow bow;

	private ParticleSystem dustTrail;

	[SerializeField] private float invincibleTime = 1f;
	[SerializeField][ReadOnly] private bool canAct = true;
	[SerializeField][ReadOnly] private bool isInvincible = false;
	#endregion

	#region PublicMethod
	[Button]
	public void Initialize()
	{
		hp.Initialize();
		deadEye.Initialize();
		ForceQuit();
	}
	public void SetActive(bool b)
	{
		canAct = b;
	}
	public void ForceQuit()
	{
		dash.ForceQuit();
		warp.ForceQuit();
		bulletTime.ForceQuit();
	}
	public void SetInvincibility(bool b)
	{
		isInvincible = b;
	}
	public void SetPosition(Vector2 _position)
	{
		dustTrail.Stop();
		transform.position = _position;
		dustTrail.Play();
	}
	[Button]
	public void Hit(float _amount)
	{
		if (isInvincible == true)
			return;
		hp.ChangeValue(_amount);
		body.StartFlickering(invincibleTime);
		bow.StartFlickering(invincibleTime);
		SetInvincibility(true);
		Invoke(nameof(RemoveInvincibility), invincibleTime);
	}
	public void Die()
	{
		MapSpawinTrigger.instance.SpawnPlayer();
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		input = new PlayerInput();
		TryGetComponent(out aim);
		aim.Initialize();
		TryGetComponent(out move);
		move.Initialize();
		TryGetComponent(out dash);
		dash.Initialize();
		TryGetComponent(out warp);
		warp.Initialize();
		TryGetComponent(out bulletTime);
		bulletTime.Initialize();
		TryGetComponent(out hp);
		TryGetComponent(out deadEye);
		Initialize();
		transform.Find("Renderer").TryGetComponent(out body);
		transform.Find("Bow").TryGetComponent(out bow);
		transform.Find("Dust Trail").TryGetComponent(out dustTrail);
	}
	private void OnEnable()
	{
		input.Enable();
		input.Player.Move.performed += OnMovePerformed;
		input.Player.Move.canceled += OnMoveCanceled;
		input.Player.Attack.performed += OnAttackPerformed;
		input.Player.Attack.canceled += OnAttackCanceled;
		input.Player.Dash.performed += OnDashPerformed;
		input.Player.Warp.performed += OnWarpPerformed;
		input.Player.Warp.canceled += OnWarpCanceled;
		input.Player.BulletTime.performed += OnBulletTimePerformed;
		input.Player.BulletTime.canceled += OnBulletTimeCanceled;
	}
	private void OnDisable()
	{
		input.Player.Move.performed -= OnMovePerformed;
		input.Player.Move.canceled -= OnMoveCanceled;
		input.Player.Attack.performed -= OnAttackPerformed;
		input.Player.Attack.canceled -= OnAttackCanceled;
		input.Player.Dash.performed -= OnDashPerformed;
		input.Player.Warp.performed -= OnWarpPerformed;
		input.Player.Warp.canceled -= OnWarpCanceled;
		input.Player.BulletTime.performed -= OnBulletTimePerformed;
		input.Player.BulletTime.canceled -= OnBulletTimeCanceled;
		input.Disable();
	}
	private void Update()
	{
		bulletTime.HandleInput();
		if (canAct == true)
		{
			aim.HandleInput();
			move.HandleInput();
			warp.HandleInput();
		}
	}
	private void OnMovePerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		move.Move(_context.ReadValue<Vector2>());
	}
	private void OnMoveCanceled(InputAction.CallbackContext _context)
	{
		move.Stop();
	}
	private void OnAttackPerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		aim.OpenFire();
	}
	private void OnAttackCanceled(InputAction.CallbackContext _context)
	{
		aim.HoldFire();
	}
	private void OnDashPerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		dash.Dash();
	}
	private void OnWarpPerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		warp.OnActionPerformed();
	}
	private void OnWarpCanceled(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		warp.OnActionCanceled();
	}
	private void OnBulletTimePerformed(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		bulletTime.OnActionPerformed();
	}
	private void OnBulletTimeCanceled(InputAction.CallbackContext _context)
	{
		bulletTime.OnActionCanceled();
	}
	private void RemoveInvincibility()
	{
		SetInvincibility(false);
	}

    private void OnParticleCollision(GameObject other)
    {
		Hit(-1);
    }
    #endregion
}
