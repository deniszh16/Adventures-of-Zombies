using System.Collections;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [Header("Время до создания")]
    [SerializeField] private float time;

    [Header("Пул объектов")]
    [SerializeField] private GameObject[] poolObjects;

    // Номер доступного объекта в пуле
    private byte objectNumber = 0;

    // Ссылка на компоненты
    private Parameters parameters;
    private AudioSource audioSource;

    private void Awake()
    {
        parameters = Camera.main.GetComponent<Parameters>();
        audioSource = GetComponent<AudioSource>();

        // Добавляем метод в событие
        parameters.StartLevel.AddListener(RunCoroutine);
    }

    private void RunCoroutine()
    {
        // Запускаем корутину по созданию объектов
        StartCoroutine(CreateObject());
    }

    // Создание объектов
    private IEnumerator CreateObject()
    {
        // Пока активен игровой режим
        while (parameters.Mode == "play")
        {
            yield return new WaitForSeconds(time);

            // Переносим объект из пула в место создания и устанавливаем угол
            poolObjects[objectNumber].transform.position = transform.position;
            poolObjects[objectNumber].transform.rotation = transform.rotation;
            // Активируем объект этот объект
            poolObjects[objectNumber].SetActive(true);

            // Увеличиваем номер доступного объекта
            objectNumber++;

            // Если номер выходит за пределы массива, сбрасываем номер
            if (objectNumber >= poolObjects.Length) objectNumber = 0;

            // Если звук не отключен и установлен звуковой файл, проигрываем его
            if (Options.sound && audioSource.clip) audioSource.Play();
        }
    }
}