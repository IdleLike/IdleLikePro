using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton <T>: MonoBehaviour  where T : MonoBehaviour{

    private static T m_Instance;

    public static T instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();
                if(m_Instance == null)
                {
                    m_Instance = new GameObject().AddComponent<T>();  
                }
            }
            return m_Instance;
        }
    }
}
