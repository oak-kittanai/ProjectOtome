using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowMainMenu(new()
        {
            OnPlay = () =>
            {
                SceneLoader.Instance.LoadScene("TurnBase");
            },
            OnExit = () =>
            {
                Application.Quit();
            }
        });
    }
}
