using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Combo_List", menuName = "Combo/Combo List")]
public class ComboList : ScriptableObject
{
    [SerializeField, LabelText("招式cd"), Tooltip("ms")] private int m_cdDuring;
    [SerializeField, LabelText("招式列表")] private ComboConfig[] m_comboList;
    
    public int cdDuring => m_cdDuring;
    
    public int comboListCount => m_comboList.Length;

    public ComboConfig TryGetComboConfig(int index)
    {
        if (index >= m_comboList.Length)
        {
            return null;
        }

        return m_comboList[index];
    }
}
