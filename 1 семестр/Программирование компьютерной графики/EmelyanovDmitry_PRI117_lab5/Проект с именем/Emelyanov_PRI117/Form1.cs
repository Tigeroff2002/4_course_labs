using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl 
using Tao.Platform.Windows;

namespace Emelyanov_PRI117
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            // очистка окна 
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода в соотвествии с размерами элемента anT 
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);


            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            // теперь необходимо корректно настроить 2D ортогональную проекцию
            // в зависимости от того, какая сторона больше 
            // мы немного варьируем то, как будут сконфигурированы настройки проекции

            if ((float)AnT.Width <= (float)AnT.Height)
            {
                Glu.gluOrtho2D(0.0, 30.0 * (float)AnT.Height / (float)AnT.Width, 0.0, 30.0);
            }
            else
            {
                Glu.gluOrtho2D(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0);
            }



            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // настройка параметров OpenGL для визуализации 
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // очищаем буфер цвета 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            // очищаем текущую матрицу 
            Gl.glLoadIdentity();
            // устанавливаем текущий цвет - голубой\ и ширину 
            Gl.glColor3f(0, 191, 255);
            Gl.glLineWidth(1f);

            // активируем режим рисования линий на основе 
            // последовательного соединения всех вершин отрезками 
            //Буква Д
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex2d(0, 5);
            Gl.glVertex2d(5, 5);
            Gl.glVertex2d(1, 5);
            Gl.glVertex2d(2, 10);
            Gl.glVertex2d(2, 10);
            Gl.glVertex2d(4, 5);
            // завершаем режим рисования 
            Gl.glEnd();
            
            //Буква И
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(6, 10);
            Gl.glVertex2d(6, 5);
            Gl.glVertex2d(6, 5);
            Gl.glVertex2d(10, 10);
            Gl.glVertex2d(10, 10);
            Gl.glVertex2d(10, 5);
            // завершаем режим рисования 
            Gl.glEnd();

            //Буква М
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(11, 5);
            Gl.glVertex2d(11, 10);
            Gl.glVertex2d(11, 10);
            Gl.glVertex2d(13, 8);
            Gl.glVertex2d(13, 8);
            Gl.glVertex2d(15, 10);
            Gl.glVertex2d(15, 10);
            Gl.glVertex2d(15, 5);
            // завершаем режим рисования 
            Gl.glEnd();

            //Буква А
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(16, 5);
            Gl.glVertex2d(18, 10);
            Gl.glVertex2d(18, 10);
            Gl.glVertex2d(20, 5);
            Gl.glVertex2d(16, 8);
            Gl.glVertex2d(20, 8);
            // завершаем режим рисования 
            Gl.glEnd();



            // дожидаемся конца визуализации кадра 
            Gl.glFlush();

            // посылаем сигнал перерисовки элемента AnT. 
            AnT.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
