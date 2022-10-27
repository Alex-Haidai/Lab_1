
namespace Lab_1
{
    internal class MainClass//головний клас
    {
        static void Main(string[] args)
        {
            var firstAccount = new GameAccount("Mike");//створюємо 2 ігрових акаунти
            var secondAccount = new GameAccount("Jimmy");

            Console.WriteLine($"First player -  {firstAccount.UserName}");//виводимо їх імена
            Console.WriteLine($"Second player - {secondAccount.UserName}\n");

            firstAccount.WinGame(secondAccount, 1, true);//робимо імітацію декількох ігор (true - маркер для зарахування гри до статистики обох гравців)
            secondAccount.LoseGame(firstAccount, 1, true);
            firstAccount.LoseGame(secondAccount, 5, true);
            secondAccount.LoseGame(firstAccount, 3, true);
            secondAccount.LoseGame(firstAccount, 0, true);
            firstAccount.TieGame(secondAccount, 3, true);

            Console.WriteLine($"{firstAccount.UserName} statistics:\n");//виводимо статистику обох гравців
            Console.WriteLine(firstAccount.GetStats());
            Console.WriteLine($"\n{secondAccount.UserName} statistics:\n");
            Console.WriteLine(secondAccount.GetStats());

            Console.WriteLine($"\nNumber of {firstAccount.UserName} games: {firstAccount.GamesCount}, current rating = {firstAccount.CurrentRating}\n");
            //виводимо кількість ігор кожного гравця та поточний рейтинг
            Console.WriteLine($"Number of {secondAccount.UserName} games: {secondAccount.GamesCount}, current rating = {secondAccount.CurrentRating}");


        }
    }
}