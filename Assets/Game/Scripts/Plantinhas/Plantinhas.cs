using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plantinhas : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public Image plantObject;
        public Sprite healthySprite;
        public Sprite wiltedSprite;
        [HideInInspector] public bool isWilted = false;
        public Button botaoPlanta;
    }
    public List<Plant> plants;
    public float timeBetweenRounds = 5f;
    private Plant previouslyWiltedPlants = null;
    private Plant wilted = null;

    private void Start()
    {
        InitializePlants();
        StartCoroutine(StartRounds());
    }


    private void InitializePlants()
    {
        foreach (var plant in plants)
        {
            SetPlantHealthy(plant);
            plant.botaoPlanta.onClick.AddListener(() => OnPlantClicked());
        }
    }

    private IEnumerator StartRounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenRounds);
            SelectRandomPlantToWilt();
        }
    }

    private void SelectRandomPlantToWilt()
    {

        if(previouslyWiltedPlants == null || previouslyWiltedPlants.isWilted == false)
        {
            // Filtra as plantas que estavam saudaveis na rodada anterior
            List<Plant> eligiblePlants = new List<Plant>();

            foreach (var plant in plants)
            {
                if (previouslyWiltedPlants != plant)
                {
                    eligiblePlants.Add(plant);
                }
            }

            if (eligiblePlants.Count > 0)
            {
                // Escolhe a planta para murchar
                Plant plantToWilt = eligiblePlants[Random.Range(0, eligiblePlants.Count)];
                SetPlantWilted(plantToWilt);
            }

            // Atualiza a lista de plantas murchas
            UpdatePreviouslyWiltedPlants();
        }

    }

    private void UpdatePreviouslyWiltedPlants()
    {
        foreach (var plant in plants)
        {
            if (plant.isWilted)
            {
                previouslyWiltedPlants = plant; 
            }
        }
    }

    public void SetPlantHealthy(Plant plant)
    {
        if (plant == null || plant.plantObject == null || plant.plantObject.GetComponent<Image>() == null || plant.botaoPlanta == null)
        {
            Debug.LogError("Plant or its components are not properly configured!");
            return;
        }

        plant.isWilted = false;
        plant.plantObject.GetComponent<Image>().sprite = plant.healthySprite;
        plant.botaoPlanta.interactable = false;
    }

    private void SetPlantWilted(Plant plant)
    {
        wilted = plant;
        plant.isWilted = true;
        plant.plantObject.GetComponent<Image>().sprite = plant.wiltedSprite;
        plant.botaoPlanta.interactable = true;
    }

    public void OnPlantClicked()
    {
        SetPlantHealthy(wilted);

    }

    /*
    public void OnPlantClicked(GameObject clickedPlant)
    {
        Plant clicked = plants.Find(p => p.plantObject == clickedPlant);
        if (clicked != null && clicked.isWilted)
        {
            SetPlantHealthy(clicked);
        }
    }
    */
}