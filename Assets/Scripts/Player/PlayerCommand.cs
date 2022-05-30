using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class PlayerCommand : MonoBehaviour
    {
        //1
        private readonly string commandName;

        //2
        public PlayerCommand(ExecuteCallback executeMethod, string name)
        {
            Execute = executeMethod;
            commandName = name;
        }

        //3
        public delegate void ExecuteCallback(Player player);

        //4
        public ExecuteCallback Execute { get; private set; }

        //5
        public override string ToString()
        {
            return commandName;
        }
    }
}