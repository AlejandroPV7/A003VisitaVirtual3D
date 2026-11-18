using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent body;

    private Vector3 targetPosition;

    [SerializeField] private float lightProbability = 0.05f;

    [Header("Matar al jugador")]
    [SerializeField] private float killDistance = 1.5f;

    private bool isActive = false;
    private bool hasKilled = false;

    private MeshRenderer visuals;
    private Collider bodyCollider;

    private void Start()
    {
        body = GetComponent<NavMeshAgent>();
        visuals = GetComponentInChildren<MeshRenderer>();
        bodyCollider = GetComponent<Collider>();

        // La rotacion la controlamos nosotros (mirando a la camara), no el agente
        body.updateRotation = false;

        // Que la imagen quede apoyada en el suelo y no medio enterrada
        body.baseOffset = visuals.transform.localScale.y / 2f;
    }

    void Update()
    {
        if (hasKilled) return;

        if (Random.value < lightProbability)
        {
            isActive = !isActive;
            LightController.Instance.SetLights(!isActive);
        }

        visuals.enabled = isActive;
        if (bodyCollider != null) bodyCollider.enabled = isActive;

        // Si no esta activo, se queda quieto (no se mueve invisible)
        body.isStopped = !isActive;

        if (isActive)
        {
            targetPosition = PlayerController.Instance.transform.position;
            body.SetDestination(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

            // Mirar a la camara solo girando en horizontal (sin inclinarse)
            Vector3 lookDir = Camera.main.transform.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(lookDir);

            // Si te alcanza, te mata
            Vector3 toPlayer = targetPosition - transform.position;
            toPlayer.y = 0f;
            if (toPlayer.magnitude < killDistance)
                KillPlayer();
        }
    }

    private void KillPlayer()
    {
        hasKilled = true;
        Debug.Log("El enemigo te ha matado");

        // Volver a encender las luces antes de reiniciar, para que el material no se quede apagado
        LightController.Instance.SetLights(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
