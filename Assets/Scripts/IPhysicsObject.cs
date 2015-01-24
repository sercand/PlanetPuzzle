// #
// # Created by Sercan Degirmenci on 2015.01.24
// #
namespace Assets.Scripts
{
    public enum PhysicsObjectType
    {
        Piece,
        Planet
    }

    public interface IPhysicsObject
    {
        PhysicsObjectType Type { get; }
        bool IsDragging { get; set; }
        bool IsGrabbed { get;}
    }
}