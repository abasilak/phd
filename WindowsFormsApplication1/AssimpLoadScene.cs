using System;
//using Assimp;
//using Assimp.Configs;
using System.IO;
using System.Reflection;

namespace abasilak
{
    class AssimpLoadScene
    {
        static void Main(string[] args)
        {
            //Filepath to our model
            String fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Seymour.dae");

            //Create a new importer
            ///AssimpImporter importer = new AssimpImporter();

            //This is how we add a configuration (each config is its own class)
            //NormalSmoothingAngleConfig config = new NormalSmoothingAngleConfig(66.0f);
            //importer.SetConfig(config);

            //This is how we add a logging callback 
            //LogStream logstream = new LogStream(delegate(String msg, String userData)
            //{
              //  Console.WriteLine(msg);
            //});
            //importer.AttachLogStream(logstream);

            //Import the model - this is considered a single atomic call. All configs are set, all logstreams attached. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            //Assimp.Scene model = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);

            //Load the model data into your own structures

            //End of example
            //importer.Dispose();
        }
    }
}
