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
    [field: SerializeField] public BoolEventAsset OnCollided { get; private set; }
    [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }
    
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
        if(MeshRenderer == null) MeshRenderer = GetComponent<MeshRenderer>();
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            readyToRace = true;
        }
        
        if (Camera.main != null)
        {
            Vector3 right = PlayerCam.transform.right;
            Vector3 up = Vector3.up;
            _forwardVector = Vector3.Cross(right, up);
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
        _isGrounded = CheckGrounded();
        
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
    }
    
    private bool CheckGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * PlayerData.GroundCheckMaxHeight, Color.red); // DEBUG
        
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, PlayerData.GroundCheckMaxHeight,
            PlayerData.GroundLayer);
    }
    #endregion
    
}
