using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator folder;

    public void Play()
    {
        folder.SetBool("isTrue", true);
    }
    public void StartButton()
    {
        StartCoroutine(PlayAnimationAndLoadScene());
    }

    private IEnumerator PlayAnimationAndLoadScene()
    {
        folder.SetBool("isTrue", false);

        // Wait for the animation to finish
        AnimatorStateInfo stateInfo = folder.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        SceneManager.LoadSceneAsync("Testing");
    }
}
