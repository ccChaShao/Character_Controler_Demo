using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Combo List", menuName = "Framework/Combo/Combo List")]
public class SOComboList : ScriptableObject
{
    [SerializeField, LabelText("招式冷却")]
    private int m_cdDuring;

    [SerializeField, LabelText("招式间隙"), PropertyRange(1, 100)]
    private int m_gapDuring;

    [SerializeField, LabelText("招式列表")] 
    private SOComboConfig[] m_comboList;
    
    public int cdDuring => m_cdDuring * 1000;
    
    public int gapDuring => m_gapDuring * 1000;
    
    public int comboListCount => m_comboList.Length;

    public SOComboConfig TryGetComboConfig(int index)
    {
        if (index >= m_comboList.Length)
        {
            return null;
        }

        return m_comboList[index];
    }
}
