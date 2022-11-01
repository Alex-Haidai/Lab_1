
using static Lab_1.GameAccount;

namespace Lab_1
{
    public class GameAccount//клас ігрового акаунту
    {
        public string UserName { get; set; }//ім'я користувача
        public int GamesCount 
        {
            get 
            {
                return GameList.Count;
            }
        }//кількість ігор користувача
        private readonly List<Game> GameList = new();//список ігор користувача
        private static readonly List<GameAccount> s_accountList = new();//список усіх ігрових акаунтів
        public enum AllPossibleGameStatus//статус гри
        {
            Victory, Defeat
        }
        public int CurrentRating//поточний рейтинг коричтувача
        {
            get
            {
                int rating = 1;//ініціалізуємо початковий рейтинг
                foreach (var item in GameList)//цикл проходу по всіх іграх, в яких брав участь користувач
                {
                    if (item.WinnerAccount.UserName.Equals(UserName))//якщо перемога додаємо до поточного рейтингу рейтинг гри
                    {
                        rating += item.GameRate;
                    }
                    else//якщо поразка
                    {
                        if (rating - item.GameRate >= 1)//якщо рейтинг гри менший поточного рейтинга
                        {
                            rating -= item.GameRate;//віднімаємо від поточного
                        }
                        else
                        {
                            rating = 1;//якщо рейтинг гри більший за поточний присвоюємо йому мінімальне значення
                        }
                    }
                }
                return rating;
            }
        }

        public GameAccount(string name)//конструктор
        {
            UserName = name;
            GameAccountIsValid(this);
            s_accountList.Add(this);
        }

        private static void GameAccountIsValid(GameAccount currentAccount) //пеервірка нового акаунту
        {
            foreach (var item in s_accountList)
            {
                if (item.UserName == currentAccount.UserName)
                {
                    throw new InvalidDataException("An account with this name already exists");
                }
            }
        }

        public void WinGame(Game game)//метод, який запускається у разі виграної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам
            GameList.Add(game);
            game.WinnerAccountRate = CurrentRating;

        }

        public void LoseGame(Game game)//метод, який запускається у разі програної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам
            GameList.Add(game);
            game.LoserAccountRate = CurrentRating;

        }

        public string GetStats()//метод отримання статистики користувача
        {
            var gameReport = new System.Text.StringBuilder();//створюємо динамічний рядок
            gameReport.AppendLine("Opponent name\tGame status\tGame Rating\tGame Id\t\tCurrent rating");//додаємо значення статистики
            foreach (var item in GameList)//цикл проходу по всіх іграх, в яких брав участь користувач
            {
                if (item.WinnerAccount.UserName.Equals(UserName))
                {
                    gameReport.AppendLine($"{item.LoserAccount.UserName}\t\t{AllPossibleGameStatus.Victory}\t\t{item.GameRate}\t\t{item.GameId}\t\t{item.WinnerAccountRate}");//виводимо рядок інформації про поточну гру та користувача 
                }
                else
                {

                    gameReport.AppendLine($"{item.WinnerAccount.UserName}\t\t{AllPossibleGameStatus.Defeat}\t\t{item.GameRate}\t\t{item.GameId}\t\t{item.LoserAccountRate}");
                }
            }
            return gameReport.ToString();
        }
    }

    public class Game//клас гри
    {
        public GameAccount WinnerAccount { get; set; }
        public GameAccount LoserAccount { get; set; }
        public  int GameId { get; set; }
        private static int gameUniqueIndex = 1;
        public int GameRate { get; }//рейтинг гри
        public int WinnerAccountRate { get; set; }
        public int LoserAccountRate { get; set; }


        public Game(GameAccount firstAccount, GameAccount secondAccount, int gameRate)//конструктор
        {
            WinnerAccount = firstAccount;
            LoserAccount = secondAccount;
            GameRate = gameRate;
            GameId = gameUniqueIndex;
            gameUniqueIndex++;
        }

        public static void MakeGame(GameAccount firstAccount, GameAccount secondAccount, int gameRate, AllPossibleGameStatus status)
        {
            GameIsValid(firstAccount, secondAccount, gameRate);
            if (status.Equals(AllPossibleGameStatus.Victory))
            {
                var game = new Game(firstAccount, secondAccount, gameRate);//створюємо об'єкт класу гри
                firstAccount.WinGame(game);
                secondAccount.LoseGame(game);
            }
            else
            {
                var game = new Game(secondAccount, firstAccount, gameRate);//створюємо об'єкт класу гри
                firstAccount.LoseGame(game);
                secondAccount.WinGame(game);
            }
        }

        private static void GameIsValid(GameAccount currentAccount, GameAccount opponentAccount, int rating) //метод перевірки гри
        {
            if (rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating of game must be positive");//якщо рейтинг гри від'ємний викидаємо виключення
            }
            if (currentAccount.Equals(opponentAccount))
            {
                throw new InvalidOperationException("You can`t play with yourself!");//якщо акаунти однакові викидаємо виключення
            }
        }
    }
}