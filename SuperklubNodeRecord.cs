using System.Numerics;

namespace Superklub
{
    /// <summary>
    /// A minimal implementation of ISuperklubNode
    /// to store data received by server
    /// </summary>
    public class SuperklubNodeRecord : ISuperklubNode
    {
        public string Id { get; set; } = "";

        private Vector3 position = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;

        public (float x, float y, float z) Position
        {
            get { return (position.X, position.Y, position.Z); }
            set { position = new Vector3(value.x, value.y, value.z); }
        }
        public (float w, float x, float y, float z) Rotation
        {
            get { return (rotation.X, rotation.Y, rotation.Z, rotation.W); }
            set { rotation = new Quaternion(value.w, value.x, value.y, value.z); }
        }
        public string Shape { get; set; } = "";
        public string Color { get; set; } = "";
    }
}
