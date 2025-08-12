using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Single_Combo_Config", menuName = "Combo/Single Combo Config")]
public class ComboConfig : ScriptableObject
{
    [Title("基础数据")] 
    [LabelText("动画名称")] public string clipName;
    [LabelText("冷却数据")] public float cdDuring;

    [Title("交互数据")] 
    [LabelText("连招配置")] public ComboInteractionConfig[] comboInteractionConfig;
    [LabelText("检测配置")] public AttackDetectionConfig[] attackDetectionConfig;

    [Header("表现数据")] 
    [LabelText("打击感配置")] public AttackFeedbackConfig[] attackFeedbackConfig;
    [LabelText("位移补偿-自身")] public MoveOffsetConfig[] myMoveOffsetConfig;
    [LabelText("位移补偿-敌人")] public MoveOffsetConfig[] enemyMoveOffsetConfig;
}

/// <summary>
/// 交互配置
/// </summary>
[Serializable]
public class ComboInteractionConfig
{
    public string[] hitName;
    public string[] hitAirName;
    
    // 武器类型
    // 攻击力度
    public float damge;
}

/// <summary>
/// 攻击检测配置
/// </summary>
[Serializable]
public class AttackDetectionConfig
{
    public float startTime;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;
}

/// <summary>
/// 打击感配置
/// </summary>
[Serializable]
public class AttackFeedbackConfig
{
    [BoxGroup("屏幕震动")]
    public float strength;          // 强度
    public float frequency;         // 频率
    public float duration;          // 持续
    
    // [BoxGroup("顿帧")]
}

/// <summary>
/// 位移补偿
/// </summary>
[Serializable]
public class MoveOffsetConfig
{
    public float startTime;
    public MoveOffsetDirection direction;
    public AnimationCurve animationCurve;
    public float scale;
    public float during;
}