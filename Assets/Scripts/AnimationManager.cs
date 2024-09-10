using System.Collections.Generic;
using UnityEngine;

public delegate void UseAnimatorDelegate(Animator animator);
public class AnimationManager: MonoBehaviour
{
    public static AnimationManager Instance; 
    
    private Dictionary<string, Animator> _animators;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitEvent(string objectName) => InitializeAnimator(objectName);

    public void UseAnimator(string objectName, UseAnimatorDelegate callback) => callback(Instance._animators[objectName]);

    private void InitializeAnimator(string objectName) {
        var animator = GameObject.Find(objectName).GetComponent<Animator>();
        if (_animators == null)
        {
            Instance._animators = new() { {objectName, animator} };
        }
        else
        {
            if (_animators.ContainsKey(objectName))
            {
                Instance._animators[objectName] = animator;
                
            }
            else
            {
                Instance._animators.Add(objectName, animator);
            }
        }
    }
}

