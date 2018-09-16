using System;

public class ClassSingleton<T> where T : new() {

    public static readonly T Instance = new T(); 

    protected ClassSingleton(){}

}
