using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaterPlantsSystemManager : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public Image plantImage; 
        public Sprite wetSprite;  
        public Sprite drySprite;
        [HideInInspector] public bool isDry = false;
    }
    [SerializeField] private Plant[] plantsArray; 
    [SerializeField] private Button[] plantsButtonArray; 
    private Plant _previouslyDriedPlant;

    [SerializeField] private Vector2 initialTimeForPlant;
    [SerializeField] private Vector2 timeBetweenRounds;
    

    private void Start()
    {
        InitializePlants();
        float initialTime = Random.Range(initialTimeForPlant.x, initialTimeForPlant.y);
        StartCoroutine(StartRounds(initialTime));
    }

    private void InitializePlants()
    {
        foreach (var plant in plantsArray)
        {
            SetPlantHealthy(plant);
        }
    }

    private IEnumerator StartRounds(float time)
    {
        yield return new WaitForSeconds(time);
        SelectRandomPlantToWilt();
    }

    private void SelectRandomPlantToWilt()
    {
        // Filtra as plantas que estavam saudaveis na rodada anterior
        List<Plant> eligiblePlants = plantsArray.ToList();

        Plant plantToWilt = new Plant();
        if (eligiblePlants.Count > 0)
        {
            // Escolhe a planta para murchar
            plantToWilt = eligiblePlants[Random.Range(0, eligiblePlants.Count)];
            SetPlantWilted(plantToWilt);
        }

        // Atualiza a lista de plantas murchas
        UpdatePreviouslyWiltedPlants(plantToWilt);
    }

    private void UpdatePreviouslyWiltedPlants(Plant currentPlant)
    {
        _previouslyDriedPlant = currentPlant;
    }

    private void SetPlantHealthy(Plant plant)
    {
        plant.isDry = false;
        plant.plantImage.GetComponent<Image>().sprite = plant.wetSprite;
    }

    private void SetPlantWilted(Plant plant)
    {
        plant.isDry = true;
        plant.plantImage.GetComponent<Image>().sprite = plant.drySprite;
    }

    private void ToggleState(Plant plant)
    {
        SetPlantHealthy(plant);
        //Button botaoPlanta;
        //botaoPlanta.interactable = true;
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
