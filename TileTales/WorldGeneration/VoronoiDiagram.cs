using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileTales.WorldGeneration {
    internal class VoronoiDiagram {
        private int sideLength;
        private int numSeeds;
        private List<TectonicPlate> plates;
        private Voronoi voronoi;

        public VoronoiDiagram(int sideLength, int numSeeds) {
            this.sideLength = sideLength;
            this.numSeeds = numSeeds;
            this.plates = new List<TectonicPlate>();
            GenerateInitialVoronoiDiagram();
        }

        private void GenerateInitialVoronoiDiagram() {
            Random random = new Random();
            List<Vector2> seeds = new List<Vector2>();

            for (int i = 0; i < numSeeds; i++) {
                seeds.Add(new Vector2(random.Next(sideLength), random.Next(sideLength)));
            }

            voronoi = Voronoi.FromPoints(seeds);
            int id = 0;

            foreach (var region in voronoi.Regions) {
                TectonicPlate plate = new TectonicPlate(id, region);
                plates.Add(plate);
                id++;
            }
        }

        public void UpdatePlates(float timeStep) {
            foreach (TectonicPlate plate in plates) {
                plate.Move(timeStep);
            }

            UpdateVoronoiDiagram();
        }

        private void UpdateVoronoiDiagram() {
            List<Vector2> seedPoints = new List<Vector2>();

            foreach (TectonicPlate plate in plates) {
                seedPoints.Add(plate.CenterOfMass);
            }

            voronoi = Voronoi.FromPoints(seedPoints);
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (TectonicPlate plate in plates) {
                plate.Draw(spriteBatch);
            }
        }

    }
}
