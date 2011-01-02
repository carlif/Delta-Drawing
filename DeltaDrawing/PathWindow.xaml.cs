using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DeltaDrawing.Model;
using System.IO;
using System.ComponentModel;

namespace DeltaDrawing.UI
{
    /// <summary>
    /// Interaction logic for PathWindow.xaml
    /// </summary>
    public partial class PathWindow : Window
    {
        public PathWindow()
        {
            InitializeComponent();
        }

        RenderTargetBitmap _rtb;
        DeltaContainer _container;
        WorkingDialog _workingDialog;

        private void drawImage()
        {
            int width;
            int height;
            //double scale = .1;
            try
            {
                ImageSource imageSource = buildImageSource(out width, out height, _container.Scale);
                /*
                    ContextSwitchDeadlock was detected
                    Message: The CLR has been unable to transition from COM context 0x11ceb00 to COM context 0x11cec70 for 60 seconds. The thread that owns the destination context/apartment is most likely either doing a non pumping wait or processing a very long running operation without pumping Windows messages. This situation generally has a negative performance impact and may even lead to the application becoming non responsive or memory usage accumulating continually over time. To avoid this problem, all single threaded apartment (STA) threads should use pumping wait primitives (such as CoWaitForMultipleHandles) and routinely pump messages during long running operations.
                 */
                runGC();
                image.Width = width;
                image.Height = height;
                image.Source = imageSource;
            }
            catch (Exception ex)
            {
                handleException(ex);
            }

        }

        private void doWork(object sender, DoWorkEventArgs e)
        {
            //Do the long running process
            drawImage();
        }

        private void workerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Hide your wait dialog
            //_workingDialog.Hide();
        }

        private void startWork()
        {
            //Show your wait dialog
            //_workingDialog = new WorkingDialog();
            //_workingDialog.Activate();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += doWork;
            worker.RunWorkerCompleted += workerCompleted;
            worker.RunWorkerAsync();
        }


