using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class MoveController : MonoBehaviour
{
    [Title("角色数据")] [LabelText("角色属性数据")]
    public SOAttribute soAttribute;
    
    [LabelText("角色动画数据")]
    public SOAnimation soAnimation;
        
    private InputService m_inputService;
    private InputAction m_moveAction;

    private void Awake()
    {
        m_inputService = InputService.Instance;
        m_moveAction = InputService.Instance.inputSystem.Player.Move;
    }

    private void OnEnable()
    {
        m_inputService.onMovePerformed.AddListener(OnMovePerformed);
    }

    private void OnDisable()
    {
        m_inputService.onMovePerformed.RemoveListener(OnMovePerformed);
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        UpdateMoveDirty();
    }

    private void UpdateMoveDirty()
    {
        if (m_moveAction.IsInProgress())
        {
            Vector2 direction = m_moveAction.ReadValue<Vector2>();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
    }
}
