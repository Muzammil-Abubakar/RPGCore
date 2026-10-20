
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector director;
        private GameObject player;
        private PlayerController playerController;
        private ActionScheduler actionScheduler;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();

            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
                actionScheduler = player.GetComponent<ActionScheduler>();
            }
        }

        private void OnEnable()
        {
            if (director == null)
            {
                director = GetComponent<PlayableDirector>();
            }

            if (director != null)
            {
                director.played += DisableControl;
                director.stopped += EnableControl;
            }
        }

        private void OnDisable()
        {
            if (director != null)
            {
                director.played -= DisableControl;
                director.stopped -= EnableControl;
            }

            EnableControl(director);
        }

        private void DisableControl(PlayableDirector pd)
        {
            if (playerController == null || actionScheduler == null)
            {
                Debug.LogWarning("Player or required components not found.", this);
                return;
            }

            actionScheduler.CancelCurrentAction();
            playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }
}
