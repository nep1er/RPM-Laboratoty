using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine
{
    public class BorderDecorator : DrawableDecorator
    {
        private int _borderWidth;
        private string _borderColor;

        public BorderDecorator(IDrawable wrappee, int borderWidth, string borderColor = "black")
            : base(wrappee)
        {
            _borderWidth = borderWidth;
            _borderColor = borderColor;
        }

        public override void Draw()
        {
            Console.Write($"[Border:{_borderColor}/{_borderWidth}px] ");
            base.Draw();
        }
    }
}
