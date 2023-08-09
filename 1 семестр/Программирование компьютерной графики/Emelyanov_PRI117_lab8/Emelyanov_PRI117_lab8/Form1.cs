using System;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;


namespace Emelyanov_PRI117_lab8
{
    public partial class Form1 : Form
    {

        // массив, в который будут заносится управляющие точки
        private float[,] DrawingArray = new float[64, 2]; // количество точек
        private int count_points = 0;

        // размеры окна
        double ScreenW, ScreenH;

        // отношения сторон окна визуализации
        // для корректного перевода координат мыши в координаты,
        // принятые в программе

        private float devX;
        private float devY;

        // вспомогательные переменные для построения линий от курсора мыши к координатным осям
        float lineX, lineY;

        // текущение координаты курсора мыши
        float Mcoord_X = 0, Mcoord_Y = 0;

        /*
        * Состояние захвата вершины мышью (при редактированиее)
        */

        // -1 означает, что нет захваченой вершины,
        // иначе номер указывает на элемент массива, хранящий захваченную вершину
        int captured = -1;

        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            // если режим редактирования сплайна
            if (comboBox1.SelectedIndex == 1)
            {
                // получаем и преобразовываем координаты нажатия
                Mcoord_X = e.X;
                Mcoord_Y = e.Y;

                lineX = devX * e.X;
                lineY = (float)(ScreenH - devY * e.Y);

                // проходим циклом по всем установленным контрольным точкам
                for (int ax = 0; ax < count_points; ax++)
                {
                    // если точка попадает под курсор
                    if (lineX < DrawingArray[ax, 0] + 5 && lineX > DrawingArray[ax, 0] - 5 && lineY < DrawingArray[ax, 1] + 5 && lineY > DrawingArray[ax, 1] - 5)
                    {
                        // отмечаем ее как захваченную (записываем ее индекс в массив captured)
                        captured = ax;
                        // останавливаем цикл, мы нашли нужную точку
                        break;
                    }
                }
            }
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            // если установлен режим создания сплайна
            if (comboBox1.SelectedIndex == 0)
            {
                // сохраняем координаты мыши
                Mcoord_X = e.X;
                Mcoord_Y = e.Y; // вычисляем параметры для будующей дорисовки линий от указателя мыши к координатным осям.
                lineX = devX * e.X;
                lineY = (float)(ScreenH - devY * e.Y);

                // текущая (интерактивная) точка, добавляемая к уже установленным - непрерывно изменяется от движения
                // мыши и создает эффект интерактивности и наглядности приложения
                DrawingArray[count_points, 0] = lineX;
                DrawingArray[count_points, 1] = lineY;
            }
            else
            {
                // обычное протоколирование координат для подсвечивания вершины в случае наведения
                // сохраняем координаты мыши
                Mcoord_X = e.X;
                Mcoord_Y = e.Y;

                // вычисляем параметры для будующей дорисовки линий от указателя мыши к координатным осям

                float _lastX = lineX;
                float _lastY = lineY;

                lineX = devX * e.X;
                lineY = (float)(ScreenH - devY * e.Y);

                // если точка захвачена (т.е. пользователь удерживает кнопку мыши)
                if (captured != -1)
                {
                    // то мы вносим разницу с последними координатами курсора
                    // другими словами перемещаем захваченную точку
                    DrawingArray[captured, 0] -= _lastX - lineX;
                    DrawingArray[captured, 1] -= _lastY - lineY;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // инициализация бибилиотеки glut
            Glut.glutInit();
            // инициализация режима экрана
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // установка цвета очистки экрана (RGBA)
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // активация проекционной матрицы
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы
            Gl.glLoadIdentity();

            // определение параметров настройки проекции, в зависимости от размеров сторон элемента AnT.
            if ((float)AnT.Width <= (float)AnT.Height)
            {
                ScreenW = 500.0;
                ScreenH = 500.0 * (float)AnT.Height / (float)AnT.Width;

                Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);
            }
            else
            {
                ScreenW = 500.0 * (float)AnT.Width / (float)AnT.Height;
                ScreenH = 500.0;

                Glu.gluOrtho2D(0.0, 500.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 500.0);
            }

            // сохранение коэфицентов, которые нам необходимы для перевода
            // координат указателя в оконной системе, в координаты,
            // принятые в нашей OpenGL сцене
            devX = (float)ScreenW / (float)AnT.Width;
            devY = (float)ScreenH / (float)AnT.Height;

            // установка объектно-видовой матрицы
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            RenderTimer.Start();

            comboBox1.SelectedIndex = 0;
        }

        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            // отмечаем, что нет захваченной точки
            captured = -1;
        }

