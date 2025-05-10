using UnityEngine;

public class Game : Singleton<Game>
{
    protected override void Awake()
    {
        base.Awake();
        Utility.Setup(this);
    }
}
