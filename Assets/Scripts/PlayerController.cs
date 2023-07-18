using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public enum CharacterState
{
    Default,
    Crouching,
    Jumping,
    Flying,
}

public class PlayerController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;
    public Transform CameraTransform;
    public float movementSpeed = 5f;

    private CharacterState currentState = CharacterState.Default;
    private Vector3 moveInput;

    private void Awake()
    {
        Motor.CharacterController = this;
    }

    private void Update()
    {
        // Get player input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        // Handle state transitions
        switch (currentState)
        {
            case CharacterState.Default:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(CharacterState.Jumping);
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    TransitionToState(CharacterState.Crouching);
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    TransitionToState(CharacterState.Flying);
                }
                break;

            case CharacterState.Crouching:
                if (Input.GetKeyUp(KeyCode.C))
                {
                    TransitionToState(CharacterState.Default);
                }
                break;

            case CharacterState.Jumping:
                if (Motor.GroundingStatus.IsStableOnGround)
                {
                    TransitionToState(CharacterState.Default);
                }
                break;

            case CharacterState.Flying:
                if (Input.GetKeyUp(KeyCode.F))
                {
                    TransitionToState(CharacterState.Default);
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        // Apply movement based on the current state
        switch (currentState)
        {
            case CharacterState.Default:
                HandleDefaultMovement();
                break;

            case CharacterState.Crouching:
                HandleCrouchingMovement();
                break;

            case CharacterState.Jumping:
                HandleJumpingMovement();
                break;

            case CharacterState.Flying:
                HandleFlyingMovement();
                break;
        }
    }

    private void HandleDefaultMovement()
    {
        // Calculate move direction relative to the camera
        Vector3 cameraForward = Vector3.Scale(CameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = cameraForward * moveInput.z + CameraTransform.right * moveInput.x;

        // Calculate movement velocity
        Vector3 movementVelocity = moveDirection * movementSpeed;

        // Apply movement velocity to the motor
        Motor.BaseVelocity = movementVelocity;
    }


    private void HandleCrouchingMovement()
    {
        // Handle crouching movement logic here
    }

    private void HandleJumpingMovement()
    {
        // Handle jumping movement logic here
    }

    private void HandleFlyingMovement()
    {
        // Handle flying movement logic here
    }

    private void TransitionToState(CharacterState newState)
    {
        // Exit current state
        switch (currentState)
        {
            case CharacterState.Default:
                // Handle exit from Default state
                break;

            case CharacterState.Crouching:
                // Handle exit from Crouching state
                break;

            case CharacterState.Jumping:
                // Handle exit from Jumping state
                break;

            case CharacterState.Flying:
                // Handle exit from Flying state
                break;
        }

        // Enter new state
        switch (newState)
        {
            case CharacterState.Default:
                // Handle entry to Default state
                break;

            case CharacterState.Crouching:
                // Handle entry to Crouching state
                break;

            case CharacterState.Jumping:
                // Handle entry to Jumping state
                break;

            case CharacterState.Flying:
                // Handle entry to Flying state
                break;
        }

        currentState = newState;
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        // Implement if needed
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // Implement if needed
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // Implement if needed
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        // Implement if needed
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        // Implement if needed
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        // Implement if needed
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Implement if needed
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        // Implement if needed
    }

    public void AddVelocity(Vector3 velocity)
    {
        // Implement if needed
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        // Implement if needed
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        // Implement the method logic here
    }
}
