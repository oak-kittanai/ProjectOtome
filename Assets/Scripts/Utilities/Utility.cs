using System;
using System.Collections;
using UnityEngine;

public static class Utility
{
    private static MonoBehaviour monoBehaviour;

    public static void Setup(MonoBehaviour monoParam)
    {
        monoBehaviour = monoParam;
    }

    public static void LoadResource<T>(string path, Action<T> onLoad) where T : UnityEngine.Object
    {
        monoBehaviour.StartCoroutine(LoadCoroutine());
        IEnumerator LoadCoroutine()
        {
            ResourceRequest request = Resources.LoadAsync("glass");
            yield return request;
            onLoad?.Invoke(request.asset as T);
        }
    }
}
