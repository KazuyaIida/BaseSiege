using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SpawnManager spawnManager;
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI cpuEnergyText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton; // ���X�^�[�g�{�^��
    public int playerEnergy = 0;
    public int cpuEnergy = 0;
    public int energyPerSecond = 1;
    public int soldierCost = 5;
    public int baseCost = 10; // ���_�̏����R�X�g
    [HideInInspector] public bool isGameOver = false;

    void Start()
    {
        InvokeRepeating("IncreaseEnergy", 1f, 1f);
        UpdateEnergyUI();
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false); // ���X�^�[�g�{�^�����\��
        restartButton.onClick.AddListener(RestartGame); // �{�^���Ƀ��X�^�[�g���\�b�h��o�^
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && playerEnergy >= soldierCost)
            {
                playerEnergy -= soldierCost;
                spawnManager.SpawnPlayerSoldier();
                UpdateEnergyUI();
            }
            //else if (Input.GetKeyDown(KeyCode.B) && playerEnergy >= baseCost)
            //{
            //    playerEnergy -= baseCost;
            //    spawnManager.SpawnPlayerIntermediateBase();
            //    UpdateEnergyUI();
            //}

            if (Time.time % 3 < Time.deltaTime && cpuEnergy >= soldierCost)
            {
                cpuEnergy -= soldierCost;
                spawnManager.SpawnCPUSoldier();
                UpdateEnergyUI();
            }
            //else if (Time.time % 5 < Time.deltaTime && cpuEnergy >= baseCost)
            //{
            //    cpuEnergy -= baseCost;
            //    spawnManager.SpawnCPUIntermediateBase();
            //    UpdateEnergyUI();
            //}
        }
    }

    void IncreaseEnergy()
    {
        if (!isGameOver)
        {
            playerEnergy += energyPerSecond;
            cpuEnergy += energyPerSecond;
            UpdateEnergyUI();
        }
    }

    public void OnBaseDestroyed(string destroyedBaseTag)
    {
        if (!isGameOver)
        {
            if (destroyedBaseTag == "PlayerBase")
            {
                GameOver("You Lose!");
            }
            else if (destroyedBaseTag == "EnemyBase")
            {
                GameOver("You Win!");
            }
        }
    }

    void GameOver(string resultMessage)
    {
        isGameOver = true;
        gameOverText.text = resultMessage;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true); // ���X�^�[�g�{�^����\��
    }

    void UpdateEnergyUI()
    {
        playerEnergyText.text = "Player Energy: " + playerEnergy;
        cpuEnergyText.text = "CPU Energy: " + cpuEnergy;
    }

    // ���X�^�[�g���\�b�h
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���݂̃V�[���������[�h
    }
}
