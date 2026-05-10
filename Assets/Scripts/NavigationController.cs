using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    
      //Load scene by name
    public void LoadScene(string sceneName)
    {
      SceneManager.LoadScene(sceneName);
    }
    /*   [SerializeField] private Animator animator;

    public void OnCloseButtonClicked()
    {
        animator.SetTrigger("Close");
    }


// Method called through animation event when the close animation is completed

    public void OnClosedAnimationCompleted()
    {
        gameObject.SetActive(false);
    }*/

}
