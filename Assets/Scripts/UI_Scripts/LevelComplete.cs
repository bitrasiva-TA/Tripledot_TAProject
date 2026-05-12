using System.Collections;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private const string LEVEL_COMPLETE_ANIMATION = "LevelComplete";

    [SerializeField] private float _delayBeforeAnimation = 0.1f;
    [SerializeField] private Animator _levelCompleteAnimator;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delayBeforeAnimation);
        _levelCompleteAnimator.Play(LEVEL_COMPLETE_ANIMATION);
    }
}
