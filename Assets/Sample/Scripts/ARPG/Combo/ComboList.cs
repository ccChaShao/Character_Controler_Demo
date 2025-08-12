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

    public ComboConfig TryGetComboConfig(int index)
    {
        if (index >= comboList.Length)
        {
            return null;
        }

        return comboList[index];
    }

    public string TryGetComboClipName(int index)
    {
        var config = TryGetComboConfig(index);
        if (!config)
        {
            return null;
        }

        return config.clipName;
    }
}
