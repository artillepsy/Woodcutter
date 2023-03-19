namespace Assets.Scripts.Core
{
    /// <summary>
    /// КОнтейнер названий ивентов
    /// </summary>
    public static class EventStrings
    {
        // глобальные ивенты
        public const string LEVEL_UP = "LevelUp";

        // ввод
        public const string JOYSTICK_INPUT = "JoystickInput";

        // действия
        public const string TREE_KICKED = "ActionKicked";

        // анимации
        public const string CUT_PROPERTY_CHANGED = "AnimatorCutPropertyChanged";
        public const string START_KICK = "StartKick";

        // манипуляции с растительностью
        public const string TREE_CUTTED = "TreeCutted";
        public const string STUMP_GROWED = "StumpGrowed";
        public const string TREE_LOOT_DROP = "TreeLootDrop";
    }
}