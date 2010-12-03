************
Overview
************

Delta Drawing is an image generation program for the Windows platform. To install the application, use the file Install\InstallDeltaDrawing.msi.

When running the application, select the Load option, browse to the Setup\SampleDrawing.xml file and click Open. Then click Draw on the Delta Drawing menu to draw the image.

The SampleDrawing.xml file can be edited in Notepad or any basic text editor. Different xml files can be created to make images.


************
Detail
************
About the objects represented in the SampleDrawing.xml:
A DeltaPath is a line, defined by a set of points. Point0 and Point3 are the start and end of the line.
Point1 controls the amount of curvature coming from Point0.
Point2 controls the amount of curvature coming from Point3. 

The DeltaPaths will be drawn the number of times defined by the startIteration and endIteration. 
Each time the path is drawn, the points in the line change by the amounts in deltaX and deltaY.
X is the horizontal axis, Y is the vertical axis.
Also, the deltaX and deltaY values also change by the amounts in deltaXdelta and deltaYdelta.

The color of the line is defined in the DeltaColor. Note that this element also has delta values.

Each time the DeltaPath is drawn, the program keeps track of where the line is and updates it for the next iteration.

In this way, the line appears to move across the drawing.


************
About
************

Delta Drawing was created by Jayce Renner. http://jaycerenner.com
