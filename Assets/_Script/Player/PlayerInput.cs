using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jumping;
    [SerializeField] private InputActionReference blocking;
    
    public bool isMoving => movement.action.IsPressed();
    public bool isBlocking => blocking.action.IsPressed();
    
    public bool AttackPressed { get; private set; }
    public bool JumpPressed { get; private set; }

    // Update is called once per frame
    void Update()
    {
        AttackPressed = attackAction.action.WasPressedThisFrame();
        JumpPressed = jumping.action.WasPressedThisFrame();
    }
}
