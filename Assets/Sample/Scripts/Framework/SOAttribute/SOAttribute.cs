using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Attribute", menuName = "Framework/SO Attribute/Character Attribute")]
public class SOAttribute : ScriptableObject
{
    [Title("基础属性")] 
    
    [LabelText("移动速度")] public float moveSpeed;
    
    [LabelText("朝向速度")] public float rotateSpeed;
}