        private ImageSource buildImageSource(out int width, out int height, double scale)
        {
            int iterations = 0;

            /*
             If I wanted to rotate points, how would I do that?
             * I think I need to know where the points are in relation to the edges of the picture
             * If I knew where point x,y was in relation to the edges, if I was told to rotate n degrees
             */

            /*
             * Completed 0.1.0:
             * -Load functionality
             * Crop functionality
             * Fuzzy functionality
             * -Add scale to Container
             * -Add iteration range to Path
             * -Confirm images can be made large enough
             * -  if not, implement GDI+ tile assembly
             * -Remove example code
             * 
             * To complete for 0.1.1:
             * -Inherit from value so that we don't need min and max in serialized objects
             * Resolve (Not Responding) issue with threading
             * Fix xml text box display (or build a real UI)
             * -Make namespaces consistent
             * About menu with dialog
             * Break out drawing functionality from UI layer
             * Understand when to keep image in memory versus saving to disk everytime
             * Do we still need tiling?
             * Implement mod drawing
             * Write documentation
             */
            populateXmlText();

            if (_container.DeltaPaths.Count > 0)
            {
                // Find the max iteration, apply offsets
                foreach (DeltaPath dPath in _container.DeltaPaths)
                {
                    dPath.ApplyOffset();

                    if (dPath.EndIteration > iterations)
                        iterations = dPath.EndIteration;
                }
            }
            
            if (_container.DeltaFigures.Count > 0)
            {
                foreach (DeltaFigure deltaFigure in _container.DeltaFigures)
                {
                    deltaFigure.ApplyOffset();

                    if (deltaFigure.EndIteration > iterations)
                        iterations = deltaFigure.EndIteration;
                }
            }

            DrawingVisual drawingVisual = new DrawingVisual();
            SolidColorBrush brush;
            //LinearGradientBrush gradientBrush;
            //RadialGradientBrush radialBrush;
            Pen pen;

            using (DrawingContext ctx = drawingVisual.RenderOpen())
            {
                if (_container.BlurRadius > 0)
                {
                    System.Windows.Media.Effects.BlurBitmapEffect blur = new System.Windows.Media.Effects.BlurBitmapEffect();
                    blur.Radius = (_container.BlurRadius * scale);
                    ctx.PushEffect(blur, null);
                }
                
                // Where the magic happens...
                // We draw the paths in this loop and apply the deltas
                for (int i = 0; i < iterations; i++)
                {
                    // For legacy containers, draw delta paths
                    foreach (DeltaPath dPath in _container.DeltaPaths)
                    {
                        if (i >= dPath.StartIteration && i <= dPath.EndIteration)
                        {
                            if (dPath.SkipIterationsList.Contains<int>(i))
                            {
                                // If we are to skip this iteration, just apply deltas and do not draw
                                dPath.ApplyDelta();
                            }
                            else
                            {
                                // Otherwise proceed to draw the path
                                PathGeometry pathGeometry = buildPath();
                                setGeometryPoints(pathGeometry, dPath);
                                brush = new SolidColorBrush(dPath.DeltaColor.Color);

                                pen = new Pen(brush, dPath.Thickness.Value);

                                ctx.DrawGeometry(null, pen, pathGeometry);

                                dPath.ApplyDelta();

                                pathGeometry.Freeze();

                                runGC();
                            }
                        }
                    }

                    // For newer containers, draw delta figures
                    foreach (DeltaFigure deltaFigure in _container.DeltaFigures)
                    {
                        if (i >= deltaFigure.StartIteration && i <= deltaFigure.EndIteration)
                        {
                            if (deltaFigure.SkipIterationsList.Contains<int>(i))
                            {
                                // If we are to skip this iteration, just apply deltas and do not draw
                                deltaFigure.ApplyDelta();
                            }
                            else
                            {
                                // Otherwise proceed to draw the path
                                PathGeometry pathGeometry = buildFigure(deltaFigure);
                                brush = new SolidColorBrush(deltaFigure.DeltaColor.Color);
                                //gradientBrush = new LinearGradientBrush(Color.FromArgb((byte)255, (byte)255, (byte)255, (byte)255), dPath.DeltaColor.Color, 130);
                                //radialBrush = new RadialGradientBrush(Color.FromArgb((byte)255, (byte)255, (byte)255, (byte)255), dPath.DeltaColor.Color);

                                pen = new Pen(brush, deltaFigure.Thickness.Value);
                                //pen = new Pen(gradientBrush, dPath.Thickness.Value);
                                //pen = new Pen(radialBrush, dPath.Thickness.Value);

                                ctx.DrawGeometry(null, pen, pathGeometry);

                                deltaFigure.ApplyDelta();

                                pathGeometry.Freeze();

                                runGC();
                            }
                        }
                    }
                }
            }

            if (_container.Transforms.CropBeforeRendering != null)
            {
                if (_container.Transforms.CropBeforeRendering.Apply)
                {
                    width = (int)(_container.Transforms.CropBeforeRendering.Width * scale);
                    height = (int)(_container.Transforms.CropBeforeRendering.Height * scale);
                }
                else
                {
                    width = (int)(drawingVisual.ContentBounds.Width * scale);
                    height = (int)(drawingVisual.ContentBounds.Height * scale);
                }
            }
            else
            {
                width = (int)(drawingVisual.ContentBounds.Width * scale);
                height = (int)(drawingVisual.ContentBounds.Height * scale);
            }

            TransformGroup transGroup = new TransformGroup();

            //RotateTransform rotateTrans = new RotateTransform(_container.RotateDegrees, width / 2, height / 2);
            //RotateTransform rotateTrans1 = new RotateTransform(30, width / 2, height / 2);
            //drawingVisual.Transform = rotateTrans;
            //transGroup.Children.Add(rotateTrans);
            //transGroup.Children.Add(rotateTrans1);

            ScaleTransform scaleTrans = new ScaleTransform((double)scale, (double)scale);
            //drawingVisual.Transform = scaleTrans;
            transGroup.Children.Add(scaleTrans);
            drawingVisual.Transform = transGroup;

            
            _rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            RenderOptions.SetBitmapScalingMode(_rtb, BitmapScalingMode.HighQuality);

            bool useTiles = true; // TODO: build UI around this
            if (useTiles)
            {
                runGC();
                return tileImage(drawingVisual, scale, out width, out height, false, _container.Name, _container.Transforms.CropBeforeRotating, _container.Transforms.CropAfterRotating, _container.Transforms.Rotate);
            }
            else
            {
                // WON'T DRAW without this
                _rtb.Render(drawingVisual);
                runGC();
                return (ImageSource)_rtb;
            }
        }

