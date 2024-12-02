using System;
using System.Collections.Generic;

namespace Aquarium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();

            aquarium.Execute();
        }
    }

    class Aquarium
    {
        private List<Fish> _fishes;
        private int _maxFishesCount;
        private bool _isExecute = true;

        public void Execute()
        {
            string askPrase = "Сколько рыбов вмещает этот аквариум: ";

            _maxFishesCount = UserUtils.GeneratePositiveNumber(askPrase);
            _fishes = new List<Fish>(_maxFishesCount);

            while (_isExecute)
            {
                Console.Clear();
                ShowFishes();
                RunMenu();
            }
        }

        private void ShowFishes()
        {
            if (HaveFish() == false)
            {
                Console.WriteLine("Рыбов нету. Добавьте рыбов.");
            }
            else
            {
                int count = 1;

                foreach (Fish fish in _fishes)
                {
                    Console.Write(count + " ");
                    fish.ShowInfo();
                    count++;
                }

                Console.WriteLine($"Вместимость {_fishes.Count} из {_maxFishesCount}");
            }
        }

        private void RunMenu()
        {
            string askPhrase = "Ваш выбор: ";
            ShowMenu();

            switch ((MenuActions)UserUtils.GeneratePositiveNumber(askPhrase))
            {
                case MenuActions.AddFish:
                    AddFish();
                    break;
                case MenuActions.DeleteFish:
                    DeleteFish();
                    break;

                case MenuActions.SpendTime:
                    SpendTime();
                    break;

                case MenuActions.Exit:
                    Exit();
                    break;

                default:
                    Console.WriteLine("Не понимаю что нужно.");
                    break;
            }
        }

        private void ShowMenu()
        {
            string indent = "\n";

            Console.WriteLine($"{indent}{(int)MenuActions.AddFish} - добавить рыбку;{indent}" +
                $"{(int)MenuActions.DeleteFish} - убрать рыбу по номеру;{indent}" +
                $"{(int)MenuActions.SpendTime} - просто понаблюдать;{indent}" +
                $"{(int)MenuActions.Exit} - выход.{indent}");
        }

        private void AddFish()
        {
            SpendTime();

            if (_fishes.Count < _maxFishesCount)
            {
                string fishName;
                int fishMaxAge;

                Console.Write("Введите название новой рыбки: ");
                fishName = Console.ReadLine();

                fishMaxAge = UserUtils.GeneratePositiveNumber("Введите срок жизни рыбки: ");

                _fishes.Add(new Fish(fishName, fishMaxAge));
            }
            else
            {
                Console.WriteLine("Рыбки больше не влезут. Это ж не селёдки в бочке.");
                UserUtils.PressAnyKeyToContinue();
            }
        }

        private void DeleteFish()
        {
            if (HaveFish())
            {
                int index;
                string askPhrase = "Введите номер рыбки которую хотите убрать из аквариума: ";

                index = UserUtils.GeneratePositiveNumber(askPhrase, _fishes.Count) - 1;

                _fishes.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Рыб итак нет.");
            }

            SpendTime();
        }

        private void SpendTime()
        {
            if (HaveFish())
            {
                foreach (Fish fish in _fishes)
                {
                    if (fish.IsAlive)
                    {
                        fish.GrowOld();
                    }
                }
            }
        }

        private void Exit()
        {
            Console.Clear();
            Console.WriteLine("Выход.");
            UserUtils.PressAnyKeyToContinue();
            _isExecute = false;
        }

        private bool HaveFish()
        {
            return _fishes.Count > 0;
        }

        private enum MenuActions
        {
            AddFish = 1,
            DeleteFish,
            SpendTime,
            Exit
        }
    }

    class Fish
    {
        private string _name;
        private int _currentAge;
        private int _maxAge;

        public Fish(string name, int maxAge)
        {
            _name = name;
            _maxAge = maxAge;
        }

        public bool IsAlive => _currentAge < _maxAge;

        public void ShowInfo()
        {
            Console.Write($"Рыба именуемая {_name} ");

            if (IsAlive)
            {
                Console.WriteLine($"жива и будет такой еще {_maxAge - _currentAge}");
            }
            else
            {
                Console.WriteLine("мертва.");
            }
        }

        public void GrowOld()
        {
            _currentAge++;
        }
    }

    static class UserUtils
    {
        public static int GeneratePositiveNumber(string askPhrase, int maxValue)
        {
            int userInput;

            do
            {
                Console.Write(askPhrase);
            }
            while (int.TryParse(Console.ReadLine(), out userInput) == false || userInput <= 0 || userInput > maxValue);

            return userInput;
        }

        public static int GeneratePositiveNumber(string askPhrase)
        {
            int userInput;

            do
            {
                Console.Write(askPhrase);
            }
            while (int.TryParse(Console.ReadLine(), out userInput) == false || userInput <= 0);

            return userInput;
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadKey(true);
        }
    }
}
