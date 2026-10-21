using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum PortalIdentifier
        {
            A, B, C, D, E, F, G
        }

        [SerializeField] private int destinationSceneIndex;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private PortalIdentifier identifier;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (destinationSceneIndex < 0)
            {
                Debug.LogWarning("Portal: Invalid destination scene index!", this);
                return;
            }

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationSceneIndex);

            yield return asyncLoad;

            Portal otherPortal = GetOtherPortal();
            if (otherPortal != null)
            {
                UpdatePlayer(otherPortal);
            }

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsInactive.Exclude))
            {
                if (portal == this)
                    continue;

                if (portal.identifier == this.identifier)
                {
                    return portal;
                }
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
    }
}