        private void runGC(Object objToNull)
        {
            objToNull = null;
        }

        private void runGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private PathGeometry buildPath()
        {
            // Legacy paths are hard coded to 4 points
            const int FIGURE_AMOUNT = 4;

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = new PathFigureCollection();
            for (int i = 0; i <= FIGURE_AMOUNT; i++)
            {
                pathGeometry.Figures.Add(new PathFigure());
            }
            pathGeometry.Figures[0].Segments = new PathSegmentCollection();
            pathGeometry.Figures[0].Segments.Add(new BezierSegment());

            return pathGeometry;
        }

        private void setGeometryPoints(PathGeometry pathGeometry, DeltaPath dp)
        {
            pathGeometry.Figures[0].StartPoint = new Point(dp.Point0.X, dp.Point0.Y);
            PathFigureCollection k = (PathFigureCollection)pathGeometry.Figures;
            BezierSegment l = (BezierSegment)k[0].Segments[0];

            l.Point1 = new Point(dp.Point1.X, dp.Point1.Y);
            l.Point2 = new Point(dp.Point2.X, dp.Point2.Y);
            l.Point3 = new Point(dp.Point3.X, dp.Point3.Y);
        }

        private PathGeometry buildFigure(DeltaFigure df)
        {
            const int POINT1_INDEX = 0;
            const int POINT2_INDEX = 1;
            const int POINT3_INDEX = 2;

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = new PathFigureCollection();
            pathGeometry.Figures.Add(new PathFigure());
            pathGeometry.Figures[0].Segments = new PathSegmentCollection();
            pathGeometry.Figures[0].StartPoint = new Point(df.StartPoint.X, df.StartPoint.Y);
            // For each delta segment in the delta figure, and a segment to the path geometry figure
            // This allows us to create a line with n points
            foreach (DeltaSegment deltaSegment in df.DeltaSegments)
            {
                BezierSegment s = new BezierSegment();
                pathGeometry.Figures[0].Segments.Add(s);
                s.IsSmoothJoin = true;
                if (deltaSegment.DeltaPoints.Count > POINT3_INDEX)
                {
                    s.Point1 = new Point(deltaSegment.DeltaPoints[POINT1_INDEX].X, deltaSegment.DeltaPoints[POINT1_INDEX].Y);
                    s.Point2 = new Point(deltaSegment.DeltaPoints[POINT2_INDEX].X, deltaSegment.DeltaPoints[POINT2_INDEX].Y);
                    s.Point3 = new Point(deltaSegment.DeltaPoints[POINT3_INDEX].X, deltaSegment.DeltaPoints[POINT3_INDEX].Y);
                }
            }
            return pathGeometry;
        }

        private String formatException(Exception ex)
        {
            return String.Format("Exception:{0} \n Stack:{1}", ex.Message, ex.ToString());
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _container = _container.FromString(eTextBlock.Text);
                saveFiles();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        private void saveFiles()
        {
            string fileName = String.Format("{0}.{1}.png", _container.Name, DateTime.Now.Ticks);
            saveImage(fileName);
            saveXmlFile(String.Format("{0}.xml", fileName));
        }

        private void saveXmlFile(String fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(eTextBlock.Text);
            }
        }

