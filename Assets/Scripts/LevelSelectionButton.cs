using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField]private LevelDataSO levelDataSo;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            LevelDataManager.Instance.LoadNewLevelData(levelDataSo);
        });
    }
}
