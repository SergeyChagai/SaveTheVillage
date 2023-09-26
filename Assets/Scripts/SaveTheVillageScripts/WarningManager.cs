using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    public Animator WheatInfoRowAnimator;
    public UnitCountManager UnitCountManager;
    public GameObject LoosePanel;

    void Start()
    {
        UnitCountManager.WheatNotEnoughAction += OnWheatNotEnoughAction;
        UnitCountManager.LooseAction += OnLooseAction;
    }

    private void OnLooseAction()
    {
        LoosePanel.SetActive(true);
    }

    private void OnWheatNotEnoughAction()
    {
        WheatInfoRowAnimator.SetTrigger("TextWarning");
    }
}
