using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle };
public class GameController : MonoBehaviour
{
    private GameState _state;
    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            _state = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (_state == GameState.Dialog)
                _state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        switch (_state)
        {
            case GameState.Dialog:
                DialogManager.Instance.HandleUpdate();
                break;
            case GameState.Battle:
            default:
                break;
        }
    }
}
