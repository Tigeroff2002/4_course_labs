using Emelyanov_PRI117_lab14;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Emelyanov_PRI117_lab15
{
    public partial class Form1 : Form
    {
        // отсчет времени
        float global_time = 0; // массив с параметрами установки камеры
        private float[,] camera_date = new float[5, 7];

        // экземпляра класса Explosion
        private Explosion BOOOOM_1 = new Explosion(1, 10, 1, 300, 500);

        // вспомогательные переменные - в них будут хранится обработанные значения,
        // полученные при перетаскивании ползунков пользователем
        double a = 0, b = 0, c = -5, d = 90, zoom = 1; // выбранные оси
        int os_x = 1, os_y = 0, os_z = 0;

        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();

            Model = new anModelLoader();
            Model.LoadModel(Model.FName);
        }

        private void AnT_Load(object sender, EventArgs e)
        {

        }

        class Explosion
        {
            // позиция взрыва
            private float[] position = new float[3];
            // мощность
            private float _power;
            // максимальное количество частиц
            private int MAX_PARTICLES = 1000;
            // текущее установленное количество частиц
            private int _particles_now;

            // активирован
            private bool isStart = false;

            // массив частиц на основе созданного ранее класса
            private Partilce[] PartilceArray;

            // дисплейный список для рисования частицы создан
            private bool isDisplayList = false;
            // номер дисплейного списка для отрисовки
            private int DisplayListNom = 0;

            // конструктор класса; в него передаются координаты, где должен произойти взрыв, мощность и количество чатиц
            public Explosion(float x, float y, float z, float power, int particle_count)
            {
                position[0] = x;
                position[1] = y;
                position[2] = z;

                _particles_now = particle_count;
                _power = power;

                // если число частиц превышает максимально разрешенное
                if (particle_count > MAX_PARTICLES)
                {
                    particle_count = MAX_PARTICLES;
                }

                // создаем массив частиц необходимого размера
                PartilceArray = new Partilce[particle_count];
            }

            // функция обновления позиции взрыва
            public void SetNewPosition(float x, float y, float z)
            {
                position[0] = x;
                position[1] = y;
                position[2] = z;
            }

            // установка нового значения мощности взрыва
            public void SetNewPower(float new_power)
            {
                _power = new_power;
            }

            // создания дисплейного списка для отрисовки частицы (т.к. отрисовывать даже небольшой полигон такое количество раз очень накладно)
            private void CreateDisplayList()
            {
                // генерация дисплейного списка
                DisplayListNom = Gl.glGenLists(1);

                // начало создания списка
                Gl.glNewList(DisplayListNom, Gl.GL_COMPILE);

                // режим отрисовки треугольника
                Gl.glBegin(Gl.GL_TRIANGLES);

                // задаем форму частицы
                Gl.glVertex3d(0, 0, 0);
                Gl.glVertex3d(0.02f, 0.02f, 0);
                Gl.glVertex3d(0.02f, 0, -0.02f);

                Gl.glEnd();

                // завершаем отрисовку частицы
                Gl.glEndList();

                // флаг - дисплейный список создан
                isDisplayList = true;
            }

            // функция, реализующая взрыв
            public void Boooom(float time_start)
            {
                // инициализируем экземпляр класса Random
                Random rnd = new Random();

                // если дисплейный список не создан, надо его создать
                if (!isDisplayList)
                {
                    CreateDisplayList();
                }

                // по всем частицам
                for (int ax = 0; ax < _particles_now; ax++)
                {
                    // создаем частицу
                    PartilceArray[ax] = new Partilce(position[0], position[1], position[2], 5.0f, 10, time_start);

                    // случайным образом генериуем ориентацию вектора ускорения для данной частицы
                    int direction_x = rnd.Next(1, 3);
                    int direction_y = -5;
                    int direction_z = rnd.Next(1, 5);
                    

                    // если сгенерированно число 2 - то мы заменяем его на -1.
                    if (direction_x == 2)
                        direction_x = -1;


                    if (direction_y == 2)
                        direction_y = -1;

                    if (direction_z == 2)
                        direction_z = -1;

                    // задаем мощность в промежутке от 5 до 100% от указанной (чтобы частицы имели разное ускорение)
                    float _power_rnd = rnd.Next((int)_power / 20, (int)_power);
                    // устанавливаем затухание, равное 50% от мощности
                    PartilceArray[ax].setAttenuation(_power / 2.0f);
                    // устанавливаем ускорение частицы, еще раз генерируя случайное число
                    // таким образом мощность определится от 10 - до 100% полученной
                    // Здесь же применяем ориентацию для векторов ускорения
                    PartilceArray[ax].SetPower(_power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_x, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_y, _power_rnd * ((float)rnd.Next(100, 1000) / 1000.0f) * direction_z);
                }

                // взрыв активирован
                isStart = true;
            }

            // калькуляция текущего взрыва
            public void Calculate(float time)
            {
                // только в том случае, если взрыв уже активирован
                if (isStart)
                {
                    // проходим циклом по всем частицам
                    for (int ax = 0; ax < _particles_now; ax++)
                    {
                        // если время жизни частицы еще не вышло
                        if (PartilceArray[ax].isLife())
                        {
                            // обновляем позицию частицы
                            PartilceArray[ax].UpdatePosition(time);

                            // сохраняем текущую матрицу
                            Gl.glPushMatrix();
                            // получаем размер частицы
                            float size = PartilceArray[ax].GetSize();

                            // выполняем перемещение частицы в необходимую позицию
                            Gl.glTranslated(PartilceArray[ax].GetPositionX(), PartilceArray[ax].GetPositionY(), PartilceArray[ax].GetPositionZ());
                            // масштабируем ее в соотвествии с ее размером
                            Gl.glScalef(size, size, size);
                            // вызываем дисплейный список для отрисовки частицы из кеша видеоадаптера
                            Gl.glCallList(DisplayListNom);
                            // возвращаем матрицу
                            Gl.glPopMatrix();

                            // отражение от "земли"
                            // если координата Y стала меньше нуля (удар о землю)
                            if (PartilceArray[ax].GetPositionY() < 0)
                            {
                                // инвертируем проекцию скорости на ось Y, как будто частица ударилась и отскочила от земли
                                // причем скорость затухает на 40%
                                PartilceArray[ax].InvertSpeed(1, 0.6f);
                            }
                        }

                    }
                }
            }
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            // отсчитываем время
            global_time += (float)RenderTimer.Interval / 1000;
            // вызываем функцию отрисовки
            Draw();
        }

        anModelLoader Model = null;

        // функция отрисовки сцены
        private void Draw()
        {
            // очистка буфера цвета и буфера глубины
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы
            Gl.glLoadIdentity();

            // помещаем состояние матрицы в стек матриц, дальнейшие трансформации затронут только визуализацию объекта
            Gl.glPushMatrix();
            // производим перемещение в зависимости от значений, полученных при перемещении ползунков
            Gl.glTranslated(a, b, c);
            // поворот по установленной оси
            Gl.glRotated(d, os_x, os_y, os_z);
            // и масштабирование объекта
            Gl.glScaled(zoom, zoom, zoom);
            // в зависимсоти от установленного режима отрисовываем сцену в черном или белом цвете
            if (comboBox2.SelectedIndex == 0)
            {
                // цвет очистки окна
                Gl.glClearColor(255, 255, 255, 1);
            }
            else
            {
                Gl.glClearColor(0, 0, 0, 1);
            }

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();

            // в зависимсоти от установленного режима отрисовываем сцену в черном или белом цвете
            if (comboBox2.SelectedIndex == 0)
            {
                // цвет рисования
                Gl.glColor3d(0, 0, 0);
            }
            else
            {
                Gl.glColor3d(255, 255, 255);
            }

            Gl.glPushMatrix();

            // определяем установленную камеру
            int camera = comboBox1.SelectedIndex;

            // используем параметры для установленной камеры
            Gl.glTranslated(camera_date[camera, 0], camera_date[camera, 1], camera_date[camera, 2]);
            Gl.glRotated(camera_date[camera, 3], camera_date[camera, 4], camera_date[camera, 5], camera_date[camera, 6]);

            Gl.glPushMatrix();

            // отрисовываем сеточную плоскость, которая нам будет напоминать где находится земля
            DrawMatrix(8);

            // выполняем просчет взрыва
            BOOOOM_1.Calculate(global_time);


            if (Model != null)
                Model.DrawModel();

            Gl.glPopMatrix();

            Gl.glPopMatrix();
            Gl.glFlush();

            // обновляем окно
            AnT.Invalidate();
        }

        // функция для отрисовки матрицы
        private void DrawMatrix(int x)
        {
            float quad_size = 1;

            // две последовательные линии после пересечения создадут "матрицу", чтобы мы могли понимать где находится земля
            Gl.glBegin(Gl.GL_LINES);

            for (int ax = 0; ax < x + 1; ax++)
            {
                Gl.glVertex3d(quad_size * ax, 0, 0);
                Gl.glVertex3d(quad_size * ax, 0, quad_size * x);
            }

            for (int bx = 0; bx < x + 1; bx++)
            {
                Gl.glVertex3d(0, 0, quad_size * bx);
                Gl.glVertex3d(quad_size * x, 0, quad_size * bx);
            }

            Gl.glEnd();
        }

        // генератор случайных чисел
        Random rnd = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            // инициализация OpenGL
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            // установка начальных значений элементов comboBox
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;

            // позиция камеры 1:

            camera_date[0, 0] = -3;
            camera_date[0, 1] = 0;
            camera_date[0, 2] = -20;
            camera_date[0, 3] = 0;
            camera_date[0, 4] = 1;
            camera_date[0, 5] = 0;
            camera_date[0, 6] = 0;

            // позиция камеры 2:

            camera_date[1, 0] = -3;
            camera_date[1, 1] = 2;
            camera_date[1, 2] = -20;
            camera_date[1, 3] = 30;
            camera_date[1, 4] = 1;
            camera_date[1, 5] = 0;
            camera_date[1, 6] = 0;

            // позиция камеры 3:

            camera_date[2, 0] = -3;
            camera_date[2, 1] = 2;
            camera_date[2, 2] = -20;
            camera_date[2, 3] = 30;
            camera_date[2, 4] = 1;
            camera_date[2, 5] = 1;
            camera_date[2, 6] = 0;

            // позиция камеры 4:

            camera_date[3, 0] = -10;
            camera_date[3, 1] = 2;
            camera_date[3, 2] = -20;
            camera_date[3, 3] = 30;
            camera_date[3, 4] = 1;
            camera_date[3, 5] = 1;
            camera_date[3, 6] = 0;

            // позиция камеры 5:

            camera_date[4, 0] = -10;
            camera_date[4, 1] = 2;
            camera_date[4, 2] = -30;
            camera_date[4, 3] = 30;
            camera_date[4, 4] = 10;
            camera_date[4, 5] = 1;
            camera_date[4, 6] = 0;

            // активация таймера
            RenderTimer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            // устанавливаем новые координаты взрыва
            BOOOOM_1.SetNewPosition(rnd.Next(1, 5), rnd.Next(1, 5), 7);
            // случайную силу
            BOOOOM_1.SetNewPower(rnd.Next(20, 80));
            // и активируем сам взрыв
            BOOOOM_1.Boooom(global_time);
        }
    }
}
