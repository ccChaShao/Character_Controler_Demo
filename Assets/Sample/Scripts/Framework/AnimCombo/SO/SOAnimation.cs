using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Animation", menuName = "Framework/SO Animation/Character Animation")]
public class SOAnimation : ScriptableObject
{
    [Title("控制动画数据")] 
    [LabelText("待机")] public string idleClip;
    
    [LabelText("移动")] public string movementClip;
}
