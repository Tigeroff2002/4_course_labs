using System;

namespace EmelyanovDmitry_PRI117_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            // переменная, которая будет хранить команду пользователя
            string user_command = "";

            // бесконечный цикл
            bool Infinity = true;

            // пустой (раный null) экземпляр класса Man
            Man SomeMan = null;

            while (Infinity) // пока Infinity равно true
            {
                // приглашение ввести команду
                Console.WriteLine("Пожалуйста, введите команду\n");

                // получение строки (команды) от пользователя
                user_command = Console.ReadLine();

                // обработка команды с помощью оператора ветвления
                switch (user_command)
                {
                    // если user_command содержит строку exit
                    case "exit":
                        {
                            Infinity = false;
                            // теперь цикл завершиться, и программа завершит свое выполнение
                            break;
                        }

                    // если user_command содержит строку help
                    case "help":
                        {
                            Console.WriteLine("\nСписок команд:");
                            Console.WriteLine("---");

                            Console.WriteLine("create_man : команда создает человека, (экземпляр класса Man)");
                            Console.WriteLine("kill_man : команда убивает человека");
                            Console.WriteLine("talk : команда застравляет человека говорить (если создан экземпляр класса)");
                            Console.WriteLine("go : команда застравляет человека идти (если создан экземпляр класса)");
                            Console.WriteLine("minus_hp: уменьшить здоровье человека (если создан экземпляр класса)");
                            Console.WriteLine("roulette : игра в русскую рулетку (если создан экземпляр класса)");

                            Console.WriteLine("---");
                            Console.WriteLine("---\n");

                            break;
                        }

                    case "create_man":
                        {
                            // проверяем, создан ли уже какой либо человек
                            if (SomeMan != null)
                            {
                                // человек уже существует.
                                // убиваем его
                                // (это не обязательная операция синтаксиса языка
                                // а всего лишь каприз автора кода :), вы можите пропустить эту строку)

                                SomeMan.Kill();

                                // создаем нового

                            }

                            // просим ввести имя человека
                            Console.WriteLine("Пожалуйста, введите имя создаваемого человека \n");

                            // получаем строку введенную пользователем
                            user_command = Console.ReadLine();

                            // создаем новый объект в памяти, в качесвте параметра
                            // передаем имя человека
                            SomeMan = new Man(user_command);

                            // сообщаем о создании
                            Console.WriteLine("Человек успешно создан \n");


                            break;
                        }

                    case "kill_man":
                        {
                            // проверяем, что объект существует в памяти
                            if (SomeMan != null)
                            {
                                // вызываем фукнцию сметри
                                SomeMan.Kill();
                            }
                            else // иначе
                            {
                                Console.WriteLine("Человек не создан. Вы не можете его убить");
                            }

                            break;
                        }

                    case "talk":
                        {
                            // проверяем, что объект существует в памяти
                            if (SomeMan != null)
                            {
                                // вызываем фукнцию разговора
                                SomeMan.Talk();
                            }
                            else // иначе
                            {
                                Console.WriteLine("Человек не создан. Команда не может быть выполнена");
                            }

                            break;
                        }

                    case "go":
                        {
                            // проверяем, что объект существует в памяти
                            if (SomeMan != null)
                            {
                                // вызываем фукнцию передвижения
                                SomeMan.Go();
                            }
                            else
                            {
                                Console.WriteLine("Человек не создан. Команда не может быть выполнена");
                            }

                            break;
                        }
                    case "minus_hp":
                        {
                            // проверяем, что объект существует в памяти
                            if (SomeMan != null)
                            {
                                // вызываем фукнцию уменьшения здоровья
                                SomeMan.DecreaseHealth();
                            }
                            else
                            {
                                Console.WriteLine("Человек не создан. Команда не может быть выполнена");
                            }

                            break;
                        }
                    case "roulette":
                        {
                            // проверяем, что объект существует в памяти
                            if (SomeMan != null)
                            {
                                int betNumber;
                                Console.WriteLine("Введи число (1-10):");
                                do
                                {
                                    if (int.TryParse(Console.ReadLine(), out betNumber))
                                    {
                                        if (betNumber < 11 && betNumber > 0)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Введите число патрон <11 и >0");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Невалидное число, давай еще раз");
                                    }
  
                                } while (true);
                                
                                // вызываем фукнцию игры на смерть
                                SomeMan.PlayOnDeath(betNumber);
                            }
                            else
                            {
                                Console.WriteLine("Человек не создан. Команда не может быть выполнена");
                            }

                            break;
                        }
                    // если команду определить не удалось
                    default:
                        {
                            Console.WriteLine("Ваша команда не определена, пожалуйста повторите снова\n");
                            Console.WriteLine("Для вывода списка команд введите команду help");
                            Console.WriteLine("Для завершения программы введите команду exit\n\n");
                            break;
                        }
                }
            }
        }
    }
}
