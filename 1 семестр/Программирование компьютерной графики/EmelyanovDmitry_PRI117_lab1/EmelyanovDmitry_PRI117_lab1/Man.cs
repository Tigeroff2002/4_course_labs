using System;

namespace EmelyanovDmitry_PRI117_lab1
{
    public class Man
    {
        // конструктор класса (данная функция вызывается
        // при создании нового экземпляра класса

        public Man(string _name)
        {
            // получаем имя человека из входного праметра
            // конструктора класса
            Name = _name;
            // экземпляр жив 
            isLife = true;

            // генерируем возраст от 15 до 50
            Age = (uint)rnd.Next(15, 50);
            // и здоровье, от 10 до 100%
            Health = (uint)rnd.Next(10, 100);
        }

        // экземпляр класса Random
        // для генерации случайных чисел
        private Random rnd = new Random();

        // закрытые члены, которые нельзя именить
        // извне класса

        // строка, содержащая имя
        private string Name;

        // беззнаковое целое число, содержащая возраст
        private uint Age;

        // беззнаковое целое число, отражающее уровень здоровья
        private uint Health;

        // булево, означающее жив ли данный человек
        private bool isLife;

        // заготоква функции "говорить"
        public void Talk()
        {
            // генерируем случайное число от 1 до 3
            int random_talk = rnd.Next(1, 3);

            // объявляем переменную, в которой мы будем хранить
            // строку

            string tmp_str = "";

            // в зависимости от случ значения random_talk
            switch (random_talk)
            {
                case 1: // если 1 - называем свое имя
                    {
                        // генериуем текст сообщения 
                        tmp_str = "Привет, меня зовут " + Name + ", рад познакомиться";

                        // завершаем оператор выбора
                        break;
                    }
                case 2: // возраст
                    {
                        // генериуем текст сообщения 
                        tmp_str = "Мне " + Age + ". А тебе?";

                        break;
                    }
                case 3: // говорим о своем здоровье
                    {
                        // в зависимости от параметра здоровья
                        if (Health > 50)
                            tmp_str = "Да я зводоров как бык!";
                        else
                            tmp_str = "Со здоровьем у меня хреново, дожить бы до " + (Age + 10).ToString();

                        // завершаем оператор выбора
                        break;
                    }
            }

            // выводим в консоль сгенерированное сообщение
            Console.WriteLine(tmp_str);

        }

        // заготовка функции "идти"
        public void Go()
        {
            // если объект жив
            if (isLife == true)
            {
                // если показатель более 40
                // считаем объект здоровым
                if (Health > 40)
                {
                    // генерируем строку текста
                    string outString = Name + " мирно прогуливается по городу";
                    // выводим в консоль
                    Console.WriteLine(outString);
                }
                else
                {
                    // генерируем строку текста
                    string outString = Name + " болен и не может гулять по городу";
                    // выводим в консоль
                    Console.WriteLine(outString);
                }
            }
            else
            {
                // генерируем строку текста
                string outString = Name + " не может идти, он умер";
                Console.WriteLine(outString);
            }

        }

        public void Kill()
        {
            // устанавливаем значение isLife (жив)
            // в false...
            isLife = false;
            Console.WriteLine(Name + " умер");
        }

        // функция, возвращающая показатель - жив ли данный человек.
        public bool IsAlive()
        {
            Console.WriteLine("LOG: Проверка живой ли человек. Emelyanov D.V. PRI-117");
            // возращаем значение, к которому мы не можем
            // обратиться на прямую из вне класса,
            //  так как оно имеет статус private
            return isLife;
        }

        // Выводид информацию о здоровье Man и уменьшает его на рандомное значение,
        // затем снова выводит информацию о здоровье Man
        public void DecreaseHealth()
        {
            // Если не жив, вывести ошибку и выйти из метода
            if (!IsAlive())
            {
                Console.WriteLine("LOG: Вывод сообщения что он уже умер. Emelyanov D.V. PRI-117");
                Console.WriteLine(Name + " уже умер!");
                return;
            }
            Console.WriteLine("LOG: Человек жив, выведем его здоровье. Emelyanov D.V. PRI-117");

            Console.WriteLine("Мое текущее здоровье: " + Health + " %");

            Console.WriteLine("Сейчас мы сделаем меньше...");

            int decresingValue;

            Console.WriteLine("LOG: Делаем здоровье человека меньше. Emelyanov D.V. PRI-117");
            // чтобы не уйти в минусовое значение, проверяем
            if (Health > 50)
            {
                decresingValue = rnd.Next(1, 50);
            }
            else
            {
                decresingValue = rnd.Next(1, (int)Health - 1);
            }

            // уменьшаем здоровье
            Health -= (uint)decresingValue;
            Console.WriteLine("LOG: Вывод текущего здоровья. Emelyanov D.V. PRI-117");
            Console.WriteLine("Мое текущее здоровье: " + Health + " %");
        }

        /// <summary>
        /// Сыграть в рулетку на смерть. Всего чисел в рулетке 10
        /// </summary>
        /// <param name="betNumber">
        /// Номер числа в рулетке
        /// </param>
        public void PlayOnDeath(int betNumber)
        {         
            // Если не жив, вывести ошибку и выйти из метода
            if (!IsAlive())
            {
                Console.WriteLine("LOG: Вывод сообщения что он уже умер. Emelyanov D.V. PRI-117");
                Console.WriteLine(Name + " уже умер!");
                return;
            }

            Console.WriteLine("LOG: Высчитываем случайное число. Emelyanov D.V. PRI-117");
            // Имитируем выпавшое число в рулетке
            var deathNumber = rnd.Next(1, 11);

            Console.WriteLine("LOG: Сравниваем случайное число с числом пользователя. Emelyanov D.V. PRI-117");
            if (betNumber == deathNumber)
            {
                Console.WriteLine("LOG: Человек выжил. Emelyanov D.V. PRI-117");
                Console.WriteLine(Name + " выжил");
            }
            else
            {
                Console.WriteLine("LOG: Человек умер. Emelyanov D.V. PRI-117");
                Kill();
            }
        }
    }

}
