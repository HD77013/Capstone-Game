using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jumping;
    [SerializeField] private InputActionReference blocking;
    
    public bool isMoving => isEnabled && movement.action.IsPressed();
    public bool isBlocking => isEnabled && blocking.action.IsPressed();

    public bool isEnabled = true;
    
    public bool AttackPressed { get; private set; }
    public bool JumpPressed { get; private set; }

    void OnEnable()
    {
        attackAction.action.Enable();
        movement.action.Enable();
        jumping.action.Enable();
        blocking.action.Enable();
    }

    void OnDisable()
    {
        attackAction.action.Disable();
        movement.action.Disable();
        jumping.action.Disable();
        blocking.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;
        
        AttackPressed = attackAction.action.WasPressedThisFrame();
        JumpPressed = jumping.action.WasPressedThisFrame();
    }
}
