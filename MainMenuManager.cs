using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI topText;
    [SerializeField]
    private TextMeshProUGUI bottomText;
    [SerializeField]
    private SlideTransition topSlideTransition;
    [SerializeField]
    private SlideTransition bottomSlideTransition;
    [SerializeField]
    private GameObject entityPrefab;

    private List<GameObject> entities = new List<GameObject>();

    private const float MAX_Y = 0.7f;
    private const float MAX_X = 0.45f;

    private void Start()
    {
        StartCoroutine(StartTextCoroutine());
        StartCoroutine(AddTextCoroutine());
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator StartTextCoroutine()
    {
        yield return new WaitUntil(() => topSlideTransition.transitionDone);

        topText.gameObject.transform.position += (Vector3)new Vector2(0.01f, 0);
        bottomText.gameObject.transform.position -= (Vector3)new Vector2(0.01f, 0);

        yield return new WaitForSeconds(Time.deltaTime);

        StartCoroutine(StartTextCoroutine());
    }

    private IEnumerator AddTextCoroutine()
    {
        topText.text += "FESTER. SPOIL. ROT. PUTREFY. ";
        bottomText.text += "PUTREFY. ROT. SPOIL. FESTER. ";

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(AddTextCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitUntil(() => topSlideTransition.transitionDone);

        if (entities.Count < 10)
        {
            SpawnEntity(EntityDB.GetRandomEntityByRarity(Rarity.Common));
        }

        yield return new WaitForSeconds(5);

        StartCoroutine(SpawnCoroutine());
    }

    public void SpawnEntity(Entity entity)
    {
        GameObject entityGO = Instantiate(entityPrefab, GetRandomCoordinate(), Quaternion.identity);
        EntityController entityController = entityGO.GetComponent<EntityController>();
        Rigidbody2D rb = entityGO.GetComponent<Rigidbody2D>();

        entities.Add(entityGO);

        entityController.Init(entity);

        float randomAngle = Random.Range(0, 360);
        float randomSpeed = Random.Range(3, 5);

        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;

        rb.AddForce(dir * randomSpeed, ForceMode2D.Impulse);
    }

    public Vector2 GetRandomCoordinate()
    {
        return new Vector2(Random.Range(-MAX_X, MAX_X), Random.Range(-MAX_Y, MAX_Y));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayGameAudio()
    {
        AudioManager.Instance.Play(4);
    }
}
