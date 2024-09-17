namespace Interfaces
{
    public interface IEnemyAction
    {
        public void UseDashAnimation(string objectName, bool isActive);
        public void UseRunAnimation(string objectName, bool isActive);
        public void UseIdleAnimation(string objectName, bool isActive);
    }
}