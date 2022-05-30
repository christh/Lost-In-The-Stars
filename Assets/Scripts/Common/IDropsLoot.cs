namespace IR
{

    internal interface IDropsLoot
    {
        void Drop();
        void SetLootValue(int value);
        void SetLootQuality(float value);
    }
}