        private void AnT_MouseClick(object sender, MouseEventArgs e)
        {
            // если мы находимся в режиме создания сплайна
            if (comboBox1.SelectedIndex == 0)
            {
                // забираем координаты мыши
                Mcoord_X = e.X;
                Mcoord_Y = e.Y;

                // приводим к нужному нам формату в соотвествии с настройками проекции
                lineX = devX * e.X;
                lineY = (float)(ScreenH - devY * e.Y);

                // создаем новую контрольную точку
                DrawingArray[count_points, 0] = lineX;
                DrawingArray[count_points, 1] = lineY;

                // и увеличиваем значение счетчика контрольных точек
                count_points++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RenderTimer.Stop();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth((float)3.1);

            //ВНутренний прямоугольник
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex2d(80, 110);
            Gl.glVertex2d(100, 110);
            Gl.glVertex2d(100, 80);
            Gl.glVertex2d(80, 80);
            Gl.glVertex2d(80, 110);
            Gl.glEnd();

            //Внешний контур
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(130, 20);
            Gl.glVertex2d(60, 20);
            Gl.glVertex2d(60, 50);
            Gl.glVertex2d(40, 50);
            Gl.glVertex2d(40, 60);
            Gl.glVertex2d(20, 60);
            Gl.glVertex2d(20, 90);
            Gl.glVertex2d(60, 90);
            Gl.glVertex2d(60, 100);
            Gl.glVertex2d(20, 100);
            Gl.glVertex2d(20, 140);
            Gl.glVertex2d(60, 140);
            Gl.glVertex2d(60, 150);
            Gl.glVertex2d(20, 150);
            Gl.glVertex2d(20, 210);
            Gl.glVertex2d(150, 210);
            Gl.glVertex2d(150, 150);
            Gl.glVertex2d(110, 150);
            Gl.glVertex2d(110, 140);
            Gl.glVertex2d(150, 140);
            Gl.glVertex2d(150, 50);
            Gl.glEnd();

            int i, j, N = 4;
            double xA, xB, xC, xD, yA, yB, yC, yD, t;
            double a0, a1, a2, a3, b0, b1, b2, b3;
            double X, Y;

            //Левый круг
            Gl.glBegin(Gl.GL_LINE_LOOP);
            float[,] CircleArray = new float[7, 2];
            float topX = 50, topY = 200,
            rightX = 70, rightY = 180,
            leftX = 30, leftY = 180,
            bottomX = 50, bottomY = 160;

            CircleArray[0, 0] = topX;
            CircleArray[0, 1] = topY;
            CircleArray[1, 0] = leftX;
            CircleArray[1, 1] = leftY;
            CircleArray[2, 0] = bottomX;
            CircleArray[2, 1] = bottomY;
            CircleArray[3, 0] = rightX;
            CircleArray[3, 1] = rightY;
            CircleArray[4, 0] = topX;
            CircleArray[4, 1] = topY;
            CircleArray[5, 0] = leftX;
            CircleArray[5, 1] = leftY;
            CircleArray[6, 0] = bottomX;
            CircleArray[6, 1] = bottomY;

            for (i = 1; i < 5; i++)
            {
                xA = CircleArray[i - 1, 0];
                xB = CircleArray[i, 0];
                xC = CircleArray[i + 1, 0];
                xD = CircleArray[i + 2, 0];

                yA = CircleArray[i - 1, 1];
                yB = CircleArray[i, 1];
                yC = CircleArray[i + 1, 1];
                yD = CircleArray[i + 2, 1];

                a3 = (-xA + 3 * (xB - xC) + xD) / 6.0;
                a2 = (xA - 2 * xB + xC) / 2.0;
                a1 = (xC - xA) / 2.0;
                a0 = (xA + 4 * xB + xC) / 6.0;
                b3 = (-yA + 3 * (yB - yC) + yD) / 6.0;
                b2 = (yA - 2 * yB + yC) / 2.0;
                b1 = (yC - yA) / 2.0;
                b0 = (yA + 4 * yB + yC) / 6.0;

                for (j = 0; j <= 5; j++)
                {
                    t = (double)j / (double)N;
                    X = (((a3 * t + a2) * t + a1) * t + a0);
                    Y = (((b3 * t + b2) * t + b1) * t + b0);
                    Gl.glVertex2d(X, Y);
                }
            }
            Gl.glEnd();

            //Правый круг
            Gl.glBegin(Gl.GL_LINE_LOOP);
            topX = 120; topY = 70;
            rightX = 140; rightY = 50;
            leftX = 100; leftY = 50;
            bottomX = 120; bottomY = 30;

            CircleArray[0, 0] = topX;
            CircleArray[0, 1] = topY;
            CircleArray[1, 0] = leftX;
            CircleArray[1, 1] = leftY;
            CircleArray[2, 0] = bottomX;
            CircleArray[2, 1] = bottomY;
            CircleArray[3, 0] = rightX;
            CircleArray[3, 1] = rightY;
            CircleArray[4, 0] = topX;
            CircleArray[4, 1] = topY;
            CircleArray[5, 0] = leftX;
            CircleArray[5, 1] = leftY;
            CircleArray[6, 0] = bottomX;
            CircleArray[6, 1] = bottomY;

            for (i = 1; i < 5; i++)
            {
                xA = CircleArray[i - 1, 0];
                xB = CircleArray[i, 0];
                xC = CircleArray[i + 1, 0];
                xD = CircleArray[i + 2, 0];

                yA = CircleArray[i - 1, 1];
                yB = CircleArray[i, 1];
                yC = CircleArray[i + 1, 1];
                yD = CircleArray[i + 2, 1];

                a3 = (-xA + 3 * (xB - xC) + xD) / 6.0;
                a2 = (xA - 2 * xB + xC) / 2.0;
                a1 = (xC - xA) / 2.0;
                a0 = (xA + 4 * xB + xC) / 6.0;
                b3 = (-yA + 3 * (yB - yC) + yD) / 6.0;
                b2 = (yA - 2 * yB + yC) / 2.0;
                b1 = (yC - yA) / 2.0;
                b0 = (yA + 4 * yB + yC) / 6.0;

                for (j = 0; j <= 5; j++)
                {
                    t = (double)j / (double)N;
                    X = (((a3 * t + a2) * t + a1) * t + a0);
                    Y = (((b3 * t + b2) * t + b1) * t + b0);
                    Gl.glVertex2d(X, Y);
                }
            }
            Gl.glEnd();

            //Закругление справа снизу
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(150, 50);
            float[,] ArcArray = new float[9, 2];
            //задание координат для скругления угла по конкретным точкам
            //в виде ломанной из 9 сегментов для построения сплайна
            ArcArray[0, 0] = 150;
            ArcArray[0, 1] = 50;
            ArcArray[1, 0] = 148;
            ArcArray[1, 1] = 46;
            ArcArray[2, 0] = 146;
            ArcArray[2, 1] = 40;
            ArcArray[3, 0] = 144;
            ArcArray[3, 1] = 34;
            ArcArray[4, 0] = 140;
            ArcArray[4, 1] = 28;
            ArcArray[5, 0] = 138;
            ArcArray[5, 1] = 26;
            ArcArray[6, 0] = 136;
            ArcArray[6, 1] = 24;
            ArcArray[7, 0] = 134;
            ArcArray[7, 1] = 22;
            ArcArray[8, 0] = 130;
            ArcArray[8, 1] = 20;

            for (i = 1; i < 7; i++)
            {
                xA = ArcArray[i - 1, 0];
                xB = ArcArray[i, 0];
                xC = ArcArray[i + 1, 0];
                xD = ArcArray[i + 2, 0];

                yA = ArcArray[i - 1, 1];
                yB = ArcArray[i, 1];
                yC = ArcArray[i + 1, 1];
                yD = ArcArray[i + 2, 1];

                a3 = (-xA + 3 * (xB - xC) + xD) / 6.0;
                a2 = (xA - 2 * xB + xC) / 2.0;
                a1 = (xC - xA) / 2.0;
                a0 = (xA + 4 * xB + xC) / 6.0;
                b3 = (-yA + 3 * (yB - yC) + yD) / 6.0;
                b2 = (yA - 2 * yB + yC) / 2.0;
                b1 = (yC - yA) / 2.0;
                b0 = (yA + 4 * yB + yC) / 6.0;

                for (j = 0; j <= 7; j++)
                {
                    t = (double)j / (double)N;
                    X = (((a3 * t + a2) * t + a1) * t + a0);
                    Y = (((b3 * t + b2) * t + b1) * t + b0);
                    Gl.glVertex2d(X, Y);
                }
            }
            Gl.glVertex2d(130, 20);
            Gl.glEnd();

            Gl.glFlush();
            AnT.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // обработка "тика" таймера - вызов функции отрисовки
            Draw();
        }

        // функция визуализации текста
        private void PrintText2D(float x, float y, string text)
        {
            // устанавливаем позицию вывода растровых символов
            // в переданных координатах x и y.
            Gl.glRasterPos2f(x, y);

            // в цикле foreach перебираем значения из массива text,
            // который содержит значение строки для визуализации
            foreach (char char_for_draw in text)
            {
                // визуализируем символ с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15.
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_8_BY_13, char_for_draw);
            }
        }

