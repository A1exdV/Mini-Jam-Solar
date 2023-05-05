using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelDataSo;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.IsLevelOpen(levelDataSo.index))
                TransitionVisual.onTransitionRequired?.Invoke(this, new TransitionEventArgs
                {
                    toVisible = true,
                    transitionTime = 1,
                    callback = () => { LevelDataHolder.Instance.LoadNewLevelData(levelDataSo); }
                });
        });
    }
}