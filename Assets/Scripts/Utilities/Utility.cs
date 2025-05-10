using System;
using System.Collections;
using UnityEngine;

public static class Utility
{
    private static MonoBehaviour monoBehaviour;

    private const string PREFIX_DIALOGUE = "dialogues";

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

    public static void LoadDialogueData(string id, Action<TextAsset> onLoad)
    {
        LoadResource($"{PREFIX_DIALOGUE}{id}.txt", onLoad);
    }
}
