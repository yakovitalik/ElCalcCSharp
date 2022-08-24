// Программа для расчета электрических величин
// Консольный вариант на языке C#
// Автор: Виталий Яковлев, yakovitalik@mail.ru

using System;

namespace ElCalc
{
    // Класс вывода меню программы
    class Menu
    {
        // Приветствие программы
        public void Hello()
        {
            Console.WriteLine("Вас привествует программа для расчета электрических величин!");
            Console.WriteLine("Автор: Виталий Яковлев, yakovitalik@mail.ru");
        }

        // основное меню выбора расчета
        public void ShowMenu()
        {
            Console.WriteLine("Выберите вариант расчета(Введите цифру): \n");
            Console.WriteLine("1. Расчет мощности трансформатора по активной мощности");
            Console.WriteLine("2. Расчет полной мощности по активной и реактивной");
            Console.WriteLine("3. Расчет тока и косинуса по мощностям");
            Console.WriteLine("4. Расчет загрузки трансформатора по стороне 0,4кВ");
            Console.WriteLine("Для выхода из программы нажмите клавишу 'q':");
        }

        // Меню выбора косинуса
        public void ChLoad()
        {
            Console.WriteLine("Выберите тип нагрузки:");
            Console.WriteLine("1. Коммунально-бытовая (cos = 0,93)");
            Console.WriteLine("2. Производственная (cos = 0,75)");
            Console.WriteLine("3. Смешанная (cos = 0,85)");
        }
    }

    // Общий класс элекроустановок
    class Elec
    {
        public const double Sqr3 = 1.73;            // корень из 3
        public const double cos1 = 0.93;            // коэффициент мощности для комунально-бытовой нагрузки
        public const double cos2 = 0.75;            // коэффициент мощности для производственной нагрузки
        public const double cos3 = 0.85;            // коэффициент для смешанной нагрузки
        public const double volNn = 230.0;          // напряжение берется 230 Вольт

        protected double actpower;                  // активная мощность, кВт
        protected double reactpower;                // реактивная мощность, кВар
        protected double fullpower;                 // полная мощность, кВА
        protected double cosin;                     // косинус (коэффициент мощности)

        // Получение активной мощности.
        public void SetAct()
        {
            Console.WriteLine("Введите значение активной мощности, кВт: ");
            actpower = Convert.ToDouble(Console.ReadLine());
        }

        // Получение реактивной мощности.
        public void SetReact()
        {
            Console.WriteLine("Введите значение реактивной мощности, кВар: ");
            reactpower = Convert.ToDouble(Console.ReadLine());
        }

        // Определение косинуса(cosin) по типу нагрузки (для расчета 1).
        public void SetCosR1()
        {
            string changeCos;             // переменная для хранения выбора типа нагрузки
            changeCos = Console.ReadLine();

            if (changeCos == "1")
                cosin = cos1;
            else if (changeCos == "2")
                cosin = cos2;
            else if (changeCos == "3")
                cosin = cos3;
            else
            {
                Console.WriteLine("Некорректное значение, выбран косинус по умолчанию (0,93)");
                cosin = cos1;
            }
        }
        // Определение косинуса по величине активной и полной мощности(для расчета3).
        public void SetCosR3()
        {
            cosin = actpower / fullpower;
        }

        // Рассчет полной мощности по активной и коэф.мощности(для расчета 1)
        public void SetFull1()
        {
            Console.WriteLine("Выполняем расчет мощности трансформатора по активной мощности");
            fullpower = actpower / cosin;
        }

        // Рассчет полной мощности по активной и реактивной(для расчета 2)
        public void SetFull2()
        {
            fullpower = Math.Sqrt((actpower * actpower) + (reactpower * reactpower));
        }

        // Вывод результатов расчета полной мощности на экран
        public void ShowResult()
        {
            Console.WriteLine("Расчет выполнен!");
            Console.WriteLine("Полная мощность будет равна: " + fullpower + " кВА");
        }
    }

