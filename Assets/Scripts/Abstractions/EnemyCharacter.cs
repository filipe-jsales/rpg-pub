using Interfaces;

namespace Abstractions
{
    public abstract class EnemyCharacter: Character, IUnityAnimations
    {
        private const string IsRunning = "isRunning";
        
        public virtual void UseDashAnimation(string objectName)
        {
            throw new System.NotImplementedException();
        }

        public virtual void UseRunAnimation(string objectName)
        {
            AnimationManager.Instance.UseAnimator(objectName, (animator) =>
            {
                animator.SetBool(IsRunning, true);
            });
        }
    }
}