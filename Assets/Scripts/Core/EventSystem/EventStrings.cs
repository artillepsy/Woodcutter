namespace Assets.Scripts.Core
{
    /// <summary>
    /// Кoнтейнер названий ивентов
    /// </summary>
    public static class EventStrings
    {
        // глобальные ивенты
        public const string LEVEL_UP = "LevelUp";
        public const string LEVEL_UP_REQUEST = "LevelUpRequest";
        public const string READY_TO_UPGRADE_LEVEL = "ReadyToUpgradeLevel";

        // ввод
        public const string JOYSTICK_INPUT = "JoystickInput";

        // действия
        public const string TREE_KICKED = "TreeKicked";

        // анимации
        public const string CUT_PROPERTY_CHANGED = "CutPropertyChanged";
        public const string START_KICK = "StartKick";

        // манипуляции с растительностью
        public const string TREE_CUTTED = "TreeCutted";
        public const string STUMP_GROWED = "StumpGrowed";
        public const string TREE_LOOT_DROP = "TreeLootDrop";
    }
}