    // Класс трансфораторов, производный от электроустановок
    class Trans : Elec
    {
        double PowTr;               // установленная мощность трансформатора
        double iA;                  // ток по фазе A для Рачета 4
        double iB;                  // ток по фазе B для Рачета 4
        double iC;                  // ток по фазе C для Рачета 4
        double sA;                  // мощность по фазе A для Рачета 4
        double sB;                  // мощность по фазе B для Рачета 4
        double sC;                  // мощность по фазе C для Рачета 4
        double sT;                  // сумма мощностей по фазам для Рачета 4
        double kzagr;               // коэффициент загрузки трансформатора (в процентах)

        // Ввод мощности и токов пользователем по фазам
        public void SetPowCurrent()
        {
            Console.WriteLine("Введите значение мощности трансформатора, кВА: ");
            PowTr = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите значение тока по фазе А, А: ");
            iA = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите значение тока по фазе B, А: ");
            iB = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите значение тока по фазе C, А: ");
            iC = Convert.ToDouble(Console.ReadLine());
        }

        // Расчет мощностей и коэффициента загрузки (Расчет 4)
        public void SetPower()
        {
            sA = Elec.volNn * iA;
            sB = Elec.volNn * iB;
            sC = Elec.volNn * iC;
            sT = sA + sB + sC;
            kzagr = ((sT / 1000) / PowTr) * 100;
        }

        // Вывод результатов расчета 4 на экран
        public void ShowResult4()
        {
            Console.WriteLine("Загрузка трансформатора составляет: " + kzagr + " %");
        }
    }

    // Класс линий электропередачи
    class Line : Elec
    {
        public double current;                 // Ток, А
        public double voltage;                 // Напряжение, кВ

        // Получение напряжения от пользователя
        public void GetVoltage()
        {
            Console.WriteLine("Введите значение напряжения, кВ: ");
            voltage = Convert.ToDouble(Console.ReadLine());
        }

        // Рассчет тока
        public void SetCurrent()
        {
            current = fullpower / (Sqr3 * voltage);
        }

        // Вывод результатов расчет 3
        public void ShowResultR3()
        {
            Console.WriteLine("Полная мощность будет равна: " + fullpower + " кВА");
            Console.WriteLine("Cos ф равен: " + cosin);
            Console.WriteLine("Ток равен: " + current + " А");
        }
    }

    class ElecCalc
    {
        static void Main()
        {
            // Создаем объекты классов
            Menu m1 = new Menu();
            Elec e1 = new Elec();
            Elec e2 = new Elec();
            Trans t1 = new Trans();
            Line l1 = new Line();
            string choise;                    // номер выбора пользователя из меню
            m1.Hello();                     // приветствие программы

            for (; ; )
            {
                do
                {
                    m1.ShowMenu();
                    choise = Console.ReadLine();

                    if (choise == "q")
                    {
                        return;
                    }

                    if (choise == "1")          // расчет1: полной мощности трансформатора активной и коэф.мощности
                    {
                        e1.SetAct();            // ввод активки
                        m1.ChLoad();            // отображение меню выбора типа нагрузки
                        e1.SetCosR1();          // выбор типа нагрузки
                        e1.SetFull1();          // выполняем рассчет;
                        e1.ShowResult();        // вывод результатов рассчета
                    }

                    else if (choise == "2")     // расчет2: полной мощности по активке и реактивке
                    {
                        e2.SetAct();            // ввод активки
                        e2.SetReact();          // ввод реактивки
                        e2.SetFull2();          // расчет полной мощности
                        e2.ShowResult();        // вывод результатов рассчета
                    }

                    else if (choise == "3")     // расчет3: параметров ВЛ: косинуса, тока
                    {
                        l1.SetAct();            // ввод активки
                        l1.SetReact();          // ввод реактивки
                        l1.GetVoltage();        // ввод напряжения
                        l1.SetFull2();          // расчет полной мощности
                        l1.SetCosR3();          // расчет косинуса
                        l1.SetCurrent();        // расчет тока
                        l1.ShowResultR3();      // вывод результатов рассчета
                    }

                    else if (choise == "4")
                    {
                        t1.SetPowCurrent();     // ввод значений
                        t1.SetPower();          // расчет значений
                        t1.ShowResult4();       // вывод результатов значений
                    }

                    else
                    {
                        Console.WriteLine("Некорректное значение!");
                    }
                } while (choise != "q");

            }
        }
    }
}