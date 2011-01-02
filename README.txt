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
The DeltaDrawing program takes in a input file and outputs an image. The input file is a specifically formatted XML file. 
The same input file will result in the same output image every time the program runs.
DeltaDrawing produces images by drawing lines that change by amounts or values set in the input file.

The file SampleDrawing.xml contains sample input information for the DeltaDrawing program. 
The values between double quotes can be modified to effect the output of the program.

The following text describes the elements of this input file and how the elements are used by the program to create an image.

At the top of the file is the DeltaContainer element. It has settings that affect the entire image output. 
scale can be set to multiply the size of the output. This changes the vectors of the points in the entire image without changing the quality of the image.
displayScale sets how big the program should display the resulting image. If the image produced is very large, set the displayScale to a decimal smaller than 1.
blurRadius can be used to soften the lines of the entire image.

Within the DeltaContainer are the elements Comments and DeltaFigures. Comments can be modified without changing the image at all. 
The comments section is useful for describing the output for future reference by a human.

The DeltaFigures element contains one or more DeltaFigure elements.
A DeltaFigure defines a line that changes each iteration.
A DeltaFigure contains a set of line segments called DeltaSegments. 
A DeltaSegment defines the points of a Bezier curve and says how much the points should change per iteration.

Each time the DeltaFigure is drawn, the program keeps track of the line's position and shape. 
In each iteration, the program updates the line's potition and shape for the next iteration.
In this way, the line appears to move across the drawing while changing shape, thickness, and color.

The DeltaFigure has a StartPoint where the line starts.
The first DeltaPoint of the first DeltaSegment controls the amount of curvature coming from the StartPoint.
The second DeltaPoint in the DeltaSegment controls the amount of curvature coming from the third DeltaPoint.
The third DeltaPoint defines the end point of the segment.
There can be any number of segments in a figure.
In any segments after the first one, the first point controls the amount of curvature coming from the third point in the previous segment.

The program draws DeltaFigures the number of times defined by the startIteration and endIteration values. 
If the startIteration is 1 and the endIteration is 11, the program will draw the line 10 times. The program does not draw on the endIteration.
Each time the figure is drawn, the points in the line change by the amounts in deltaX and deltaY.
X controls the point's position along the horizontal axis, and Y effects the vertical axis.
Also, the deltaX and deltaY values also change each iteration by the amounts in deltaXdelta and deltaYdelta.
The skipIterationsCSV can be empty, or it can contain comma separated values (CSV) of iterations to skip.
If skipIterationsCSV has "5,6" then the program will update all of the delta values during the iterations 5 and 6, but it will not draw the line those times.

The color of the line is defined in the DeltaColor. Note that this element also has delta values, meaning they change per iteration.
The Alpha element in the DeltaColor controls the opacity of the color.
The thickness of the line is also a delta value.

General points:
If an element's delta values are set to 0, that element won't move or change as the program iterates.
The name attributes are arbitrary labels that can be set by people to document an element's purpose.


************
About
************

Delta Drawing was created by Jayce Renner. http://jaycerenner.com
