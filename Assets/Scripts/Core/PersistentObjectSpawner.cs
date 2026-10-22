using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObject;

        private static bool hasSpawned = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetStaticData()
        {
            hasSpawned = false;
        }

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject spawnInstance = Instantiate(persistentObject);
            DontDestroyOnLoad(spawnInstance);
        }
    }
}