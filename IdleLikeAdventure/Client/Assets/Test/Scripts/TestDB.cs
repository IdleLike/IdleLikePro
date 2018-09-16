using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Entity;

[Serializable][CreateAssetMenu]
public class TestDB : ScriptableObject {

    private static TestDB instance;
    public static TestDB Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<TestDB>("TestDB");
            }
            return instance;
        }
    }


    public List<UserEntity> Users = new List<UserEntity>();

    public List<HeroEntity> Heros = new List<HeroEntity>();
}
