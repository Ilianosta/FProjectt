using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class Building : MonoBehaviour
{
    [SerializeField] private float constructionTime;
    public BuildingSize[] buildingSize; // Tamaño en la grid (en unidades de grid)
    [SerializeField] private Resource[] resourceGeneration;
    public Sprite Sprite => gameObject.GetComponent<SpriteRenderer>().sprite;
    private bool isWorking;

    private async void Awake()
    {
        await ConstructBuilding();
    }
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

    private async Task ConstructBuilding()
    {
        float timer = constructionTime;
        while (timer > 0)
        {
            await Task.Delay(1000);
            timer -= 1;
            // Debug.Log("timer: " + timer);
        }
        isWorking = true;
        // Debug.Log("iniciando produccion");
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
                // Debug.Log("Item: " + _resource.ToString() + "\n Counter: " + _counter.ToString());
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
