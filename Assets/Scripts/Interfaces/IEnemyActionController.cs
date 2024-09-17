namespace Interfaces
{
    public interface IEnemyActionController
    {
        public void UseDashAnimation(bool isActive);
        public void UseRunAnimation(bool isActive);
        public void UseIdleAnimation(bool isActive);
    }
}