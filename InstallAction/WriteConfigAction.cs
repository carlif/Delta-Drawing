using System.IO;
using System.ComponentModel;
using System.Configuration.Install;

namespace InstallAction
{
    [RunInstaller(true)]
    public partial class WriteConfigAction : Installer
    {
        public WriteConfigAction()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string targetDirectory = Context.Parameters["DP_TargetDirectory"].ToString();
            stateSaver.Add("targetDirectory", targetDirectory);

            string tileDirectory = Context.Parameters["DP_TileDirectory"].ToString();
            stateSaver.Add("TileDirectory", tileDirectory);

            string outputDirectory = Context.Parameters["DP_OutputDirectory"].ToString();
            stateSaver.Add("OutputDirectory", outputDirectory);

            string configuration = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<configuration>\n  <appSettings>\n    <add key=\"TileDirectory\" value=\"{0}\"/>\n    <add key=\"OutputDirectory\" value=\"{1}\"/>\n    <add key=\"DeleteTiles\" value=\"true\"/>\n  </appSettings>\n</configuration>", tileDirectory, outputDirectory);

            // Save the config file using the Tile Directory and Output Directory
            StreamWriter sw = new StreamWriter(targetDirectory + "DeltaDrawing.UI.exe.config", false);

            // Move the Sample Drawing to the output folder
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            string sampleDrawingFileName = "SampleDrawing.xml";
            File.Move(targetDirectory + sampleDrawingFileName, outputDirectory + "\\" + sampleDrawingFileName);

            sw.Write(configuration);
            sw.Flush();
            sw.Close(); 
        }
    }
}
