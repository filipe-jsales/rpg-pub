using Abstractions;

namespace Impl
{
    public class EnemyCharacterImpl : EnemyCharacter
    {
        public double Health { get; private set; }
        public double Mana { get; private set; }
        public double BaseDamage { get; private set; }
        public int BasePoise { get; private set; }
        public string ElementalAffinity { get; private set; }
        private Armor EquippedArmor { get; set; }
        private Weapon EquippedWeapon { get; set; }

        public EnemyCharacterImpl(string name, double health, double baseDamage, int basePoise, Armor equippedArmor,
            Weapon equippedWeapon)
        {
            Name = name;
            Health = health;
            BaseDamage = baseDamage;
            BasePoise = basePoise;
            EquippedArmor = equippedArmor;
            EquippedWeapon = equippedWeapon;
        }

        public override double OnHit(Armor armor)
        {
            EquippedWeapon.HandleDurabilityDamage(armor);
            return EquippedWeapon.HandlePhysicalDamage(BaseDamage);
        }

        public override void OnHitTaken(Character attacker)
        {
            var damage = attacker.OnHit(EquippedArmor);
            var damageTaken = EquippedArmor.HandlePhysicalDamage(damage);
            EquippedArmor.HandleDurabilityDamage(damage);
            EquippedArmor.HandlePoiseDamage(attacker.Weapon);
            SetHealth(Health - damageTaken);
        }

        public override double GetHealth()
        {
            return Health;
        }

        public override void SetHealth(double value)
        {
            Health = value;
        }

        protected override void SetBaseDamage(double value)
        {
            BaseDamage = value;
        }

        protected override void SetBasePoise(int value)
        {
            BasePoise = value;
        }

        protected override void SetMana(double value)
        {
            Mana = value;
        }

        protected override void SetElementalAffinity(string value)
        {
            ElementalAffinity = value;
        }

        protected override void SetLevel(int value)
        {
            Level = value;
        }

        protected override void SetExperience(double value)
        {
            Experience = value;
        }

        protected override Armor GetEquippedArmor()
        {
            return EquippedArmor;
        }

        protected override void SetEquippedArmor(Armor value)
        {
            EquippedArmor = value;
        }

        protected override Weapon GetEquippedWeapon()
        {
            return EquippedWeapon;
        }

        protected override void SetEquippedWeapon(Weapon value)
        {
            EquippedWeapon = value;
        }
    }
}