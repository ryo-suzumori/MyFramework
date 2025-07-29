using UnityEngine;
using UnityEngine.UI;

namespace MyFw
{
    public class MaterialDesignColorComponent : MonoBehaviour
    {
        [SerializeField] private MaterialColorKey materialColor = MaterialColorKey.Blue;
        [SerializeField] private MaterialColorWeight colorWeight = MaterialColorWeight._500;

        private IMaterialColorApplicable colorAdapter;
        private bool hasAppliedAtRuntime = false;

        public MaterialColorKey MaterialColor => materialColor;
        public MaterialColorWeight ColorWeight => colorWeight;

        void Awake()
        {
            // 利用可能なコンポーネントを自動検出してアダプターを作成
            InitializeAdapter();
        }

        void Start()
        {
            if (Application.isPlaying && !hasAppliedAtRuntime)
            {
                ApplyMaterialColor();
                hasAppliedAtRuntime = true;
            }
        }

        private void InitializeAdapter()
        {
            // 優先度順でコンポーネントをチェック
            if (TryGetComponent<Image>(out var image))
            {
                colorAdapter = new ImageColorAdapter(image);
                return;
            }

            if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                colorAdapter = new SpriteRendererColorAdapter(spriteRenderer);
                return;
            }
            
            LogUtil.LogWarning($"MaterialDesignColorComponent: No supported component found on '{gameObject.name}'. Supported components: Image, SpriteRenderer, TextMeshProUGUI");
        }

        public void ApplyMaterialColor()
        {
            if (colorAdapter == null)
            {
                InitializeAdapter();
            }

            if (colorAdapter != null)
            {
                Color color = MaterialDesignPalette.GetColor(materialColor, colorWeight);
                colorAdapter.ApplyColor(color);
            }
        }

        public Color GetCurrentMaterialColor()
        {
            return MaterialDesignPalette.GetColor(materialColor, colorWeight);
        }

        public string GetTargetComponentName()
        {
            return colorAdapter?.GetComponentName() ?? "None";
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!Application.isPlaying)
            {
                ApplyMaterialColor();
            }
        }
#endif
    }
}