using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
	[SerializeField]
	private CameraState thisState;
	private Transform playerPos = null;

	private GameManager gameManager;

	private void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	public CameraState getState() { return thisState; }

	public void setState(CameraState state)
	{
		thisState = state;

		if (state == CameraState.CAUGHT_PLAYER)
		{
			gameManager.GameOver();
		}
		else if (state == CameraState.PATROLLING)
		{
			gameManager.resetDangerFill();
		}
	}

	public void setPlayerPos(Transform p) { playerPos = p; }

	public Transform getPlayerPos() { return playerPos; }
}

public enum CameraState
{
	PATROLLING = 1,
	DISCOVERED_PLAYER = 3,
	CAUGHT_PLAYER = 4,
}
