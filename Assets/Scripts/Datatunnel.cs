
using UnityEngine;

public class Datatunnel : MonoBehaviour
{
    public string playerName;

    public void Awake()
    {
        if (this != null)
        {
            Destroy(this);
        }
        Init();
    }

    private void Init()
    {
        DontDestroyOnLoad(this);
    }

}
