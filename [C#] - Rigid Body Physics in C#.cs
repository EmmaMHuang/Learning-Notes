using System;
using Microsoft.Xna.Framework;

namespace Phys {
  public class RigidBody {
    public Vector3 Position;
    public Quaternion Orientation = Quaternion.Identity;
    public Vector3 LinearVelocity;
    public Vector3 AngularVelocity;
    public Vector3 Force;
    public Vector3 Torque;
    public float Mass = 1;
 
    public void Integrate(float dt) {
      LinearVelocity += Force / Mass * dt;
      Force = Vector3.Zero;
      AngularVelocity += Torque / Mass * dt;
      Torque = Vector3.Zero;
      Position += LinearVelocity * dt;
      Orientation += new Quaternion((AngularVelocity * dt), 0) * Orientation;
      Orientation.Normalize();
    }

    public void ApplyForce(Vector3 forcePosition, Vector3 directionMagnitude) {
      float lengthSquared = directionMagnitude.LengthSquared();
      if (lengthSquared < 1e-8f) return; // or use your own favorite epsilon
      Force += directionMagnitude;
      Vector3 distance = forcePosition - Position;
      Torque += Vector3.Cross(directionMagnitude, distance);
    }

    public Vector3 PointVelocity(Vector3 worldPoint) {
      Vector3 distance = worldPoint - Position;
      Vector3 vel = Vector3.Cross(AngularVelocity, distance) + LinearVelocity;
      return vel;
    }

    public override string ToString() {
      return String.Format("Pos: {0} LVel: {1} Ori: {2} AVel: {3}", Position, LinearVelocity, Orientation, AngularVelocity);
    }
  }

  public static class P {

    public static void Main(string[] str) {
      RigidBody rb = new RigidBody();
      Console.WriteLine("{0}", rb);
      rb.Integrate(0.017f);
      Console.WriteLine("{0}", rb);
      rb.ApplyForce(new Vector3(-1, 0, -1), new Vector3(0, 4, 0));
      Console.WriteLine("{0}", rb);
      rb.Integrate(0.017f);
      Console.WriteLine("{0}", rb);
    }
  }
}
