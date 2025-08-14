using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Animator))]
public class MoveController : MonoBehaviour
{
    [Title("角色数据")] [LabelText("角色属性数据"), SerializeField]
    private SOAttribute soAttribute;
    
    [LabelText("角色动画数据"), SerializeField]
    private SOAnimation soAnim;
    
    [Title("角色状态")] [LabelText("移动开关"), SerializeField]
    private bool m_EnableMove = true;
        
    private InputService m_inputService;
    private InputAction m_moveAction;
    private Animator m_animator;
    private Camera m_camera;
    private bool m_isMoveDirty = false;

    public bool enableMove
    {
        get => m_EnableMove;
        set => m_EnableMove = value;
    }

    private void Awake()
    {
        m_camera = Camera.main;
        m_animator = GetComponent<Animator>();
        m_inputService = InputService.Instance;
        m_moveAction = InputService.Instance.inputSystem.Player.Move;
    }

    private void OnEnable()
    {
        m_inputService.onMovePerformed.AddListener(OnMovePerformed);
        m_inputService.onMoveCanceled.AddListener(OnMoveCanceled);
    }

    private void OnDisable()
    {
        m_inputService.onMovePerformed.RemoveListener(OnMovePerformed);
        m_inputService.onMoveCanceled.RemoveListener(OnMoveCanceled);
    }

    private void Update()
    {
        UpdateMoveDirty();
    }

    private void UpdateMoveDirty()
    {
        if (!m_isMoveDirty || !m_EnableMove)
        {
            return;
        }
        // 获取相机在水平面的前向和右向（忽略Y轴）
        Vector3 camForward = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(m_camera.transform.right, Vector3.up).normalized;
        // 输入方向
        Vector2 inputDirection = m_moveAction.ReadValue<Vector2>();
        // 基于相机的移动方向
        Vector3 moveDir = (inputDirection.y * camForward) + (inputDirection.x * camRight);
        if (moveDir != Vector3.zero) {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, soAttribute.rotateSpeed);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (m_EnableMove)
        {
            m_animator.CrossFadeInFixedTime(soAnim.movementClip, 0.155f, 0, 0);
        }
        m_isMoveDirty = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (m_EnableMove)
        {
            m_animator.CrossFadeInFixedTime(soAnim.idleClip, 0.155f, 0, 0);
        }
        m_isMoveDirty = false;
    }

    public void ClearMovement()
    {
        m_isMoveDirty = false;
    }
}
