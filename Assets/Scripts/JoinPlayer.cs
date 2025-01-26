using System;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer : MonoBehaviour
{
    private const int MaxPlayers = 2;
    private List<PlayerInput> _playerList = new List<PlayerInput>();

    [field: SerializeField] public PlayerInputManager PlayerInputManager { get; private set; }
    
    [SerializeField] private BoolEventAsset _allPlayersJoined;
    [SerializeField] private GameObject[] _playerModels = new GameObject[2];
    [SerializeField] private GameObject[] _playerUIModels = new GameObject[2];
    [SerializeField] private Transform[] _spawnLocation;
    
    private void OnValidate()
    {
        if (PlayerInputManager == null)
        {
            PlayerInputManager = GetComponent<PlayerInputManager>();
        }
    }

    private void Awake()
    {
        foreach (var uiModels in _playerUIModels)
        {
            uiModels.SetActive(false);
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
        AssignModel(playerInput);
        SpawnOnLocation(playerInput);
    }

    private void TryJoin(PlayerInput playerInput)
    {
        if (_playerList.Count < MaxPlayers)
        {
            if (_playerList.Contains(playerInput) == false)
            {
                _playerList.Add(playerInput);
            }
        }

        if (_playerList.Count == MaxPlayers)
        {
            _allPlayersJoined.Invoke(true);
        }
    }

    private void AssignModel(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        if (playerIndex >= 0 && playerIndex < _playerModels.Length)
        {
            if (playerInput.transform.childCount > 0)
            {
                Transform firstChild = playerInput.transform.GetChild(0);
                Destroy(firstChild.gameObject);
            }
            
            GameObject model = Instantiate(_playerModels[playerIndex], playerInput.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.SetParent(playerInput.transform);
        }
    }
    
    private void SpawnOnLocation(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;
        if (index >= 0 && index < _spawnLocation.Length)
        {
            if (_playerUIModels[index] != null)
            {
                _playerUIModels[index].SetActive(true);
            }
            playerInput.transform.position = _spawnLocation[index].position;
        }
    }

}
