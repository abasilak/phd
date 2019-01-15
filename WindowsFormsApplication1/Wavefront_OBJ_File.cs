using System;
using System.Windows.Forms;
using OpenTK;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace abasilak
{
    public class WaveFront_OBJ_File
    {
        static public int _step = 1;

        public WaveFront_OBJ_File()
        {
        }
        public static List<List<Mesh3D>> OpenOBJFile(ProgressBar pBar, ToolStripStatusLabel tLabel, bool X_Rotation, bool ignoreGroups)
        {
            List<List<Mesh3D>> result = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wavefront .obj|*.obj|SMF .smf|*.smf|TXT .txt|*.txt";
            ofd.InitialDirectory = @Properties.Settings.Default.PoseSequencePath;
            ofd.Multiselect = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();

            if (ofd.FileNames.Length > 0)
                result = new List<List<Mesh3D>>();
            else
                return null;

            string[] FileNames = new string[(int)(ofd.FileNames.Length / _step)];
            for (int i = 0; i < FileNames.Length; i++)
                FileNames[i] = ofd.FileNames[i*_step];

            Application.UseWaitCursor = true;
            pBar.Value = 0;
            pBar.Maximum = FileNames.Length;

            for (int i = 0; i < FileNames.Length; i++)
            {
                if (ofd.FileNames[i].Trim() == "")
                    return result;
                switch (ofd.FilterIndex)
                {
                    case 1:
                    case 2:
                        {
                            result.Add(LoadOBJFile(FileNames[i].Trim(), X_Rotation, ignoreGroups, ofd.FilterIndex)); 
                            pBar.Increment(1);
                            tLabel.Text = FileNames[i].Trim() + " loaded!!";
                            Application.DoEvents();
                            break;
                        }
                    default:
                        break;
                }
            }
            Application.UseWaitCursor = false;
            return result;
        }
        /// <summary>
        /// This function opens a stream and reads the .obj file. 
        /// !!So far It Assumes that the object described in the file 
        /// is segmented into parts and that the file contains object part names i.e. vertex lists start with "o mesh_part_name"
        /// </summary>
        /// <param name="FileName"></param>
        public static List<Mesh3D> LoadOBJFile(string FilePath, bool X_Rotation, bool ignoreGroups, int filterIndex)
        {
            List<Mesh3D> results = new List<Mesh3D> ();

            int indexVertex  = 0;
            int indexNormal  = 0;
            int indexTexture = 0;
            int indexFacet   = 0;

            int FacetCount = 0;
            List<string> GroupName = new List<string>();
            string FileName = Path.GetFileName(FilePath.Replace((filterIndex == 1) ? ".obj" : ".smf", "."));

            List<List<Vector3  >> VertexList = new List<List<Vector3>>();
            List<List<Vector3  >> NormalList = new List<List<Vector3>>();
            List<List<Vector3  >> ColorList  = new List<List<Vector3>>();
            List<List<Vector2  >> TextureList= new List<List<Vector2>>();
            List<List<uint     >> FacetList  = new List<List<uint>>();
            List<List<List<int>>> NeighborsVertices = new List<List<List<int>>>();
            List<List<List<int>>> NeighborsFacets = new List<List<List<int>>>();
            

            VertexList.Add (new List<Vector3>());
            NormalList.Add (new List<Vector3>());
            ColorList.Add(new List<Vector3>());
            TextureList.Add(new List<Vector2>());
            FacetList.Add  (new List<uint>());
            NeighborsVertices.Add(new List<List<int>>());
            NeighborsFacets.Add(new List<List<int>>());

            using (StreamReader sreader = new StreamReader(FilePath, Encoding.Default))
            {
                try
                {
                    //While we are not yet at the end of stream                        
                    while (!sreader.EndOfStream)
                    {
                        string Line = sreader.ReadLine();
                        string[] currentLine = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (currentLine.Length == 0) continue;

                        switch (currentLine[0])
                        {
                            case "mtllib":
                            {
                                string mtlFile = Path.GetDirectoryName(FilePath) + '\\' + currentLine[1];
                                LoadMTLFile(mtlFile);
                                break;
                            }
                            case "v":
                                {
                                    try
                                    {
                                        Vector3d TransformedV;
                                        Vector3d nV = new Vector3d(Convert.ToDouble(currentLine[1]), Convert.ToDouble(currentLine[2]), Convert.ToDouble(currentLine[3]));

                                        Vector3d ColorV = new Vector3d();
                                        if (currentLine.Length > 4)
                                            ColorV = new Vector3d(Convert.ToDouble(currentLine[4]), Convert.ToDouble(currentLine[5]), Convert.ToDouble(currentLine[6]));

                                        if (X_Rotation)
                                        {
                                            Matrix4d R = Matrix4d.CreateRotationX(-Math.PI / 2.0);
                                            Vector3d.Transform(ref nV, ref R, out TransformedV);
                                        }
                                        else
                                            TransformedV = nV;

                                        VertexList[indexVertex].Add(new Vector3((float)TransformedV.X, (float)TransformedV.Y, (float)TransformedV.Z)); //X,Y,Z
                                        ColorList[indexVertex].Add(new Vector3((float)ColorV.X, (float)ColorV.Y, (float)ColorV.Z)); //X,Y,Z

                                        NeighborsVertices[indexVertex].Add(new List<int>());
                                        NeighborsFacets[indexVertex].Add(new List<int>());
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Vertex Storing:" + ex.Message);
                                    }
                                    break;
                                }
                            case "vt":
                                {
                                    TextureList[indexTexture].Add(new Vector2(Convert.ToSingle(currentLine[1]), Convert.ToSingle(currentLine[2])));
                                    break;
                                }
                            case "vn":
                                {
                                    NormalList[indexNormal].Add(new Vector3((float)Convert.ToDouble(currentLine[1]), (float)Convert.ToDouble(currentLine[2]), (float)Convert.ToDouble(currentLine[3])));
                                    break;
                                }
                            case "vp":
                                {
                                    break;
                                }
                            case "f":
                                {
                                    uint VertexCount = 0;
                                    if (!ignoreGroups)
                                        for (int i = 0; i < indexFacet; ++i)
                                            VertexCount += (uint)VertexList[i].Count;

                                    uint[] vertices = new uint[3];
                                    for (int i=1; i < currentLine.Length; ++i)
                                    {
                                        string[] vData = currentLine[i].Split('/');
                                        //In obj files vertex numbering in a facet declaration statement is 1-based.
                                        //So we subtruct 1 from each reference because we want 0-based
                                        vertices[i - 1] = Convert.ToUInt32(vData[0]) - 1 - VertexCount;
                                        FacetList[indexFacet].Add(vertices[i - 1]);
                                    }                                   

                                    // Neighborhood...
                                    for(int i=0; i<3; ++i)
                                    {
                                        NeighborsFacets[indexFacet][(int)vertices[i]].Add(FacetCount);

                                        if (!NeighborsVertices[indexFacet][(int)vertices[i]].Contains((int)vertices[(i + 1) % 3]))
                                            NeighborsVertices[indexFacet][(int)vertices[i]].Add((int)vertices[(i + 1) % 3]);
                                        if (!NeighborsVertices[indexFacet][(int)vertices[i]].Contains((int)vertices[(i + 2) % 3]))
                                            NeighborsVertices[indexFacet][(int)vertices[i]].Add((int)vertices[(i + 2) % 3]);
                                    }
                                    
                                    FacetCount++;
                                    break;
                                }
                            case "g":
                                {
                                    if (ignoreGroups || currentLine.Length == 1)
                                        break;

                                    if (ColorList[indexVertex].Count > 0)
                                    {
                                        ColorList.Add(new List<Vector3>());
                                    }
                                    if (VertexList[indexVertex].Count   > 0) { indexVertex++; VertexList.Add(new List<Vector3>()); NeighborsVertices.Add(new List<List<int>>()); NeighborsFacets.Add(new List<List<int>>());}
                                    if (NormalList[indexNormal].Count   > 0) { indexNormal++; NormalList.Add(new List<Vector3>()); }
                                    if (TextureList[indexTexture].Count > 0) { indexTexture++; TextureList.Add(new List<Vector2>()); }
                                    if (FacetList[indexFacet].Count     > 0) { indexFacet++; FacetList.Add(new List<uint>()); FacetCount = 0; }

                                    GroupName.Add(FileName + currentLine[1].ToString());
                                    break;
                                }
                                
                            case "o": break;
                            case "#": break;
                        }//switch
                    }
                }
                catch (Exception ex)
                {
                    results = null;
                    MessageBox.Show(ex.ToString());
                }
            }

            results = new List<Mesh3D>();

            if (GroupName.Count == 0)
            {
                GroupName.Add(FileName.Substring(0,FileName.Length-1));
            }

            for (int i = 0; i < GroupName.Count; i++)
            {
                Vector3[] normals = (i < NormalList.Count) ? NormalList[i].ToArray() : null;
                Vector3[] colors  = (i < ColorList.Count) ? ColorList[i].ToArray() : null;
                Vector2[] tex_indices = (i < TextureList.Count) ? TextureList[i].ToArray() : null;

                uint[] indices = FacetList[i].ToArray();

                Mesh3D mesh = new Mesh3D(
                    i,
                    GroupName[i],
                    VertexList[i].ToArray(),
                    normals,
                    colors,
                    tex_indices,
                    indices,
                    NeighborsVertices[i].ToArray(),
                    NeighborsFacets[i].ToArray()
                );
                results.Add(mesh);
            }
            return results;
        }
        public static void LoadMTLFile(string FilePath)
        {
            return;
        }
    }
}