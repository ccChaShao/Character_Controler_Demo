using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo_List", menuName = "Combo/Combo List")]
public class ComboList : ScriptableObject
{
    [SerializeField] private ComboConfig[] comboList;

    public int GetComboCount()
    {
        return comboList.Length;
    }
}
