namespace abasilak
{
    using MIConvexHull;

    /// <summary>
    /// Represents a point in 3D space.
    /// </summary>
    public class Vertex : IVertex
    {
        public Vertex(double x, double y, double z)
        {
            Position = new double[] { x, y, z };
        }

        public double[] Position { get; set; }
    }    
        
    
    /// <summary>
    /// A face is a simple class that stores the...
    /// </summary>
    public class Face : ConvexFace<Vertex, Face>
    {

    }
}