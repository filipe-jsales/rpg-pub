using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle };
public class GameController : MonoBehaviour
{

    GameState state;
    

    private void Start()
    {
        var weapon = new TestWeapon(1000, 70, 200);
        var armor = new TestArmor(1000, 50, 500);
        var weapon2 = new TestWeapon(1000, 70, 200);
        var armor2 = new TestArmor(1000, 50, 500);
        var player1 = new Player(100, 10, 15, armor, weapon);
        var player2 = new Player(100, 10, 15, armor2, weapon2);
        player1.OnHitTaken(player2);
        player2.OnHitTaken(player1);
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            //gingerMovement.Update();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
    }
}
