using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    [SerializeField] private Resource[] resourceGeneration;
    [SerializeField] private bool isWorking;
    private void Update()
    {
        if (!isWorking) return;

        foreach (Resource resourceGen in resourceGeneration)
        {
            if (resourceGen.UpdateCounter <= 0)
            {
                resourceGen.ResetCounter();
                AddResources(resourceGen.resource, resourceGen.amount);
            }
        }
    }

    private void AddResources(Resources resource, float amount)
    {
        GameManager.instance.SetResource(resource, amount);
    }

    [System.Serializable]
    private class Resource
    {
        [SerializeField] private Resources _resource;
        [SerializeField] private float _amount;
        [SerializeField] private float _timeBetweenObtainance;
        private float _counter;
        public Resources resource => _resource;
        public float amount => _amount;
        public float UpdateCounter
        {
            get
            {
                _counter -= Time.deltaTime;
                Debug.Log("Item: " + _resource.ToString() + "\n Counter: " + _counter.ToString());
                return _counter;
            }
        }
        public void ResetCounter() => _counter = _timeBetweenObtainance;
    }
    public enum Resources
    {
        food,
        water,
        electricity,
        gold,
        diamonds
    }
}

