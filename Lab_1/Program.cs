
namespace Lab_1
{
    public class GameAccount//клас ігрового акаунту
    {
        public string UserName { get; set; }//ім'я користувача
        public int GamesCount { get; set; }//кількість ігор користувача

        private List<Game> gameList = new();//список ігор користувача
        private static List<GameAccount> accountList = new();//список усіх ігрових акаунтів
        public enum AllPossibleGameStatus//статус гри
        {
            Victory, Defeat, Tie
        }
        public int CurrentRating//поточний рейтинг коричтувача
        {
            get
            {
                int rating = 1;//ініціалізуємо початковий рейтинг
                foreach (var item in gameList)//цикл проходу по всіх іграх, в яких брав участь користувач
                {
                    if (item.CurrentGameStatus == AllPossibleGameStatus.Victory)//якщо перемога додаємо до поточного рейтингу рейтинг гри
                    {
                        rating += item.GameRate;
                    }
                    else if (item.CurrentGameStatus == AllPossibleGameStatus.Defeat)//якщо поразка
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
            accountList.Add(this);
        }

        private static void GameAccountIsValid(GameAccount currentAccount) //пеервірка нового акаунту
        {
            foreach (var item in accountList)
            {
                if (item.UserName == currentAccount.UserName)
                {
                    throw new InvalidDataException("An account with this name already exists");
                }
            }
        }

        public void WinGame(GameAccount opponentAccount, int rating, bool recordGameForBothPlayers)//метод, який запускається у разі виграної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам
            GameIsValid(opponentAccount, rating);
            var game = new Game(opponentAccount, rating, AllPossibleGameStatus.Victory);//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            game.PlayerCurrentRating = CurrentRating;
            if (recordGameForBothPlayers == true)
            {
                opponentAccount.LoseGame(this, rating, false);//якщо маркер позитивний, викликаємо метод поразки для акаунта опонента з протилежним маркером
                MakeTheSameGameId(game.GameId, opponentAccount);
            }

        }

        public void LoseGame(GameAccount opponentAccount, int rating, bool recordGameForBothPlayers)//метод, який запускається у разі програної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам
            GameIsValid(opponentAccount, rating);
            var game = new Game(opponentAccount, rating, AllPossibleGameStatus.Defeat);//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            game.PlayerCurrentRating = CurrentRating;
            if (recordGameForBothPlayers == true)
            {
                opponentAccount.WinGame(this, rating, false);//якщо маркер позитивний, викликаємо метод перемоги для акаунта опонента з протилежним маркером
                MakeTheSameGameId(game.GameId, opponentAccount);
            }

        }

        public void TieGame(GameAccount opponentAccount, int rating, bool recordGameForBothPlayers)//метод, який запускається у разі нічиєї
        {
            GameIsValid(opponentAccount, rating);
            var game = new Game(opponentAccount, rating, AllPossibleGameStatus.Tie);//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            game.PlayerCurrentRating = CurrentRating;
            if (recordGameForBothPlayers == true)
            {
                opponentAccount.TieGame(this, rating, false);//якщо маркер позитивний, викликаємо метод нічиєї для акаунта опонента з протилежним маркером
                MakeTheSameGameId(game.GameId, opponentAccount);
            }
        }

        private void GameIsValid(GameAccount opponentAccount, int rating) //метод перевірки гри
        {
            if (rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating of game must be positive");//якщо рейтинг гри від'ємний викидаємо виключення
            }
            if (opponentAccount == this)
            {
                throw new InvalidOperationException("You can`t play with yourself!");//якщо акаунти однакові викидаємо виключення
            }
        }

        private void MakeTheSameGameId(string gameId, GameAccount opponentAccount)//метод, що робить однаковим id гри для обох користувачів
        {
            for (int i = 0; i < opponentAccount.gameList.Count; i++)
            {
                if (i == opponentAccount.gameList.Count - 1)
                {
                    opponentAccount.gameList[i].GameId = this.gameList[i].GameId;
                }

            }
        }

        public string GetStats()//метод отримання статистики користувача
        {
            var gameReport = new System.Text.StringBuilder();//створюємо динамічний рядок
            gameReport.AppendLine("Opponent name\tGame status\tGame Rating\tGame Id\t\tCurrent rating");//додаємо значення статистики
            foreach (var item in gameList)//цикл проходу по всіх іграх, в яких брав участь користувач
            {
                gameReport.AppendLine($"{item.OpponentName}\t\t{item.CurrentGameStatus}\t\t{item.GameRate}\t\t{item.GameId}\t\t{item.PlayerCurrentRating}");//виводимо рядок інформації про поточну гру та користувача 
            }
            return gameReport.ToString();
        }
    }

    public class Game//клас гри
    {
        public string GameId { get; set; }
        private static int gameUniqueIndex = 12345432;
        public string OpponentName { get; }//назва акаунту опонента
        public GameAccount.AllPossibleGameStatus CurrentGameStatus { get; set; }
        public int GameRate { get; }//рейтинг гри
        public int PlayerCurrentRating { get; set; }

        public Game(GameAccount opponentAccount, int gameRate, GameAccount.AllPossibleGameStatus status)//конструктор
        {
            OpponentName = opponentAccount.UserName;
            GameRate = gameRate;
            CurrentGameStatus = status;
            GameId = gameUniqueIndex.ToString();
            gameUniqueIndex++;
        }
    }
}