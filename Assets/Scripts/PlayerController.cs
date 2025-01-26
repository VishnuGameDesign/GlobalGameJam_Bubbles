using FMODUnity;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Camera PlayerCam { get; private set; }     
    [field: SerializeField] public PlayerData PlayerData { get; private set; }     
    [field: SerializeField] public BoolEventAsset CanRaceNow { get; private set; }
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    [SerializeField] private EventReference _jumpSFX;
    [SerializeField] private EventReference _runSFX;

    private Vector3 _forwardVector;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private bool _isJumpPressed;
    private int _jumpCounter;

    public bool canMove = true;
    public bool readyToRace;
    
    #region SETUP
    private void OnValidate()
    {
        if (Rigidbody == null) Rigidbody = GetComponent<Rigidbody>();
        if (PlayerCam == null) PlayerCam = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        CanRaceNow.OnInvoked.AddListener(BeginRace);
    }

    private void OnDisable()
    {
        CanRaceNow.OnInvoked.RemoveListener(BeginRace);
    }

    private void BeginRace(bool arg0)
    {
        readyToRace = true;
    }

    #endregion

    #region UPDATE
    private void Update()
    {
        Debug.Log(GroundLayer.ToString());
        if (Camera.main != null)
        {
            Vector3 right = PlayerCam.transform.right;
            Vector3 up = Vector3.up;
            _forwardVector = Vector3.Cross(right, up);
        }

        if (this.transform.position.y < -10)
        {
            transform.position = new Vector3(0, PlayerData.RespawnYDistance, PlayerData.RespawnZDistance);
        }
    }

    private void FixedUpdate()
    {
        if (canMove && readyToRace)
        {
            Vector3 movement = _forwardVector * (PlayerData.RunSpeed * Time.fixedDeltaTime);
            Rigidbody.MovePosition(transform.position + movement);
        }
        
        Vector3 rotation = Vector3.up * (_moveInput.x * PlayerData.TurnSpeed * Time.fixedDeltaTime); ;
        Rigidbody.transform.rotation = transform.rotation * Quaternion.Euler(rotation);
    }
    #endregion

    #region INPUTS
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    
    private void OnJump(InputValue value)
    {
        _isJumpPressed = value.isPressed;
        TryJump();
    }
    #endregion

    #region JUMP & GROUNDCHECK
    private void TryJump()
    {
        CheckGrounded();
        
        if (_isGrounded)
        {
            _jumpCounter = 0;
        }
        if (_isJumpPressed && _jumpCounter < PlayerData.MaxJumps)
        {
            Jump();
            _jumpCounter++;
        }
    }

    private void Jump()
    {
        Rigidbody.AddForce(Vector3.up * PlayerData.JumpHeight, ForceMode.Impulse);
        RuntimeManager.PlayOneShot(_jumpSFX, transform.position);
    }
    
    private void CheckGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * PlayerData.GroundCheckMaxHeight, Color.red); // DEBUG
        
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, PlayerData.GroundCheckMaxHeight, GroundLayer))
        {
            _isGrounded = true;
        }
        _isGrounded = false;
    }
    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out PlayerController otherPlayer))
        {
            Rigidbody otherRigidbody = otherPlayer.Rigidbody;
            otherRigidbody.AddForce(Vector3.up * PlayerData.PunchForce, ForceMode.Impulse);
        }
    }
}