        private void saveXmlFileCurrentState()
        {
            string fileName = String.Format("{0}.{1}.png", _container.Name, DateTime.Now.Ticks);
            saveXmlFileCurrentState(String.Format("{0}.xml", fileName));
        }

        private void saveXmlFileCurrentState(String fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(_container.ToString());
            }
        }

        private String saveImage(String fileName)
        {
            if (_rtb != null)
            {
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(_rtb));

                using (System.IO.Stream s = System.IO.File.Create(fileName))
                {
                    png.Save(s);
                }
            }
            return fileName;
        }

        private ImageSource tileImage(DrawingVisual drawingVisual, double scale, out int width, out int height, bool saveToDisk, string name, CropTransform cropBeforeRotating, CropTransform cropAfterRotating, Model.RotateTransform rotate)
        {
            // We want to break the image into tiles
            // So I need to loop horitzontally and then vertically, saving the images as I go
            // I can come up with a naming convention so they can be put back together again, maybe using the positions of the tiles
            // Then I can reassemble the image, hopefully saving on memory versus calling Render() on the whole thing
            bool willCrop = false;
            if (cropBeforeRotating != null)
            {
                willCrop = (cropBeforeRotating.Height > 0 && cropBeforeRotating.Width > 0 && cropBeforeRotating.Apply);
            }

            String tileDirectory = System.Configuration.ConfigurationSettings.AppSettings["TileDirectory"];
            String outputDirectory = System.Configuration.ConfigurationSettings.AppSettings["OutputDirectory"];
            String fileName = String.Empty;
            String ticks = DateTime.Now.Ticks.ToString();
            if (!Directory.Exists(tileDirectory))
            {
                Directory.CreateDirectory(tileDirectory);
            }

            const String EXTENSION = "png";
            const int DEFAULT_H = 1000;
            const int DEFAULT_W = 1000;
            int remainderW = 0;
            int remainderH = 0;
            int offsetW = 0;
            int offsetH = 0;
            int sizeW = 0;
            int sizeH = 0;
            int posW = 0;
            int posH = 0;

            // If we are going to crop let's do it here
            if (willCrop)
            {
                offsetW = (int)(cropBeforeRotating.X * scale);
                offsetH = (int)(cropBeforeRotating.Y * scale);
                width = offsetW + (int)(cropBeforeRotating.Width * scale);
                height = offsetH + (int)(cropBeforeRotating.Height * scale);
            }
            else
            {
                if (_container.Transforms.CropBeforeRendering != null)
                {
                    if (_container.Transforms.CropBeforeRendering.Apply)
                    {
                        width = (int)(_container.Transforms.CropBeforeRendering.Width * scale);
                        height = (int)(_container.Transforms.CropBeforeRendering.Height * scale);
                    }
                    else
                    {
                        width = (int)(drawingVisual.ContentBounds.Width * scale);
                        height = (int)(drawingVisual.ContentBounds.Height * scale);
                    }
                }
                else
                {
                    width = (int)(drawingVisual.ContentBounds.Width * scale);
                    height = (int)(drawingVisual.ContentBounds.Height * scale);
                }
            }

            // test the remaining width and height
            // if >= the default amount, then leave at default
            // if < the default amount, set H or W to the remainder

            while (offsetH < height)
            {
                remainderH = height - offsetH;
                sizeH = (remainderH < DEFAULT_H ? remainderH : DEFAULT_H);

                while (offsetW < width)
                {
                    remainderW = width - offsetW;
                    sizeW = (remainderW < DEFAULT_W ? remainderW : DEFAULT_W);

                    drawingVisual.Offset = new Vector(offsetW * -1, offsetH * -1);
                    _rtb = new RenderTargetBitmap(sizeW, sizeH, 96, 96, PixelFormats.Pbgra32);
                    _rtb.Render(drawingVisual);
                    fileName = String.Format("{0}\\{1}.{2}.{3}.{4}", tileDirectory, posH.ToString(), posW.ToString(), ticks, EXTENSION);
                    offsetW += DEFAULT_W;
                    posW++;
                    saveImage(fileName);
                }
                
                if (willCrop)
                {
                    offsetW = (int)(cropBeforeRotating.X * scale);
                }
                else
                {
                    offsetW = 0;
                }
                remainderW = 0;
                posW = 0;

                offsetH += DEFAULT_H;
                posH++;
            }

            if (willCrop)
            {
                height = (int)(cropBeforeRotating.Height * scale);
                width = (int)(cropBeforeRotating.Width * scale);
            }

            return assembleTiles(tileDirectory, DEFAULT_H, DEFAULT_W, ticks, EXTENSION, out width, out height, name, width, height, scale, cropAfterRotating, rotate, outputDirectory);
        }

        private ImageSource assembleTiles(String tileDirectory, int tileW, int tileH, string instanceMarker, string extension, out int width, out int height, string name, int imageWidth, int imageHeight, double scale, CropTransform cropAfterRotating, Model.RotateTransform rotate, String outputDirectory)
        {
            // Now that we have our tiles on the hard drive, piece them back together to display to the user

            // WPF assemblage is too memory-intensive (as far as I can tell). Shell out to GDI+
            TileHelper.Concatenater helper = new TileHelper.Concatenater();
            string fileName = helper.ConcatenateTiles(imageHeight, imageWidth, tileH, tileW, tileDirectory, instanceMarker, extension, name, scale, cropAfterRotating, rotate, outputDirectory);
            // Now that the files are concatenated, save off the matching xml
            string xmlFileName = String.Format("{0}.{1}.{2}.xml", _container.Name, instanceMarker, extension);
            saveXmlFile(xmlFileName);
            // I think I need to save off a small version to display in the UI

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                PngBitmapDecoder d = new PngBitmapDecoder(new Uri(fileName), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                ctx.DrawImage(d.Frames[0], new Rect(0, 0, (int)d.Frames[0].Width, (int)d.Frames[0].Height));
            }
            // Scale it back down for UI
            double displayScale = (_container.DisplayScale <= 0 ? 0.1 : _container.DisplayScale);
            width = (int)(dv.ContentBounds.Width * displayScale);
            height = (int)(dv.ContentBounds.Height * displayScale);
            ScaleTransform scaleTrans = new ScaleTransform(displayScale, displayScale);
            dv.Transform = scaleTrans;
            _rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            _rtb.Render(dv);

            // Now that we're done with the tiles, clean up
            if (System.Configuration.ConfigurationSettings.AppSettings["deleteTiles"].ToLower() == "true")
            {
                helper.deleteTiles(tileDirectory, instanceMarker, extension);
            }

            return (ImageSource)_rtb;
        }

        private void assembleTilesToFile()
        {
            throw new NotImplementedException();
        }

        private void menuXmlVisibility_Click(object sender, RoutedEventArgs e)
        {
            textboxXml.Visibility = (textboxXml.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        private void EditableTextBlock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            eTextBlock.IsInEditMode = true;
        }

        private void menuLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_container == null) _container = new DeltaContainer();
                // Configure open file dialog box
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "Xml files (.xml)|*.xml"; // Filter files by extension
                if (!Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["OutputDirectory"]))
                {
                    Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["OutputDirectory"]);
                }
                dlg.InitialDirectory = System.Configuration.ConfigurationSettings.AppSettings["OutputDirectory"];

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string xml = sr.ReadToEnd();
                        _container = _container.FromString(xml);
                    }
                }
                populateXmlText();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        private void menuDraw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _container = _container.FromString(eTextBlock.Text);
                drawImage();
                //TODO: share objects between threads
                //startWork();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        private void populateXmlText()
        {
            eTextBlock.Text = (" " + (string)_container.ToString().Clone()).Trim(); // Work around for WPF bug
        }

        private void handleException(Exception ex)
        {
            textGeneral.Text = formatException(ex);
        }

    }
}
