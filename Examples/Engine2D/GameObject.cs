using System;
using System.Collections.Generic;
using System.Text;
using SharpDX;

namespace Examples.Engine2D
{
    public abstract class GameObject
    {

        public abstract void Update(float elapsedTime);
        public abstract void Render(float elapsedTime, Drawer drawer);
    }
}
