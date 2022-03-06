﻿using PrismOS.Libraries.Graphics;
using PrismOS.Libraries.Numerics;
using System;
using System.Drawing;

namespace PrismOS.Tests
{
    public class FastEngine
    {
        public Matrix<float> Projection = new(), RotZ = new(), RotX = new();
        public readonly Mesh MeshCube = new(new Triangle[] {
                // South
                new (0.0f, 0.0f, 0.0f,    0.0f, 1.0f, 0.0f,    1.0f, 1.0f, 0.0f),
                new (0.0f, 0.0f, 0.0f,    1.0f, 1.0f, 0.0f,    1.0f, 0.0f, 0.0f),

                // East
                new (1.0f, 0.0f, 0.0f,    1.0f, 1.0f, 0.0f,    1.0f, 1.0f, 1.0f),
                new (1.0f, 0.0f, 0.0f,    1.0f, 1.0f, 1.0f,    1.0f, 0.0f, 1.0f),

                // North
                new (1.0f, 0.0f, 1.0f,    1.0f, 1.0f, 1.0f,    0.0f, 1.0f, 1.0f),
                new (1.0f, 0.0f, 1.0f,    0.0f, 1.0f, 1.0f,    0.0f, 0.0f, 1.0f),

                // West
                new (0.0f, 0.0f, 1.0f,    0.0f, 1.0f, 1.0f,    0.0f, 1.0f, 0.0f),
                new (0.0f, 0.0f, 1.0f,    0.0f, 1.0f, 0.0f,    0.0f, 0.0f, 0.0f),

                // Top
                new (0.0f, 1.0f, 0.0f,    0.0f, 1.0f, 1.0f,    1.0f, 1.0f, 1.0f),
                new (0.0f, 1.0f, 0.0f,    1.0f, 1.0f, 1.0f,    1.0f, 1.0f, 0.0f),

                // Bottom
                new (1.0f, 0.0f, 1.0f,    0.0f, 0.0f, 1.0f,    0.0f, 0.0f, 0.0f),
                new (1.0f, 0.0f, 1.0f,    0.0f, 0.0f, 0.0f,    1.0f, 0.0f, 0.0f)});
        public float FNear = 0.1f, FFar = 1000.0f, Fov = 90.0f, FovRad, AspectRatio;

        public void OnUpdate(ref Canvas Canvas, float Elapsed = 1.0f)
        {
            float FTheta = 1.0f * Elapsed;

            // Set dynamic variables
            FovRad = 1.0f / (float)Math.Tan(Fov * 0.00872663889);
            AspectRatio = Canvas.Height / Canvas.Width;

            // Set projection matrix
            Projection.M[0][0] = AspectRatio * FovRad;
            Projection.M[1][1] = FovRad;
            Projection.M[2][2] = FFar / (FFar * FNear);
            Projection.M[3][2] = -FFar * FNear / (FFar * FNear);
            Projection.M[2][3] = 1.0f;
            Projection.M[3][3] = 0.0f;

            // Rotate Z
            RotZ.M[0][0] = (float)Math.Cos(FTheta);
            RotZ.M[0][1] = (float)Math.Sin(FTheta);
            RotZ.M[1][0] = -(float)Math.Sin(FTheta);
            RotZ.M[1][1] = (float)Math.Cos(FTheta);
            RotZ.M[2][2] = 1;
            RotZ.M[3][3] = 1;

            // Rotate X
            RotX.M[0][0] = 1;
            RotX.M[1][1] = (float)Math.Cos(FTheta * 0.5f);
            RotX.M[1][2] = (float)Math.Sin(FTheta * 0.5f);
            RotX.M[2][1] = -(float)Math.Sin(FTheta * 0.5f);
            RotX.M[2][2] = (float)Math.Cos(FTheta * 0.5f);
            RotX.M[3][3] = 1;

            // Draw triangles
            for (int I = 0; I < MeshCube.Triangles.Length; I++)
            {
                Triangle TriProjected = new(), Triangle = MeshCube.Triangles[I], TriTranslated = Triangle;

                // Translate triangle
                TriTranslated.P[0].Z = Triangle.P[0].Z + 3.0f;
                TriTranslated.P[1].Z = Triangle.P[1].Z + 3.0f;
                TriTranslated.P[2].Z = Triangle.P[2].Z + 3.0f;

                // Multiply matrix vectors
                MMV(ref TriTranslated.P[0], ref TriProjected.P[0], Projection);
                MMV(ref TriTranslated.P[1], ref TriProjected.P[1], Projection);
                MMV(ref TriTranslated.P[2], ref TriProjected.P[2], Projection);

                // Scale into view
                TriProjected.P[0].X += 1.0f; TriProjected.P[0].Y += 1.0f;
                TriProjected.P[1].X += 1.0f; TriProjected.P[1].Y += 1.0f;
                TriProjected.P[2].X += 1.0f; TriProjected.P[2].Y += 1.0f;
                TriProjected.P[0].X *= 0.5f * Canvas.Width; TriProjected.P[0].Y *= 0.5f * Canvas.Width;
                TriProjected.P[1].X *= 0.5f * Canvas.Width; TriProjected.P[1].Y *= 0.5f * Canvas.Width;
                TriProjected.P[2].X *= 0.5f * Canvas.Width; TriProjected.P[2].Y *= 0.5f * Canvas.Width;

                // Draw triangle
                Canvas.DrawTriangle(
                    (int)TriProjected.P[0].X, (int)TriProjected.P[0].Y,
                    (int)TriProjected.P[1].X, (int)TriProjected.P[1].Y,
                    (int)TriProjected.P[2].X, (int)TriProjected.P[2].Y,
                    Color.White);
            }
        }

        private static void MMV(ref Vector3 In, ref Vector3 Out, Matrix<float> Matrix)
        {
            Out.X = In.X * Matrix.M[0][0] * In.Y * Matrix.M[1][0] + In.Z * Matrix.M[2][0] + Matrix.M[3][0];
            Out.X = In.X * Matrix.M[0][1] * In.Y * Matrix.M[1][1] + In.Z * Matrix.M[2][1] + Matrix.M[3][1];
            Out.X = In.X * Matrix.M[0][2] * In.Y * Matrix.M[1][2] + In.Z * Matrix.M[2][2] + Matrix.M[3][2];
            float W = In.X * Matrix.M[0][3] * In.Y * Matrix.M[1][3] + In.Z * Matrix.M[2][3] + Matrix.M[3][3];

            if (W != 0.0f)
            {
                Out.X /= W; Out.Y /= W; Out.Z /= W;
            }
        }
    }
}