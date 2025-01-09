using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plantinhas : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public GameObject plantObject; 
        public Sprite healthySprite;  
        public Sprite wiltedSprite;
        [HideInInspector] public bool isWilted = false;
    }
    public List<Plant> plants; 
    public float timeBetweenRounds = 5f; 
    private List<Plant> previouslyWiltedPlants = new List<Plant>(); 

    private void Start()
    {
        InitializePlants();
        StartCoroutine(StartRounds());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                foreach (var plant in plants)
                {
                    if (hit.collider.gameObject == plant.plantObject)
                    {
                        ToggleState(plant);
                        break;
                    }
                }
            }
        }
    }

    private void InitializePlants()
    {
        foreach (var plant in plants)
        {
            SetPlantHealthy(plant);
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
        // Filtra as plantas que estavam saudaveis na rodada anterior
        List<Plant> eligiblePlants = plants.FindAll(p => !p.isWilted && !previouslyWiltedPlants.Contains(p));

        if (eligiblePlants.Count > 0)
        {
            // Escolhe a planta para murchar
            Plant plantToWilt = eligiblePlants[Random.Range(0, eligiblePlants.Count)];
            SetPlantWilted(plantToWilt);
        }

        // Atualiza a lista de plantas murchas
        UpdatePreviouslyWiltedPlants();
    }

    private void UpdatePreviouslyWiltedPlants()
    {
        previouslyWiltedPlants.Clear();
        foreach (var plant in plants)
        {
            if (plant.isWilted)
            {
                previouslyWiltedPlants.Add(plant);
            }
        }
    }

    private void SetPlantHealthy(Plant plant)
    {
        plant.isWilted = false;
        plant.plantObject.GetComponent<Image>().sprite = plant.healthySprite;
    }

    private void SetPlantWilted(Plant plant)
    {
        plant.isWilted = true;
        plant.plantObject.GetComponent<Image>().sprite = plant.wiltedSprite;
    }

    private void ToggleState(Plant plant)
    {
        SetPlantHealthy(plant);
        Button botaoPlanta = new Button();
        botaoPlanta.interactable = true;
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
