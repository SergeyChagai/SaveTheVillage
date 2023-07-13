using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class UnitCountManager : MonoBehaviour
{
    [SerializeField]
    private int wheatPerCycleForOnePeasant;
    [SerializeField]
    private int wheatPerCycleForOneSoldier;
    [SerializeField]
    private int SecondsInCycle;
    [SerializeField]
    private int _wheatCount;
    [SerializeField]
    private int x;
    private int _peasantCount;
    private int _soldiersCount;
    private Peasant[] _peasants;
    private Soldier[] _soldiers;
    private TMP_Text _infoText;
    private Timer _timer;

    public event Action WheatNotEnoughAction;


    // Start is called before the first frame update
    void Start()
    {
        _peasants = (Peasant[])FindObjectsByType(typeof(Peasant), FindObjectsInactive.Include, FindObjectsSortMode.None);
        _soldiers = (Soldier[])FindObjectsByType(typeof(Soldier), FindObjectsInactive.Include, FindObjectsSortMode.None);
        _infoText = GameObject.FindGameObjectWithTag("InfoTextField").GetComponent<TMP_Text>();

        _timer = (Timer)FindAnyObjectByType(typeof(Timer));
        _timer.StartTimer();
        _timer.IteractionChanged += OnIteractionChanged;
        EnablePeasants(4);
        UpdateInfo();
    }

    public void AddPeasant()
    {
        if (_wheatCount - x < 0)
        {
            WheatNotEnoughAction?.Invoke();
            return;
        }
        _wheatCount -= x;
        EnablePeasants(1);
        UpdateInfo();
    }

    public void AddSoldier()
    {
        if (_wheatCount - x < 0)
        {
            WheatNotEnoughAction?.Invoke();
            return;
        }
        _wheatCount -= x;
        EnableSoldiers(1);
        UpdateInfo();
    }

    public void DisableSoldier()
    {
        _soldiersCount--;
        UpdateInfo();

    }
    public void DisablePeasant()
    {
        _peasantCount--;
        UpdateInfo();

    }

    private void OnIteractionChanged()
    {
        CalculateWheat();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(_wheatCount.ToString());
        stringBuilder.AppendLine(_peasantCount.ToString());
        stringBuilder.AppendLine(_soldiersCount.ToString());
        _infoText.text = stringBuilder.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ;
    }

    private void EnablePeasants(int count)
    {
        _peasantCount += count;
        int firstIndex = 0;
        for (int i = 0; i < _peasants.Length;  i++)
        {
            if (_peasants[i] != null && _peasants[i].gameObject.activeInHierarchy != true)
            {
                firstIndex = i;
                break;
            }
        }
        for (int j = 0; j + firstIndex < _peasants.Length && j <= count - 1; j++)
        {
            _peasants[j + firstIndex].gameObject.SetActive(true);
            _peasants[j + firstIndex].GetComponent<Peasant>().Death += DisablePeasant;
        }
    }

    private void EnableSoldiers(int count)
    {
        _soldiersCount += count;
        int firstIndex = 0;
        for (int i = 0; i < _soldiers.Length; i++)
        {
            if (_soldiers[i] != null && _soldiers[i].gameObject.activeInHierarchy != true)
            {
                firstIndex = i;
                break;
            }
        }
        for (int j = 0; j + firstIndex < _soldiers.Length && j <= count - 1; j++)
        {
            _soldiers[j + firstIndex].gameObject.SetActive(true);
            _soldiers[j + firstIndex].GetComponent<Soldier>().Death += DisableSoldier;
        }
    }

    private void CalculateWheat()
    {
        _wheatCount = _wheatCount + _peasantCount * wheatPerCycleForOnePeasant - _soldiersCount * wheatPerCycleForOneSoldier;
    }
}
