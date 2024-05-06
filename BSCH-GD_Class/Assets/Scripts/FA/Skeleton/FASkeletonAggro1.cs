using UnityEngine;

public class FASkeletonAggro : MonoBehaviour
{
    private Animator skeletonAnimator;

    private void Start()
    {
        // Find the summoning script on the summoner object and subscribe to the event
        FASkeletonBossSummon summoner = FindObjectOfType<FASkeletonBossSummon>();
        if (summoner != null)
        {
            summoner.OnBossSummoned += HandleBossSummoned;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        FASkeletonBossSummon summoner = FindObjectOfType<FASkeletonBossSummon>();
        if (summoner != null)
        {
            summoner.OnBossSummoned -= HandleBossSummoned;
        }
    }

    private void HandleBossSummoned(GameObject boss)
    {
        // Access the Animator from the summoned boss
        skeletonAnimator = boss.GetComponent<Animator>();
        skeletonAnimator.SetBool("IsRunning", true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && skeletonAnimator != null)
        {
            skeletonAnimator.SetBool("IsRunning", true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && skeletonAnimator != null)
        {
            skeletonAnimator.SetBool("IsRunning", false);
        }
    }
}
