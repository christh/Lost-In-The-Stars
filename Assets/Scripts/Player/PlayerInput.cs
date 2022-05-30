using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class PlayerInput : MonoBehaviour
    {
        //1
        private static readonly PlayerCommand MoveUp =
            new PlayerCommand(delegate (Player player)
            {
                player.Move(ThrustStates.Thrusting);
            }, "moveUp");

        //2
        private static readonly PlayerCommand MoveDown =
            new PlayerCommand(delegate (Player player)
            {
                player.Move(ThrustStates.Reversing);
            }, "moveDown");

        //3
        private static readonly PlayerCommand MoveLeft =
            new PlayerCommand(delegate (Player player)
            {
                player.Move(TurnStates.Left);
            }, "moveLeft");

        //4
        private static readonly PlayerCommand MoveRight =
            new PlayerCommand(delegate (Player player)
            {
                player.Move(TurnStates.Right);
            }, "moveRight");

        //5
        private static readonly PlayerCommand Shoot =
            new PlayerCommand(delegate (Player player) { Debug.Log("Pew Pew."); player.Shoot(); }, "shoot");
    }
}