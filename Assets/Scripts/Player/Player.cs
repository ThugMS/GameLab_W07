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
	#endregion

	#region PublicMethod
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
	}
	private void OnEnable()
	{
		input.Enable();
		input.Player.Move.performed += OnMovePerformed;
		input.Player.Move.canceled += OnMoveCanceled;
		input.Player.Attack.performed += OnAttackPerformed;
		input.Player.Attack.canceled += OnAttackCanceled;
	}
	private void OnDisable()
	{
		input.Player.Move.performed -= OnMovePerformed;
		input.Player.Move.canceled -= OnMoveCanceled;
		input.Player.Attack.performed -= OnAttackPerformed;
		input.Player.Attack.canceled -= OnAttackCanceled;
		input.Disable();
	}
	private void Update()
	{
		aim.HandleInput();
		move.HandleInput();
	}
	private void OnMovePerformed(InputAction.CallbackContext _context)
	{
		move.Move(_context.ReadValue<Vector2>());
	}
	private void OnMoveCanceled(InputAction.CallbackContext _context)
	{
		move.Stop();
	}
	private void OnAttackPerformed(InputAction.CallbackContext _context)
	{
		aim.OpenFire();
	}
	private void OnAttackCanceled(InputAction.CallbackContext _context)
	{
		aim.HoldFire();
	}
	#endregion
}
