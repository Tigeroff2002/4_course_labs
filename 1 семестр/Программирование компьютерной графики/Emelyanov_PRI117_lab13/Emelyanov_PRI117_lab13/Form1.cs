using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Emelyanov_PRI117_lab13 {
    public partial class Form1 : Form {
        private bool textureIsLoad;
        private int rot;
        private uint mGlTextureObject;
        private int imageId;

        public Form1() {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // инициализация бибилиотеки glut
            Glut.glutInit();
            // инициализация режима экрана
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // инициализация библиотеки OpenIL
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);

            // установка цвета очистки экрана (RGBA)
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // активация проекционной матрицы
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы
            Gl.glLoadIdentity();

            // установка перспективы
            Glu.gluPerspective(30, AnT.Width / AnT.Height, 1, 100);

            // установка объектно-видовой матрицы
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // начальные настройки OpenGL
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            // активация таймера
            RenderTimer.Start();
        }

        private void AnT_Load(object sender, EventArgs e) {

        }

        private void RenderTimer_Tick(object sender, EventArgs e) {
            // вызов функции отрисовки сцены
            Draw();

        } // функция отрисовки
        private void Draw() {

            // если текстура загружена
            if (textureIsLoad) {

                // увеличиваем угол поворота
                rot++;
                // корректируем угол
                if (rot > 360)
                    rot = 0;

                // очистка буфера цвета и буфера глубины
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glClearColor(255, 255, 255, 1);
                // очищение текущей матрицы
                Gl.glLoadIdentity();

                // включаем режим текстурирования
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования, указывая идентификатор mGlTextureObject
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject);

                // сохраняем состояние матрицы
                Gl.glPushMatrix();

                // выполняем перемещение для более наглядного представления сцены
                Gl.glTranslated(0, -1, -5);
                // реализуем поворот объекта
                Gl.glRotated(rot, 0, 1, 0);

                Gl.glClear(Gl.GL_4D_COLOR_TEXTURE);
                // отрисовываем полигон
                Gl.glBegin(Gl.GL_QUADS);

                // указываем поочередно вершины и текстурные координаты
                Gl.glVertex3d(2, 1, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(1, 0, 0);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(0, 1, 0);
                Gl.glTexCoord2f(0, 1);

                // завершаем отрисовку
                Gl.glEnd();

                // возвращаем матрицу
                Gl.glPopMatrix();
                // отключаем режим текстурирования
                Gl.glDisable(Gl.GL_TEXTURE_2D);

                // обновляем элемент со сценой
                AnT.Invalidate();

            }
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e) {
            // открываем окно выбора файла
            DialogResult res = openFileDialog1.ShowDialog(); // если файл выбран и возвращен результат - OK
            if (res == DialogResult.OK) {

                // создаем изображение с идентификатором imageId
                Il.ilGenImages(1, out imageId);
                // делаем изображение текущим
                Il.ilBindImage(imageId);

                // адрес изображения, полученный с помощью окна выбра файла
                string url = openFileDialog1.FileName;

                // пробуем загрузить изображение
                if (Il.ilLoadImage(url)) {

                    // если загрузка прошла успешно
                    // сохраняем размеры изображения
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата
                    {

                        // создаем текстуру, используя режим GL_RGB или GL_RGBA
                        case 24:
                            mGlTextureObject = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                            break;
                        case 32:
                            mGlTextureObject = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                            break;

                    }

                    // активируем флаг, сигнализирующий загрузку текстуры
                    textureIsLoad = true;
                    // очищаем память
                    Il.ilDeleteImages(1, ref imageId);

                }

            }

        }

        // создание текстуры в памяти openGL
        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h) {

            // идентификатор текстурного объекта
            uint texObject;

            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // создаем RGB или RGBA текстуру
            switch (Format) {

                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

            }

            // возвращаем идентификатор текстурного объекта

            return texObject;
        }
    }
}