        // функция отрисовки, вызываемая событием таймера
        private void Draw()
        {
            // количество сегментов при расчете сплайна
            int N = 30; // вспомогательные переменные для расчета сплайна
            double X, Y;

            // n = count_points+1 означает что мы берем все созданные контрольные
            // точки + ту, которая следует за мышью, для создания интерактивности приложения
            int eps = 4, i, j, n = count_points + 1, first;
            double xA, xB, xC, xD, yA, yB, yC, yD, t;
            double a0, a1, a2, a3, b0, b1, b2, b3;

            // очистка буфера цвета и буфера глубины
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы
            Gl.glLoadIdentity();

            // установка черного цвета
            Gl.glColor3f(0, 0, 0);

            // помещаем состояние матрицы в стек матриц
            Gl.glPushMatrix();

            Gl.glPointSize(5.0f);
            Gl.glBegin(Gl.GL_POINTS);

            Gl.glVertex2d(0, 0);

            Gl.glEnd();
            Gl.glPointSize(1.0f);

            PrintText2D(devX * Mcoord_X + 0.2f, (float)ScreenH - devY * Mcoord_Y + 0.4f, "[ x: " + (devX * Mcoord_X).ToString() + " ; y: " + ((float)ScreenH - devY * Mcoord_Y).ToString() + "]");


            // выполняем перемещение в прострастве по осям X и Y

            // выполняем цикл по контрольным точкам
            for (i = 0; i < n; i++)
            {
                // сохраняем координаты точки (более легкое представления кода)
                X = DrawingArray[i, 0];
                Y = DrawingArray[i, 1];

                // если точка выделена (перетаскивается мышью)
                if (i == captured)
                {
                    // для ее отрисовки будут использоватся более толстые линии
                    Gl.glLineWidth(3.0f);
                }

                // начинаем отрисовку точки (квадрат)
                Gl.glBegin(Gl.GL_LINE_LOOP);

                Gl.glVertex2d(X - eps, Y - eps);
                Gl.glVertex2d(X + eps, Y - eps);
                Gl.glVertex2d(X + eps, Y + eps);
                Gl.glVertex2d(X - eps, Y + eps);

                Gl.glEnd();

                // если была захваченная точка - необходимо вернуть толщину линий
                if (i == captured)
                {
                    // возвращаем прежнее значение
                    Gl.glLineWidth(1.0f);
                }
            }


            // дополнительный цикл по всем контрольным точкам -
            // подписываем их координаты и номер
            for (i = 0; i < n; i++)
            {
                // координаты точки
                X = DrawingArray[i, 0];
                Y = DrawingArray[i, 1];
                // выводим подпись рядом с точкой
                PrintText2D((float)(X - 20), (float)(Y - 20), "P " + i.ToString() + ": " + X.ToString() + ", " + Y.ToString());
            }

            // начинает отрисовку кривой
            Gl.glBegin(Gl.GL_LINE_STRIP);

            // используем все точки -1 (т,к. алгоритм "зацепит" i+1 точку)
            for (i = 1; i < n - 1; i++)
            {
                // реализация представленного в теоретическом описании алгоритма для калькуляции сплайна
                first = 1;
                xA = DrawingArray[i - 1, 0];
                xB = DrawingArray[i, 0];
                xC = DrawingArray[i + 1, 0];
                xD = DrawingArray[i + 2, 0];

                yA = DrawingArray[i - 1, 1];
                yB = DrawingArray[i, 1];
                yC = DrawingArray[i + 1, 1];
                yD = DrawingArray[i + 2, 1];

                a3 = (-xA + 3 * (xB - xC) + xD) / 6.0;

                a2 = (xA - 2 * xB + xC) / 2.0;

                a1 = (xC - xA) / 2.0;

                a0 = (xA + 4 * xB + xC) / 6.0;

                b3 = (-yA + 3 * (yB - yC) + yD) / 6.0;

                b2 = (yA - 2 * yB + yC) / 2.0;

                b1 = (yC - yA) / 2.0;

                b0 = (yA + 4 * yB + yC) / 6.0;

                // отрисовка сегментов

                for (j = 0; j <= N; j++)
                {
                    // параметр t на отрезке от 0 до 1
                    t = (double)j / (double)N;

                    // генерация координат
                    X = (((a3 * t + a2) * t + a1) * t + a0);
                    Y = (((b3 * t + b2) * t + b1) * t + b0);

                    // и установка вершин
                    if (first == 1)
                    {
                        first = 0;
                        Gl.glVertex2d(X, Y);
                    }
                    else
                        Gl.glVertex2d(X, Y);
                }
            }
            Gl.glEnd();


            // завершаем рисование
            Gl.glFlush();

            // сигнал для обновление элемента реализующего визуализацию
            AnT.Invalidate();
        }

    }
}
