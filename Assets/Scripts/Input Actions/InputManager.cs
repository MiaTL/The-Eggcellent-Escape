using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool EscSeqInput { get; private set; }
    private PlayerInput _playerInput;
    private InputAction _escSeq;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _playerInput = GetComponent<PlayerInput>();
        _escSeq = _playerInput.actions["EscapeSequence"];

    }

    private void Update()
    {
        EscSeqInput = _escSeq.WasPressedThisFrame();
     }
}
