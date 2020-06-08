namespace Cubra
{
    public class Coin : ObjectsToCollect
    {
        /// <summary>
        /// Действия при касании персонажа с коллайдером
        /// </summary>
        /// <param name="character">персонаж</param>
        public override void ActionsOnEnter(Character character)
        {
            // Устанавливаем звук и воспроизводим
            character.SetSound(Character.Sounds.Coin);
            character.PlayingSound.PlaySound();

            // Увеличиваем количество монет
            Main.Instance.Coins++;

            // Отключаем объект
            InstanseObject.SetActive(false);
        }
    }
}