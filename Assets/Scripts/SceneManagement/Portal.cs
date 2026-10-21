
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private string destinationScene;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (string.IsNullOrEmpty(destinationScene))
            {
                Debug.LogWarning("Portal: No destination scene assigned!", this);
                return;
            }

            Debug.Log($"Player enters portal. Loading {destinationScene}.");

            SceneManager.LoadScene(destinationScene);
        }
    }
}
