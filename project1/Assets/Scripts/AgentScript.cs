using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AgentScript : MonoBehaviour
{
    public Animator animator;
    public Transform goal;  // Player's position that the enemy follows

    public bool isDead = false;
    private bool deathHandled = false; // Prevent multiple calls
    public static float count;
    public static float score;
    private NavMeshAgent agent;

    // References to TextMeshPro components in the scene
    private TextMeshProUGUI enemyCountText;
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find the player object in the scene by its tag and assign it as the goal
        GameObject player = GameObject.FindWithTag("Plr");  // Assuming the player is tagged with "Player"

        if (player != null)
        {
            goal = player.transform;  // Assign the player's transform to the goal
        }
        else
        {
            Debug.LogError("Player not found in the scene. Please make sure the player is tagged with 'Player'.");
        }

        // Find TextMeshProUGUI objects in the scene for enemy count and score
        enemyCountText = GameObject.FindWithTag("enemyCount")?.GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.FindWithTag("scoreCount")?.GetComponent<TextMeshProUGUI>();

        // Check if the TextMeshProUGUI components are found
        if (enemyCountText == null || scoreText == null)
        {
            Debug.LogError("UI TextMeshPro components not found in the scene. Make sure they are assigned in the scene.");
        }
    }

    private void Update()
    {
        if (isDead && !deathHandled)
        {
            deathHandled = true; // Ensure death logic runs only once
            HandleDeath();
        }
        else if (!isDead && goal != null)
        {
            agent.destination = goal.position;  // Enemy follows the player
            animator.SetFloat("Speed_f", agent.velocity.magnitude);  // Update animation speed
        }
    }

    private void HandleDeath()
    {
        animator.SetBool("Death_b", true);  // Trigger death animation
        agent.speed = 0;  // Stop the agent's movement
        count++;

        // Update enemy count and score directly
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies Shot: " + count.ToString();
        }

        score = count * 100;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2f);  // Wait 2 seconds before removal

        Destroy(gameObject);  // Destroy the enemy object
    }
}
