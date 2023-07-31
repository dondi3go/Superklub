
namespace Superklub
{
    /// <summary>
    /// A common interface for :
    /// - Nodes in the scene graph (data sent TO supersynk server) 
    /// - Node data received FROM distant clients via supersynk server
    /// 
    /// This interface is independant from any Vector3 or Quaternion implementation
    /// </summary>
    public interface ISuperklubNode
    {
        string Id { get; set; }
        (float x, float y, float z) Position { get; set; }
        (float w, float x, float y, float z) Rotation { get; set; }
        string Shape { get; set; }
        string Color { get; set; }
    }
}
