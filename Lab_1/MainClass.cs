
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

            Game.MakeGame(firstAccount,secondAccount, 1,GameAccount.AllPossibleGameStatus.Victory);//робимо імітацію декількох ігор (true - маркер для зарахування гри до статистики обох гравців)
            Game.MakeGame(secondAccount, firstAccount, 1, GameAccount.AllPossibleGameStatus.Defeat);
            Game.MakeGame(firstAccount, secondAccount, 5,GameAccount.AllPossibleGameStatus.Defeat);
            Game.MakeGame(secondAccount, firstAccount, 3, GameAccount.AllPossibleGameStatus.Defeat);
            Game.MakeGame(secondAccount, firstAccount, 0, GameAccount.AllPossibleGameStatus.Defeat);
         

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