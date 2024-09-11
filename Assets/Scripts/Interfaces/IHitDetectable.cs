using Abstractions;

namespace Interfaces
{
    public interface IHitDetectable
    {
        public float OnHit(Armor armor);
        public void OnHitTaken(Character attacker);
    }
}