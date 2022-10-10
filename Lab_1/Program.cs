
namespace Lab_1
{
    public class GameAccount//клас ігрового акаунту
    {
        public string UserName { get; set; }//ім'я користувача
        public int CurrentRating//поточний рейтинг коричтувача
        {
            get
            {
                int rating = 1;//ініціалізуємо початковий рейтинг
                foreach (var item in gameList)//цикл проходу по всіх іграх, в яких брав участь користувач
                {
                    if (item.GameStatus == "Victory")//якщо перемога додаємо до поточного рейтингу рейтинг гри
                    {
                        rating += item.GameRate;
                    }
                    else if (item.GameStatus == "Defeat")//якщо поразка
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
        public int GamesCount { get; set; }//кількість ігор користувача
        List<Game> gameList = new List<Game>();//список ігор користувача

        public GameAccount(string name)//конструктор
        {
            UserName = name;
        }

        public void WinGame(GameAccount opponentAccount, int rating, bool flag)//метод, який запускається у разі виграної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам

            if (rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating of game must be positive!");//якщо рейтинг гри від'ємний викидаємо виключення
            }
            if (opponentAccount.UserName == UserName)
            {
                throw new InvalidOperationException("You can`t play with yourself!");//якщо імена акаунтів однакові викидаємо виключення
            }
            var game = new Game(opponentAccount, rating, "Victory");//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            if (flag == true)
            {
                opponentAccount.LoseGame(this, rating, false);//якщо маркер позитивний, викликаємо метод поразки для акаунта опонента з протилежним маркером
            }

        }

        public void LoseGame(GameAccount opponentAccount, int rating, bool flag)//метод, який запускається у разі програної гри
        {//на вхід приймає об'єкт акаунту опонента, рейтинг гри та маркер, що визначає чи зарахується гра обом користувачам

            if (rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating of game must be positive");//якщо рейтинг гри від'ємний викидаємо виключення
            }
            if (opponentAccount.UserName == UserName)
            {
                throw new InvalidOperationException("You can`t play with yourself!");//якщо імена акаунтів однакові викидаємо виключення
            }
            var game = new Game(opponentAccount, rating, "Defeat");//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            if (flag == true)
            {
                opponentAccount.WinGame(this, rating, false);//якщо маркер позитивний, викликаємо метод перемоги для акаунта опонента з протилежним маркером
            }

        }

        public void TieGame(GameAccount opponentAccount, int rating, bool flag)//метод, який запускається у разі нічиєї
        {
            if (rating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating of game must be positive");//якщо рейтинг гри від'ємний викидаємо виключення
            }
            if (opponentAccount.UserName == UserName)
            {
                throw new InvalidOperationException("You can`t play with yourself!");//якщо імена акаунтів однакові викидаємо виключення
            }
            var game = new Game(opponentAccount, rating, "Tie");//створюємо об'єкт класу гри
            gameList.Add(game);//додаємо об'єкт до списку ігор
            GamesCount++;//додаємо лічильник ігор користувача
            if (flag == true)
            {
                opponentAccount.TieGame(this, rating, false);//якщо маркер позитивний, викликаємо метод нічиєї для акаунта опонента з протилежним маркером
            }

        }

        public string GetStats()//метод отримання статистики користувача
        {
            var gameReport = new System.Text.StringBuilder();//створюємо динамічний рядок
            gameReport.AppendLine("Opponent name\tGame status\tGame Rating\tGame Index\tCurrent rating");//додаємо значення статистики
            int rating = 1;//змінна для поточного рейтингу користувача на кожній грі
            int gameIndex = 0;//змінна для індексування ігор
            foreach (var item in gameList)//цикл проходу по всіх іграх, в яких брав участь користувач
            {
                if (item.GameStatus == "Victory")
                {
                    rating += item.GameRate;//якщо перемога додаємо рейтинг гри до рейтингу користувача
                }
                else if (item.GameStatus == "Defeat")//якщо поразка
                {
                    if (rating - item.GameRate >= 1)//якщо рейтинг гри менший поточного рейтингу
                    {
                        rating -= item.GameRate;//віднімаємо від поточного рейтинг гри
                    }
                    else//якщо рейтинг гри більший або дорівнює поточному рейтингу
                    {
                        rating = 1;//присвоюємо мінімальне значення рейтингу
                    }
                }
                gameIndex++;//додаємо індекс гри
                gameReport.AppendLine($"{item.OpponentName}\t\t{item.GameStatus}\t\t{item.GameRate}\t\t{gameIndex}\t\t{rating}");//виводимо рядок інформації про поточну гру та користувача 
            }
            return gameReport.ToString();
        }
    }

    public class Game//клас гри
    {
        public string OpponentName { get; }//назва акаунту опонента
        public string GameStatus { get; set; }//статус гри
        public int GameRate { get; }//рейтинг гри

        public Game(GameAccount opponentAccount, int gameRate, string status)//конструктор
        {
            OpponentName = opponentAccount.UserName;
            GameRate = gameRate;
            GameStatus = status;
        }
    }

}