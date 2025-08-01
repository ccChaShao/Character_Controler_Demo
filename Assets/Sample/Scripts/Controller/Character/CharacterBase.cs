using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator),typeof(CharacterController))]
public class CharacterBase : MonoBehaviour
{
    public Animator animator { get; private set; }
    public CharacterController controller { get; private set; }
    
    [Header("重力设置")]
    public float gravity = -12;
    public Vector2 velocityLimit = new Vector2(-20, 60);
    public LayerMask groundMask;
    public float groundDetectedOffset = -0.06f;
    public float groundRadius = 1.2f;
    
    
    // 地表检测信息
    [HideInInspector] public BindableProperty<bool> isOnGround = new ();
    [HideInInspector] public Vector3 detectedOrigin;

    // 角色速度
    [HideInInspector] public Vector3 verticalVelocity;           // 角色垂直速度
    [FormerlySerializedAs("horizontalVelocityInAir")] [HideInInspector] public Vector3 horizontalVelocity;        // 角色水平速度
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public Vector3 animationVelocity;
    
    // 角色移动方向
    [HideInInspector] public Vector3 moveDirection;

    [HideInInspector] public Vector3 animatorDeltaPositionOffset;
    [Range(0.1f, 10)] public float moveSpeedMult = 1;
    
    public bool applyFullRootMotion { get; set; } = false;
    public bool ignoreRootMotionY { get; set; } = false;            //忽视根运动的Y量
    public bool disableGravity { get; set; } = false;               //是否禁用程序重力

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
    }

    #region 角色重力处理

    private bool CheckOnGround()
    {
        detectedOrigin = transform.position - groundDetectedOffset * Vector3.up;
        bool isHit = Physics.CheckSphere(detectedOrigin, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
        isOnGround.Value = isHit & verticalSpeed < 0;
        return isOnGround.Value;
    }
    
    private void CharacterGravity()
    {
        if (disableGravity)
        {
            return;
        }
        if (isOnGround.Value)
        {
            verticalSpeed = -2;
        }
        else
        {
            verticalSpeed += Time.deltaTime * gravity;
            verticalSpeed = Mathf.Clamp(verticalSpeed, velocityLimit.x, velocityLimit.y);
        }
        verticalVelocity = new Vector3(0, verticalSpeed, 0);
    }

    #endregion

    #region 角色移动
    
    private void ResetHorizontalVelocity()
    {
        if (isOnGround.Value)
        {
            horizontalVelocity = Vector3.zero;
        }
    }
    
    private void CharacterVerticalVelocity()
    {
        if (disableGravity)
        {
            verticalVelocity = Vector3.zero;
        }
        if (controller.enabled)
        {
            controller.Move((verticalVelocity + horizontalVelocity) * Time.deltaTime);
        }
    }
    
    //在播放动画时调用次方法,没有动画不会执行
    protected virtual void OnAnimatorMove()     
    {
        //开启角色的根运动，重力默认为角色自带的向下的动画位移量
        if (applyFullRootMotion)                
        {
            animator.ApplyBuiltinRootMotion();
        }
        //不启用根运动，但是采样的也是角色根运动信息(位移)
        else
        {
            Vector3 animationMovement = animator.deltaPosition + animatorDeltaPositionOffset;
            if (ignoreRootMotionY)
            {
                animationMovement.y = 0;
            }
            moveDirection = SetDirOnSlop(animationMovement) * moveSpeedMult;
            UpdateCharacterMove(moveDirection, animator.deltaRotation);
        }
    }

    public void UpdateCharacterMove(Vector3 deltaDir, Quaternion deltaRotation)
    {
        if (deltaRotation != Quaternion.identity)
        {
            transform.rotation = deltaRotation * transform.rotation;
        }
        //每帧移动Dir个单位
        if (controller.enabled)
        {
            animationVelocity = deltaDir;
            controller.Move(deltaDir);
        }
      
    }
    public float SetVerticalSpeed(float val)
    {
        return verticalSpeed = val;
    }
    
    public Vector3 SetHorizontalVelocity(Vector3 val)
    {
        return horizontalVelocity = new Vector3(val.x, 0, val.z);
    }

    #endregion

    #region 斜坡的处理
    private Vector3 SetDirOnSlop(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1))
        {
            if (Vector3.Dot(hitInfo.normal, Vector3.up) != 1)
            {
                return Vector3.ProjectOnPlane(dir, hitInfo.normal);
            }
        }
        return dir;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (CheckOnGround())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position - groundDetectedOffset * Vector3.up, groundRadius);
    }
}
