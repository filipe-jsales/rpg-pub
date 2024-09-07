using Abstractions;

namespace Interfaces
{
    public interface IHitDetectable
    {
        public double OnHit(Armor armor);
        public void OnHitTaken(Character attacker);
    }
}