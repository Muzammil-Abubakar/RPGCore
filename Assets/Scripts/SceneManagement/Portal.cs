using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int destinationSceneIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (destinationSceneIndex < 0)
            {
                Debug.LogWarning("Portal: Invalid destination scene index!", this);
                return;
            }

            Debug.Log($"Player enters portal. Loading scene index {destinationSceneIndex} asynchronously.");

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationSceneIndex);

            yield return asyncLoad;

            Debug.Log($"Scene loaded: {destinationSceneIndex} (printed from the previous portal).");

            Destroy(gameObject);
        }
    }
}