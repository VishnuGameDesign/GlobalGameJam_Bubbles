using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer : MonoBehaviour
{
    private const int _maxPlayers = 2;
    private List<PlayerInput> _playerList = new List<PlayerInput>();

    [field: SerializeField] public PlayerInputManager PlayerInputManager { get; private set; }
    
    [SerializeField] private BoolEventAsset _allPlayersJoined;
    [SerializeField] private Color[] _playerColors = new Color[2];
    [SerializeField] private Transform[] _spawnLocation;

    private void OnValidate()
    {
        if (PlayerInputManager == null)
        {
            PlayerInputManager = GetComponent<PlayerInputManager>();
        }
    }

    private void Start()
    {
        _playerList.Clear();
        PlayerInputManager.onPlayerJoined += OnJoin;
    }

    private void OnJoin(PlayerInput playerInput)
    {
        TryJoin(playerInput);
        AssignColor(playerInput);
        SpawnOnLocation(playerInput);
    }

    private void TryJoin(PlayerInput playerInput)
    {
        if (_playerList.Count < _maxPlayers)
        {
            if (_playerList.Contains(playerInput) == false)
            {
                _playerList.Add(playerInput);
            }
        }

        if (_playerList.Count == _maxPlayers)
        {
            _allPlayersJoined.Invoke(true);
        }
    }

    private void SpawnOnLocation(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;
        if (index >= 0 && index < _spawnLocation.Length)
        {
            playerInput.transform.position = _spawnLocation[index].position;
        }
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
