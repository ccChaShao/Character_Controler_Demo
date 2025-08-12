using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindableProperty<T> 
{
    private Action<T> m_valueChanged;

    public Action<T> ValueChanged
    {
        get { return m_valueChanged; }
        set { m_valueChanged = value; }
    }
    
    private T m_value;
    
    public T Value
    {
        get { return m_value; }
        set
        {
            if (!Equals(m_value, value))
            {
                m_value = value;
                m_valueChanged?.Invoke(m_value);
            }
        }
    }
}
