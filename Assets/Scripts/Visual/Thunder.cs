using System;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        NoteGameManager.Instance.onCloudTriggered+= OnCloudTriggered;
    }

    private void OnCloudTriggered(object sender, EventArgs e)
    {
        print("++++");
        _animator.SetTrigger("Trigger");
    }
}
