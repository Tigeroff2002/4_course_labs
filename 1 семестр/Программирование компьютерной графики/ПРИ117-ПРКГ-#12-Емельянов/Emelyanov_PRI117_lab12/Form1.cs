using System;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Emelyanov_PRI117_lab12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }
        // вспомогательные переменные - в них будут хранится обработанные значения,
        // полученные при перетаскивании ползунков пользователем
        double a = 0, b = 0, c = -5, d = 0, zoom = 1; // выбранные оси
        int os_x = 1, os_y = 0, os_z = 0;
        int Q;
        Random rand = new Random();
        // режим сеточной визуализации
        bool Wire = false;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // переводим значение, установившееся в элементе trackBar, в необходимый нам формат
            a = (double)trackBar1.Value / 1000.0;
            // подписываем это значение в label элементе под данным ползунком
            label8.Text = a.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            // переводим значение, установившееся в элементе trackBar, в необходимый нам формат
            b = (double)trackBar2.Value / 1000.0;
            // подписываем это значение в label элементе под данным ползунком
            label9.Text = b.ToString();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            // переводим значение, установившееся в элементе trackBar, в необходимый нам формат
            c = (double)trackBar3.Value / 1000.0;
            // подписываем это значение в label элементе под данным ползунком
            label10.Text = c.ToString();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            // переводим значение, установившееся в элементе trackBar, в необходимый нам формат
            d = (double)trackBar4.Value;
            // подписываем это значение в label элементе под данным ползунком
            label12.Text = d.ToString();

        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            // переводим значение, установившееся в элементе trackBar, в необходимый нам формат
            zoom = (double)trackBar5.Value / 1000.0;
            // подписываем это значение в label элементе под данным ползунком
            label11.Text = zoom.ToString();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // если отмечен
            if (checkBox1.Checked)
            {
                // устанавливаем сеточный режим визуализации
                Wire = true;
            }
            else
            {
                // иначе - полигональная визуализация
                Wire = false;
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // в зависимости от выбранного режима
            switch (comboBox1.SelectedIndex)
            {
                // устанавливаем необходимую ось (будет испльзовано в функции glRotate**)
                case 0:
                    {
                        os_x = 1;
                        os_y = 0;
                        os_z = 0;
                        break;
                    }
                case 1:
                    {
                        os_x = 0;
                        os_y = 1;
                        os_z = 0;
                        break;
                    }
                case 2:
                    {
                        os_x = 0;
                        os_y = 0;
                        os_z = 1;
                        break;
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

            // установка перспективы
            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // начальная настройка параметров openGL (тест глубины, освещение и первый источник света)
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            // установка первых элементов в списках combobox
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            // активация таймера, вызывающего функцию для визуализации
            RenderTimer.Start();
            Q = rand.Next(-3, 3);
        }


        private void AnT_Load(object sender, EventArgs e)
        {

        }


        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            // очистка буфера цвета и буфера глубины
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы
            Gl.glLoadIdentity();

            // помещаем состояние матрицы в стек матриц, дальнейшие трансформации затронут только визуализацию объекта
            Gl.glPushMatrix();

            Random n_rand = new Random(Q);
            for (int i = 0; i <= 6; i++)
            {
                Gl.glTranslated(n_rand.Next(-2, 2), n_rand.Next(-2, 2), n_rand.Next(-15, 10));
                // производим перемещение в зависимости от значений, полученных при перемещении ползунков
                Gl.glTranslated(a, b, c);
                // поворот по установленной оси
                Gl.glRotated(d, os_x, os_y, os_z);
                // и масштабирование объекта
                Gl.glScaled(zoom, zoom, zoom);

                // в зависимсоти от установленного типа объекта
                switch (comboBox2.SelectedIndex)
                {
                    // рисуем нужный объект, используя фунции бибилиотеки GLUT
                    case 0:
                        if (Wire) // если установлен сеточный режим визуализации
                            Glut.glutWireSphere(2, 16, 16); // сфера
                        else
                            Glut.glutSolidSphere(2, 16, 16);
                        break;
                    case 1:
                        if (Wire) // если установлен сеточный режим визуализации
                            Glut.glutWireCylinder(1, 2, 32, 32); // цилиндр
                        else
                            Glut.glutSolidCylinder(1, 2, 32, 32);
                        break;
                    case 2:
                        if (Wire) // если установлен сеточный режим визуализации
                            Glut.glutWireCube(2); // куб
                        else
                            Glut.glutSolidCube(2);
                        break;
                    case 3:
                        if (Wire) // если установлен сеточный режим визуализации
                            Glut.glutWireCone(2, 3, 32, 32); // конус
                        else
                            Glut.glutSolidCone(2, 3, 32, 32);
                        break;
                    case 4:
                        if (Wire) // если установлен сеточный режим визуализации
                            Glut.glutWireTorus(0.2, 2.2, 32, 32); // тор
                        else
                            Glut.glutSolidTorus(0.2, 2.2, 32, 32);
                        break;
                    default:
                        break;
                }

                // возвращаем состояние матрицы
                Gl.glPopMatrix();

                // завершаем рисование
                Gl.glFlush();

                // обновлем элемент AnT
                AnT.Invalidate();
            }
        }
    }
}
