using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    private static MonoBehaviour monoBehaviour;
    private static Dictionary<string, UnityEngine.Object> resourceCache = new();

    private const string PREFIX_ITEM = "items";
    private const string PREFIX_DIALOGUE = "dialogues";
    private const string PREFIX_UI = "ui";

    public static void Setup(MonoBehaviour monoParam)
    {
        monoBehaviour = monoParam;
    }

    public static void LoadResource<T>(string prefix, string path, Action<T> onLoad) where T : UnityEngine.Object
    {
        monoBehaviour.StartCoroutine(LoadCoroutine());
        IEnumerator LoadCoroutine()
        {
            if (resourceCache.TryGetValue(prefix + "/" + path, out UnityEngine.Object value))
            {
                onLoad?.Invoke(value as T);
                yield break;
            }
            ResourceRequest request = Resources.LoadAsync<T>(prefix + "/" + path);
            yield return request;
            if (request.asset == null)
            {
                request = Resources.LoadAsync(prefix + "/default");
                yield return request;
            }
            resourceCache[prefix + "/" + path] = request.asset;
            onLoad?.Invoke(request.asset as T);
        }
    }

    public static void LoadDialogueData(string id, Action<TextAsset> onLoad)
    {
        LoadResource(PREFIX_DIALOGUE, id, onLoad);
    }

    public static void LoadItemSprite(int id, Action<Sprite> onLoad)
    {
        LoadResource<Texture2D>(PREFIX_ITEM, $"{id}", (texture) =>
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            onLoad?.Invoke(sprite);
        });
    }

    public static void LoadUI(string path, Action<GameObject> onLoad)
    {
        LoadResource(PREFIX_UI, path, onLoad);
    }

    public static void WaitForSeconds(float seconds, Action onComplete)
    {
        monoBehaviour.StartCoroutine(WaitCoroutine());
        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
}
