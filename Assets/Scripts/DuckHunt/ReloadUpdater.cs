using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ReloadUpdater : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() => DuckGameManager.Instance.OnReload.AddListener(UpdateText);
    void OnDisable() => DuckGameManager.Instance.OnReload.RemoveListener(UpdateText);

    private void UpdateText(int reload) => textMesh.text = $"{reload}";
}
