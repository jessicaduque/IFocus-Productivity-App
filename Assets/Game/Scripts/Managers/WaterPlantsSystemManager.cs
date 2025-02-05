using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WaterPlantsSystemManager : MonoBehaviour
{
    [Serializable]
    public class Plant
    {
        public Image plantImage; 
        public Button plantButton; 
        public Sprite wetSprite;  
        public Sprite drySprite;
        public GameObject waterPlantEffect;
    }
    [Header("Plants Components")]
    [SerializeField] private Plant[] plantsArray; 
    private Plant _currentDryPlant, _previouslyDriedPlant;
    [Header("Plant Times")]
    [SerializeField] private Vector2 initialTimeForPlant;
    [SerializeField] private Vector2 timeBetweenRounds;
    [SerializeField] private float timeForAnimation;
    [Header("UI Elements")] 
    [SerializeField] private GameObject blockClicksPanel;
    [SerializeField] private Button closeHaveWaterPanelButton;
    private UIPanelsManager _uIPanelsManager => UIPanelsManager.I;
    private AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        closeHaveWaterPanelButton.onClick.AddListener(delegate
        {
            _uIPanelsManager.ControlHaveWaterNotificationPanel(false);
        });
        
        InitializePlants();
        float initialTime = Random.Range(initialTimeForPlant.x, initialTimeForPlant.y);
        StartCoroutine(StartRounds(initialTime));
    }

    private void InitializePlants()
    {
        foreach (var plant in plantsArray)
        {
            SetPlantWet(plant);
            plant.plantButton.onClick.AddListener(WaterPlant);
            plant.plantButton.enabled = false;
        }
    }

    private void SetPlantWet(Plant plant)
    {
        plant.plantImage.sprite = plant.wetSprite;
        plant.waterPlantEffect.SetActive(false);
    }
    
    private void SetPlantDry(Plant plant)
    {
        plant.plantImage.sprite = plant.drySprite;
        _currentDryPlant = plant;
        SetPreviousDryPlant(plant);
        plant.plantButton.enabled = true;
    }
    
    private void SetPreviousDryPlant(Plant currentPlant)
    {
        _previouslyDriedPlant = currentPlant;
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
            SetPlantDry(plantToWilt);
        }

        // Atualiza a lista de plantas murchas
        SetPreviousDryPlant(plantToWilt);
    }

    private void WaterPlant()
    {
        StartCoroutine(WaterPlantCoroutine());
    }
    private IEnumerator WaterPlantCoroutine()
    {
        blockClicksPanel.SetActive(true);
        _audioManager.PlaySfx("waterPlant");
        _currentDryPlant.waterPlantEffect.SetActive(true);
        yield return new WaitForSeconds(timeForAnimation);
        _uIPanelsManager.ControlHaveWaterNotificationPanel(true);
        blockClicksPanel.SetActive(false);
    }
    
}
