using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using DeltaDrawing.Model;

namespace DeltaDrawing.TileHelper
{
    public class Concatenater
    {

        public string ConcatenateTiles(int height, int width, int tileHeight, int tileWidth, string tileDirectoryFullPath, string key, string extension, string name, double scale, CropTransform cropAfterRotating, RotateTransform rotate, string outputDirectory)
        {
            if (String.IsNullOrEmpty(tileDirectoryFullPath) || String.IsNullOrEmpty(key))
            {
                return String.Empty;
            }
            else
            {
                const int WIDTH_POSITION = 1;
                const int HEIGHT_POSITION = 0;
                int posW = 0;
                int posH = 0;

                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(b);

                DirectoryInfo di = new DirectoryInfo(tileDirectoryFullPath);

                foreach (FileInfo fi in di.GetFiles())
                {
                    string[] positions = fi.Name.Split('.');
                    if (positions.Length > WIDTH_POSITION)
                    {
                        if (int.TryParse(positions[WIDTH_POSITION], out posW) &&
                            int.TryParse(positions[HEIGHT_POSITION], out posH) &&
                            fi.Name.Contains(key) &&
                            fi.Name.EndsWith(extension))
                        {
                            Image i = Image.FromFile(fi.FullName);
                            g.DrawImage(i, new Rectangle(posW * tileWidth, posH * tileHeight, i.Width, i.Height));
                            i = null;
                        }
                    }
                }

                

                string fileName = String.Format("{0}\\{1}.{2}.nonrotated.{3}", outputDirectory, name, key, extension);
                b.Save(fileName);
                g.Dispose();
                runGC();

                if (rotate != null)
                {

                    if (rotate.Angle != 0 && rotate.Apply)
                    {
                        // Rotation code

                        Bitmap b1 = new Bitmap(width, height);
                        Graphics g1 = Graphics.FromImage(b1);

                        Image rotatedImage = Image.FromFile(fileName);
                        float rotateAngle = (float)rotate.Angle;
                        // Adjust the rotate angle to match GDI+ goofy idea of angle
                        if (rotateAngle > 0 && rotateAngle < 360)
                            rotateAngle -= 180;

                        g1.RotateTransform(rotateAngle);
                        g1.TranslateTransform(width / 2, height / 2, System.Drawing.Drawing2D.MatrixOrder.Append);
                        // TODO: implement cropping here
                        float floatScale = (float)scale;
                        g1.DrawImage(rotatedImage, width / 2, height / 2, -width, -height);

                        fileName = String.Format("{0}\\{1}.{2}.rotated.{3}", outputDirectory, name, key, extension);

                        b1.Save(fileName);
                        g1.Dispose();
                        runGC();

                        if (cropAfterRotating != null)
                        {

                            if (cropAfterRotating.Width > 0 && cropAfterRotating.Height > 0 && cropAfterRotating.Apply)
                            {
                                float cropWidthVector = (float)cropAfterRotating.Width * floatScale;
                                float cropHeightVector = (float)cropAfterRotating.Height * floatScale;
                                Bitmap b2 = (Bitmap)Image.FromFile(fileName); 
                                Bitmap cropped = new Bitmap((int)cropWidthVector, (int)cropHeightVector);
                                float cropX = (float)cropAfterRotating.X * floatScale;
                                float cropY = (float)cropAfterRotating.Y * floatScale;

                                Graphics g2 = Graphics.FromImage(cropped);

                                g2.DrawImage(b2, new Rectangle(0, 0, cropped.Width, cropped.Height), cropX, cropY, cropped.Width, cropped.Height, GraphicsUnit.Pixel);

                                g2.Dispose();
                                runGC();

                                fileName = String.Format("{0}\\{1}.{2}.cropped.{3}", outputDirectory, name, key, extension);
                                cropped.Save(fileName);
                            }
                        }
                    }
                }

                return fileName;
            }
        }

        public void deleteTiles(string tileDirectoryFullPath, string key, string extension)
        {
            DirectoryInfo di = new DirectoryInfo(tileDirectoryFullPath);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Contains(key) && fi.Name.EndsWith(extension))
                {
                    File.Delete(fi.FullName);
                }
            }
        }

        private void runGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

    }
}
