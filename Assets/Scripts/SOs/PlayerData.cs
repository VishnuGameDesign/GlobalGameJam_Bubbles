using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public float RunSpeed { get; private set; } = 10f;
    [field: SerializeField] public float TurnSpeed { get; private set; } = 1f;
    [field: SerializeField] public float JumpHeight { get; private set; } = 5f;
    [field: SerializeField] public float GroundCheckMaxHeight { get; private set; } = 5f;
    [field: SerializeField] public float RespawnZDistance { get; private set; } = 5f;
    [field: SerializeField] public float RespawnTime { get; private set; } = 5f;
    [field: SerializeField] public int MaxJumps { get; private set; } = 2;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
}
