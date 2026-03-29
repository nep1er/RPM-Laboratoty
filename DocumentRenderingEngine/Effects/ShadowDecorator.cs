using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine
{
    public class ShadowDecorator : DrawableDecorator
    {
        private int _offsetX, _offsetY;
        private string _shadowColor;

        public ShadowDecorator(IDrawable wrappee, int offsetX, int offsetY, string shadowColor = "gray")
            : base(wrappee)
        {
            _offsetX = offsetX; _offsetY = offsetY;
            _shadowColor = shadowColor;
        }

        public override void Draw()
        {
            Console.Write($"[Shadow:{_shadowColor}+({_offsetX},{_offsetY})] ");
            base.Draw();
        }
    }
}
