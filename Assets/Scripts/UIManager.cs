using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Events;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    public GameObject pointsTextPrefab;
    public RectTransform pointsPosition;

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamage);
        CharacterEvents.characterHealed += (CharacterHealed);
        CharacterEvents.pointsAdded += (AddPoints);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamage);
        CharacterEvents.characterHealed -= (CharacterHealed);
        CharacterEvents.pointsAdded -= (AddPoints);
    }

    public void CharacterTookDamage(GameObject character, float damageReceived)
    {
        Vector3 spawnPosition = character.transform.position;

        TextMeshProUGUI tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TextMeshProUGUI>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, float healthRestored)
    {
        Vector3 spawnPosition = character.transform.position;

        TextMeshProUGUI tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TextMeshProUGUI>();

        tmpText.text = healthRestored.ToString();
    }

    public void AddPoints(PlayerController player, int pointsAdded)
    {
        TextMeshProUGUI tmpText = Instantiate(pointsTextPrefab, pointsPosition).GetComponent<TextMeshProUGUI>();
        tmpText.text = "+" + pointsAdded.ToString();
    }
}
