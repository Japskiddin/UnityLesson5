using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // количество ячеек сетки и их расстояние друг от друга
    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2.2f;
    public const float offsetY = 2.2f;
    [SerializeField] private MemoryCard originalCard; // ссылка на карты в сцене
    [SerializeField] private Sprite[] images; // массив для ссылок на ресурсы-спрайты
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    [SerializeField] private TextMesh scoreLabel;

    public bool canReveal {
        get { return _secondRevealed == null; } // геттер, возвращающий значение false, если вторая карта уже открыта
    }

    public void CardRevealed(MemoryCard card) {
        if (_firstRevealed == null) { // сохраняем карты в одну из двух переменных, в зависимости от того, какая из них свободна
            _firstRevealed = card;
        } else {
            _secondRevealed = card;
            Debug.Log("Match ? " + (_firstRevealed.id == _secondRevealed.id)); // сравнение идентификаторов двух карт
            StartCoroutine(CheckMatch()); // вызывает сопрограмму после открытия двух карт
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPos = originalCard.transform.position; // положение первой карты

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 }; // целочисленный массив с парами идентификаторов для четырех спрайтов
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++) {
            for (int j = 0; j < gridRows; j++) {
                MemoryCard card; // ссылка на контейнер для исходной карты или её копий
                if (i == 0 && j == 0) {
                    card = originalCard;
                } else {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i; // идентификаторы получаем из перемешанного списка
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetX * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z); // значение Z не меняем
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // алгоритм Фишера-Йетса
    private int[] ShuffleArray(int[] numbers) {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++) {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    private IEnumerator CheckMatch() {
        if (_firstRevealed.id == _secondRevealed.id) {
            _score++; // увеличиваем счет при совпадении
            Debug.Log("Score: " + _score);
            scoreLabel.text = "Score: " + _score;
        } else {
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal(); // закрываем несовпадающие карты
            _secondRevealed.Unreveal();
        }

        _firstRevealed = null;
        _secondRevealed = null; // очищаем переменные
    }

    public void Restart() {
        SceneManager.LoadScene("SampleScene");
    }
}
