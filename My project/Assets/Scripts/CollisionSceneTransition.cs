using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionSceneTransition : MonoBehaviour
{
    [Header("Collision Settings")]
    [Tooltip("The tag of the object that this object should collide with to trigger the scene transition.")]
    [SerializeField] private string targetTag = "TransitionTrigger";

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private float transitionDelay = 0.5f;

    [Header("Optional Transition Effects")]
    [SerializeField] private bool fadeTransition = true;
    [SerializeField] private float fadeTime = 1f;

    private bool hasTriggered = false;

    // This script is now attached to the spawned object itself
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the specified tag and the transition hasn't happened yet
        if (collision.gameObject.CompareTag(targetTag) && !hasTriggered)
        {
            hasTriggered = true;

            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogError("Scene name is empty! Please set the scene name in the inspector.");
                return;
            }

            if (transitionDelay > 0)
            {
                Invoke(nameof(LoadScene), transitionDelay);
            }
            else
            {
                LoadScene();
            }
        }
    }

    private void LoadScene()
    {
        // Add your logic to disable other components if necessary
        // For example, if you need to clean up interaction handlers
        // that exist on this object or others in the scene.
        var grabAndLocates = FindObjectsOfType<Meta.XR.MRUtilityKit.BuildingBlocks.GrabAndLocate>();
        foreach (var component in grabAndLocates)
        {
            if (component != null)
            {
                component.enabled = false;
            }
        }
        
        if (fadeTransition)
        {
            StartCoroutine(FadeAndLoadScene());
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Implement your fade effect
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}