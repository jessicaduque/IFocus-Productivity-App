using System;
using System.Collections;
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
    [SerializeField] private Vector2 initialTimeForPlantSeconds;
    [SerializeField] private Vector2 timeBetweenRoundsSeconds;
    [SerializeField] private float timeForAnimation;
    [Header("UI Elements")] 
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
        float initialTime = Random.Range(initialTimeForPlantSeconds.x, initialTimeForPlantSeconds.y);
        StartCoroutine(StartRoundCoroutine(initialTime));
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
        plant.plantButton.enabled = false;
    }
    
    private void SetPlantDry(Plant plant)
    {
        plant.plantImage.sprite = plant.drySprite;
        _currentDryPlant = plant;
        _previouslyDriedPlant = plant;
        plant.plantButton.enabled = true;
    }
    
    private IEnumerator StartRoundCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        SelectRandomPlantToDry();
    }

    private void SelectRandomPlantToDry()
    {
        if (_previouslyDriedPlant == null)
        {
            if (PlayerPrefs.HasKey("previouslyDriedPlant"))
            {
                _previouslyDriedPlant = plantsArray[PlayerPrefs.GetInt("previouslyDriedPlant")];
            }
        }

        int plantToDry = Random.Range(0, plantsArray.Length);
        if (_previouslyDriedPlant == null)
        {
            SetPlantDry(plantsArray[plantToDry]);
        }
        else
        {
            while (plantsArray[plantToDry] == _previouslyDriedPlant)
            {
                plantToDry = Random.Range(0, plantsArray.Length);
            }
            SetPlantDry(plantsArray[plantToDry]);
        }

        PlayerPrefs.SetInt("previouslyDriedPlant", plantToDry);
    }

    private void WaterPlant()
    {
        StartCoroutine(WaterPlantCoroutine());
    }
    private IEnumerator WaterPlantCoroutine()
    {
        _uIPanelsManager.ControlBlockClicksPanel(true);
        _audioManager.PlaySfx("waterPlant");
        _currentDryPlant.waterPlantEffect.SetActive(true);
        yield return new WaitForSeconds(timeForAnimation * 2 / 3);
        SetPlantWet(_currentDryPlant);
        yield return new WaitForSeconds(timeForAnimation / 3);
        _uIPanelsManager.ControlHaveWaterNotificationPanel(true);
        _currentDryPlant.waterPlantEffect.SetActive(false);
        _uIPanelsManager.ControlBlockClicksPanel(false);
        float nextRoundTime = Random.Range(timeBetweenRoundsSeconds.x, timeBetweenRoundsSeconds.y);
        StartCoroutine(StartRoundCoroutine(nextRoundTime));
    }
}
