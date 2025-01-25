using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer : MonoBehaviour
{
    private const int _maxPlayers = 2;
    private List<PlayerInput> _playerList = new List<PlayerInput>();
    private PlayerInputManager _playerInputManager;
    
    [SerializeField] private InputAction _joinAction;
    [SerializeField] private Color[] _playerColors = new Color[2];
    [SerializeField] private Transform[] _spawnLocation;
    
    private void Awake()
    {
        _playerColors[0] = Color.red;
        _playerColors[1] = Color.blue;
        
        _joinAction.Enable();
    }

    private void Start()
    {
        _playerList.Clear();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += OnJoin;
    }

    private void OnJoin(PlayerInput playerInput)
    {
        TryJoin(playerInput);
        AssignColor(playerInput);
        SpawnOnLocation(playerInput);
    }

    private void TryJoin(PlayerInput playerInput)
    {
        Camera playerCamera = GetComponentInChildren<Camera>();
        
        if (_playerList.Count < _maxPlayers)
        {
            if (_playerList.Contains(playerInput) == false)
            {
                _playerList.Add(playerInput);
            }

            if (playerInput.playerIndex == 0)
            {
                playerCamera.rect = new Rect(0, 0, 1, 1);
            }

            if (playerInput.playerIndex == 1)
            {
                playerCamera.rect = new Rect(0, 0.5f, 1, 1);
            }
        }
    }

    private void SpawnOnLocation(PlayerInput playerInput)
    {
        playerInput.transform.position = _spawnLocation[_playerList.IndexOf(playerInput)].position;
    }

    private void AssignColor(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;

        if (playerIndex >= 0 && playerIndex < _playerColors.Length)
        {
            if (playerInput.TryGetComponent(out Renderer renderer))
            {
                renderer.material.SetColor("_BaseColor", _playerColors[playerIndex]);
            }
            else
            {
                Debug.LogError($"Player {playerInput.playerIndex} does not have a renderer");
            }
        }
    }
}
