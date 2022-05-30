using IR.Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class DropMinerals : MonoBehaviour, IDropsLoot
    {
        public GameObject MineralSprayer;
        public int MineralCount = 2;
        public float Quality = 0.25f;
        bool Triggered;

        public void Drop()
        {
         //   if (!Triggered)
            {
                //Debug.Log("Dropping minerals!");
                //Triggered = true;
                PickupFactory.SpawnMinerals(MineralSprayer, transform.position, MineralCount, Quality);
            }
        }


        public void SetLootQuality(float value)
        {
            Quality = value;
        }

        public void SetLootValue(int value)
        {
            MineralCount = value;
        }
    }
}