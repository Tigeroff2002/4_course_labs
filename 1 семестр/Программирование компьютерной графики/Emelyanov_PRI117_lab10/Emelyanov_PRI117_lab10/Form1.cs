using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Emelyanov_PRI117_lab10 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        static private double[,] mode_ = new double[5, 2];

        // объекты, содержащие настройки для потоков
        ParamsForThread[] threadInputParams = new ParamsForThread[8];

        // Слово делегат (delegate) используется в C# для обозначения хорошо известного понятия. Делегат задает определение функционального типа (класса) данных. Экземплярами класса являются функции. Описание делегата в языке C# представляет собой описание еще одного частного случая класса. Каждый делегат описывает множество функций с заданной сигнатурой. Каждая функция (метод), сигнатура которого совпадает с сигнатурой делегата, может рассматриваться как экземпляр класса, заданного делегатом.
        delegate void RenderDLG();

        // массив пикселей
        static private byte[,,] PixelsArray = new byte[600, 600, 3];

        // объявляем объекты для управления потоками
        Thread th_1 = null;
        Thread th_2 = null;
        Thread th_3 = null;
        Thread th_4 = null;
        Thread th_5 = null;
        Thread th_6 = null;
        Thread th_7 = null;
        Thread th_8 = null;

        private void button1_Click(object sender, EventArgs e) {
            // делим строки обрабатываемого изображения между потоками
            threadInputParams[0] = new ParamsForThread(0, 75, 600);
            threadInputParams[1] = new ParamsForThread(75, 150, 600);
            threadInputParams[2] = new ParamsForThread(150, 225, 600);
            threadInputParams[3] = new ParamsForThread(225, 300, 600);
            threadInputParams[4] = new ParamsForThread(300, 375, 600);
            threadInputParams[5] = new ParamsForThread(375, 450, 600);
            threadInputParams[6] = new ParamsForThread(450, 525, 600);
            threadInputParams[7] = new ParamsForThread(525, 600, 600);


            threadInputParams[0]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[1]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[2]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[3]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[4]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[5]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[6]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);
            threadInputParams[7]._pointerToDraw = new ParamsForThread._RenderDLG(Draw);

            threadInputParams[0].code_mode = comboBox1.SelectedIndex;
            threadInputParams[1].code_mode = comboBox1.SelectedIndex;
            threadInputParams[2].code_mode = comboBox1.SelectedIndex;
            threadInputParams[3].code_mode = comboBox1.SelectedIndex;
            threadInputParams[4].code_mode = comboBox1.SelectedIndex;
            threadInputParams[5].code_mode = comboBox1.SelectedIndex;
            threadInputParams[6].code_mode = comboBox1.SelectedIndex;
            threadInputParams[7].code_mode = comboBox1.SelectedIndex;

            /*создаем 8 потоков, в качестве параметров передаем имя Выполняемой функции*/
            th_1 = new Thread(CalculateImage);
            th_2 = new Thread(CalculateImage);
            th_3 = new Thread(CalculateImage);
            th_4 = new Thread(CalculateImage);
            th_5 = new Thread(CalculateImage);
            th_6 = new Thread(CalculateImage);
            th_7 = new Thread(CalculateImage);
            th_8 = new Thread(CalculateImage);

            //расставляем приоритеты для потоков ниже среднего
            th_1.Priority = ThreadPriority.Lowest;
            th_2.Priority = ThreadPriority.Lowest;
            th_3.Priority = ThreadPriority.Lowest;
            th_4.Priority = ThreadPriority.Lowest;
            th_5.Priority = ThreadPriority.Lowest;
            th_6.Priority = ThreadPriority.Lowest;
            th_7.Priority = ThreadPriority.Lowest;
            th_8.Priority = ThreadPriority.Lowest;

            /*запускаем каждый поток, в качестве параметра передаем выводимый символ
            этот символ передастся в качестве параметра в функцию WriteString*/

            Render_Timer.Start();

            th_1.Start(threadInputParams[0]);
            th_2.Start(threadInputParams[1]);
            th_3.Start(threadInputParams[2]);
            th_4.Start(threadInputParams[3]);
            th_5.Start(threadInputParams[4]);
            th_6.Start(threadInputParams[5]);
            th_7.Start(threadInputParams[6]);
            th_8.Start(threadInputParams[7]);
        }

        private void AnT_Load(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {
            // инициализация openGL (glut)
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            // цвет очистки окна
            Gl.glClearColor(255, 255, 255, 1);

            // настройка порта просмотра
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // очищаем массив пикселей
            for (int ax = 0; ax < 600; ax++) {
                for (int bx = 0; bx < 600; bx++) {
                    PixelsArray[ax, bx, 0] = 0;
                    PixelsArray[ax, bx, 1] = 0;
                    PixelsArray[ax, bx, 2] = 0;
                }
            }

            // устанавливаем первый элемент в comboBox
            comboBox1.SelectedIndex = 0;

            // и добавляем 1 режим в массив _mode
            mode_[1, 0] = 0.32;
            mode_[1, 1] = 0.04;
        }

        // функция отрисовки
        private void Draw() {
            // устанавливаем позицию вывода
            Gl.glRasterPos2i(-1, -1);
            // визуализируем массив
            Gl.glDrawPixels(600, 600, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, PixelsArray);
            Gl.glFlush();
            AnT.Invalidate();
        }

        // функция вычисления изображения
        static void CalculateImage(object Settings) {
            // получаем параметры через объект Settings, приводя его к типу ParamsForThread
            ParamsForThread thisThreadSettings = (ParamsForThread)Settings;

            // инициализация начальных значений переменных
            double xmin = -1.5;
            double ymin = -1.2;
            double xmax = 1.5;
            double ymax = 1.5;

            int W = 600;
            int H = 600;

            double dx = (xmax - xmin) / (double)(W - 1);
            double dy = (ymax - ymin) / (double)(H - 1);

            double x, y, X, Y, Cx, Cy;

            // циклы по всем пикселям результирующего изображения
            for (int ax = thisThreadSettings._FromImageH; ax < thisThreadSettings._ToImageH; ax++) {
                for (int bx = 0; bx < thisThreadSettings._ImageW; bx++) {
                    // подготовка к выполнению итерации

                    x = xmin + ax * dx;
                    y = ymin + bx * dy;

                    Cx = x;
                    Cy = y;
                    X = x;
                    Y = y;

                    double ix = 0, iy = 0, n = 0;

                    // выполнение итерации
                    while ((ix * ix + iy * iy < 4) && (n < 64)) {
                        // если режим установлен - standart
                        if (thisThreadSettings.code_mode == 0) {
                            ix = X * X - Y * Y + Math.Pow(Cx, 2);
                            iy = 2 * X * Y + 2 * Cx * Cy - Math.Pow(Cy, 2);
                        } else {
                            // иначе используем значения из массива _mode используя индекс comboBox1,
                            // переданный в поток через thisThreadSettings
                            ix = X * X - Y * Y + mode_[thisThreadSettings.code_mode, 0];
                            iy = 2 * X * Y + mode_[thisThreadSettings.code_mode, 1];
                        }

                        n++;
                        X = ix;
                        Y = iy;
                    }

                    // заносим значение цвета в массив, описывающий результирующее изображение
                    PixelsArray[bx, ax, 0] = (byte)(255 - n * 4);
                    PixelsArray[bx, ax, 1] = (byte)(255 - n * 4);
                    PixelsArray[bx, ax, 2] = (byte)(255 - n * 4);

                    // вызываем функцию отрисовки
                    thisThreadSettings._pointerToDraw();
                }
            }
        }

        private void Render_Timer_Tick(object sender, EventArgs e) {
            Gl.glRasterPos2i(-1, -1);
            Gl.glDrawPixels(600, 600, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, PixelsArray);
            Gl.glFlush();

            AnT.Invalidate();
        }
    }

    // данный класс - это набор параметров, которые мы будем передавать в поток
    public class ParamsForThread {
        public ParamsForThread(int startH, int endH, int Width) {
            _FromImageH = startH;
            _ToImageH = endH;
            _ImageW = Width;
        }

        // элемент, установленный в comboBox1
        public int code_mode;

        public delegate void _RenderDLG();
        // указатель на функцию отрисовки
        public _RenderDLG _pointerToDraw = null;

        // параметры части изображения, которое будет расчитыватся в данном потоке
        public int _FromImageH;
        public int _ToImageH;
        public int _ImageW;
    }
}
