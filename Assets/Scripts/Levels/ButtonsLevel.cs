using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class ButtonsLevel : MonoBehaviour, IRewardedVideoAdListener
{
    // Ссылка на параметры игры
    private Parameters parameters;

    private void Awake() { parameters = Camera.main.GetComponent<Parameters>(); }

    private void Start()
    {
        // Активация обратных вызовов для видеорекламы
        Appodeal.setRewardedVideoCallbacks(this);
    }

    // Переключение паузы
    public void TogglePause(int time)
    {
        // Устанавливаем течение времени
        Time.timeScale = time;

        // Устанавливаем режим игры в зависимости от времени
        parameters.Mode = (time == 0) ? "pause" : "play";
    }

    // Определение варианта оживления
    public void IdentifyTypeRespawn()
    {
        // Если достаточно монет
        if (PlayerPrefs.GetInt("coins") >= 50)
        {
            // Вычитаем стоимость оживления
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 50);
            // Запускаем оживления персонажа
            RunCharacterRespawn();
        }
        else
        {
            // Иначе проверяем предзагрузку видеорекламы
            Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
            // Отображаем рекламу
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }

        // Если доступен интернет
        if (Application.internetReachability != NetworkReachability.NotReachable)
            // Разблокируем достижение (только не сначала)
            PlayServices.UnlockingAchievement(GPGSIds.achievement_10);
    }

    // Запуск оживления персонажа
    private void RunCharacterRespawn()
    {
        // Скрываем панель затемнения, панель подсказок, рамку для кнопок, панель поражения и панель монет
        parameters.interfaceElements[(int)Parameters.InterfaceElements.Blackout].SetActive(false);
        parameters.interfaceElements[(int)Parameters.InterfaceElements.HintPanel].SetActive(false);
        parameters.interfaceElements[(int)Parameters.InterfaceElements.Frame].SetActive(false);
        parameters.interfaceElements[(int)Parameters.InterfaceElements.LosePanel].SetActive(false);
        parameters.interfaceElements[(int)Parameters.InterfaceElements.CoinsPanel].SetActive(false);
        // Отображаем кнопку паузы
        parameters.interfaceElements[(int)Parameters.InterfaceElements.PauseButton].SetActive(true);

        // Активируем игровой режим
        parameters.Mode = "play";

        // Оживляем персонажа
        parameters.Character.CharacterRespawn();
        // Продолжаем отсчет времени прохождения
        parameters.StartCoroutine("CountTime");

        // Восстанавливаем приостановленные методы
        parameters.StartLevel.Invoke();
    }

    #region Appodeal
    // Обратные вызовы для видеорекламы с вознаграждением
    public void onRewardedVideoLoaded(bool isPrecache) { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoClosed(bool finished) { }
    // Если реклама полностью просмотрена, активируем оживление персонажа
    public void onRewardedVideoFinished(double amount, string name) { RunCharacterRespawn(); }
    public void onRewardedVideoClicked() { }
    public void onRewardedVideoExpired() { }
    #endregion

    // Завершение уровня
    public void CompleteLevel()
    {
        // Скрываем кнопку паузы
        parameters.interfaceElements[(int)Parameters.InterfaceElements.PauseButton].SetActive(false);
        // Переключаем режим игры на победу
        parameters.Mode = "victory";
        // Отображаем результат уровня
        parameters.Invoke("ShowResults", 0.3f);
    }
}