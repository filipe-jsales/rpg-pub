using Interfaces;

namespace Abstractions
{
    public abstract class EnemyCharacter: Character, IEnemyAction
    {
        // TODO: create enum and isIdling should be isIdle
        private const string IsRunning = "isRunning";
        private const string IsIdling = "isIdling";
        
        public virtual void UseDashAnimation(string objectName, bool isActive)
        {
            throw new System.NotImplementedException();
        }

        public virtual void UseRunAnimation(string objectName, bool isActive)
        {
            EnemyActionManager.Instance.UseAnimator(objectName, (animator) =>
            {
                animator.SetBool(IsRunning, isActive);
                animator.SetBool(IsIdling, !isActive);
            });
        }

        public virtual void UseIdleAnimation(string objectName, bool isActive)
        {
            EnemyActionManager.Instance.UseAnimator(objectName, (animator) =>
            {
                animator.SetBool(IsIdling, isActive);
                animator.SetBool(IsRunning, !isActive);
            });
        }
    }
}