using UnityEngine;

public class DishModelPreviewRenderer : MonoBehaviour
{
    [Header("Catálogo de modelos")]
    [SerializeField] private ARDishModelCatalog dishModelCatalog;

    [Header("Escena de previsualización")]
    [SerializeField] private Camera previewCamera;
    [SerializeField] private Transform previewRoot;
    [SerializeField] private RenderTexture previewRenderTexture;

    [Header("Rotación automática")]
    [SerializeField] private float autoRotationSpeed = 22.0f;

    [Header("Presentación")]
    [SerializeField] private Vector3 modelWrapperEulerOffset = new Vector3(0.0f, -25.0f, 0.0f);
    [SerializeField] private Vector3 modelWrapperPositionOffset = Vector3.zero;

    private GameObject currentWrapper;
    private GameObject currentModel;
    private float currentRotationY;

    public RenderTexture PreviewRenderTexture => previewRenderTexture;

    private void Awake()
    {
        if (previewCamera != null && previewRenderTexture != null)
        {
            previewCamera.targetTexture = previewRenderTexture;
        }
    }

    private void Update()
    {
        if (currentWrapper == null)
        {
            return;
        }

        currentRotationY += autoRotationSpeed * Time.deltaTime;
        ApplyRotation();
    }

    public void ShowDishModel(string dishId)
    {
        if (string.IsNullOrWhiteSpace(dishId))
        {
            Debug.LogWarning("[DishModelPreviewRenderer] DishId vacío.");
            return;
        }

        if (dishModelCatalog == null)
        {
            Debug.LogWarning("[DishModelPreviewRenderer] No hay ARDishModelCatalog asignado.");
            return;
        }

        if (previewRoot == null)
        {
            Debug.LogWarning("[DishModelPreviewRenderer] No hay PreviewRoot asignado.");
            return;
        }

        ClearCurrentModel();

        GameObject prefab = dishModelCatalog.GetPrefabByDishId(dishId);

        if (prefab == null)
        {
            Debug.LogWarning($"[DishModelPreviewRenderer] No se encontró prefab para DishId={dishId}.");
            return;
        }

        currentWrapper = new GameObject($"PreviewWrapper_{dishId}");
        currentWrapper.transform.SetParent(previewRoot, false);
        currentWrapper.transform.localPosition = modelWrapperPositionOffset;
        currentWrapper.transform.localRotation = Quaternion.identity;
        currentWrapper.transform.localScale = Vector3.one;

        currentModel = Instantiate(prefab, currentWrapper.transform);

        /*
         * Importante:
         * No forzamos currentModel.transform.localScale = Vector3.one.
         * No forzamos escala en X/Y/Z.
         * El modelo conserva la escala que ya configuraste en el prefab/modelo.
         */

        currentModel.transform.localPosition = Vector3.zero;

        SetLayerRecursively(currentWrapper, previewRoot.gameObject.layer);

        currentRotationY = 0.0f;
        ApplyRotation();

        Debug.Log($"[DishModelPreviewRenderer] Modelo cargado en visor: {prefab.name}");
    }

    private void ApplyRotation()
    {
        if (currentWrapper == null)
        {
            return;
        }

        Vector3 finalRotation = modelWrapperEulerOffset + new Vector3(0.0f, currentRotationY, 0.0f);
        currentWrapper.transform.localRotation = Quaternion.Euler(finalRotation);
    }

    private void ClearCurrentModel()
    {
        if (currentWrapper != null)
        {
            Destroy(currentWrapper);
            currentWrapper = null;
            currentModel = null;
        }

        if (previewRoot == null)
        {
            return;
        }

        for (int i = previewRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(previewRoot.GetChild(i).gameObject);
        }
    }

    private void SetLayerRecursively(GameObject target, int layer)
    {
        if (target == null)
        {
            return;
        }

        target.layer = layer;

        foreach (Transform child in target.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}