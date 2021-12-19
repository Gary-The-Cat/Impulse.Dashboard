// <copyright file="Scene.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Impulse.Mono.Viewer
{
    public class Scene
    {
        private readonly List<Model> models;

        public Scene()
        {
            models = new List<Model>();
        }

        public IEnumerable<Model> Models => models.AsEnumerable();

        public void AddModel(Model model)
        {
            models.Add(model);
        }
    }
}
