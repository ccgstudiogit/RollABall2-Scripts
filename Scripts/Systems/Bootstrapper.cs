using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Execute()
    {
        Debug.Log("Loaded by the Persist Objects from the Bootstrapper script");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistentObjects")));
    }
}
