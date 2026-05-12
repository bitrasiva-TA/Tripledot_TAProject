using System.Collections;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private const string LEVEL_COMPLETE_ANIMATION = "LevelComplete";

    [SerializeField] private float delayBeforeAnimation = 0.1f;
    [SerializeField] private Animator levelCompleteAnimator;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeAnimation);
        levelCompleteAnimator.Play(LEVEL_COMPLETE_ANIMATION);
    }
}
