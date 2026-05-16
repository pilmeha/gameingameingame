using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() => DuckGameManager.Instance.OnScoreChange.AddListener(UpdateText);
    void OnDisable() => DuckGameManager.Instance.OnScoreChange.RemoveListener(UpdateText);

    private void UpdateText(float score) => textMesh.text = $"{score}";
}
