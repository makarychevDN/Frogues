namespace FroguesFramework
{
    public static class CharacterAnimatorParameters
    {
        public const string Attack = "Attack";
        public const string WeaponIndex = "Weapon Index";
        public const string Death = "Death";
        public const string Kick = "Kick";
        public const string Cast = "Cast";
        public const string TakeDamage = "Take Damage";
        public const string ChangeWeapon = "Change Weapon";
        public const string BoxIsOn = "BoxIsOn";
    }

    public enum AbilityAnimatorTriggers
    {
        Attack = 1, 
        DirectCast = 2,
        UndirectCast = 3,
        Kick = 4
    }

    public enum WeaponIndexes
    {
        NoNeedToChangeWeapon = -1,
        WeaponlessIndex = 0,
        ShieldIndex = 1,
        SwordIndex = 2,
        SpearIndex = 3,
        KnifeIndex = 4
    }
}