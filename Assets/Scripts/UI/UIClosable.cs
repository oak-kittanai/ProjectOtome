using System;
using System.Collections.Generic;
using UnityEngine;

public class UIClosable : MonoBehaviour
{
    private readonly List<Action> cleanup = new();

    public void RegisterCleanup(Action cleanup)
    {
        this.cleanup.Add(cleanup);
    }

    public void Close()
    {
        for (int i = 0; i < cleanup.Count; i++)
        {
            cleanup[i]?.Invoke();
        }
        Destroy(gameObject);
    }
}
