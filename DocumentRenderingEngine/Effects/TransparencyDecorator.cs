using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentRenderingEngine.Engine.Core.Interfaces;

namespace DocumentRenderingEngine.Engine.Effects
{
    public class TransparencyDecorator : DrawableDecorator
    {
        private float _alpha;

        public TransparencyDecorator(IDrawable wrappee, float alpha) : base(wrappee)
        {
            _alpha = Math.Clamp(alpha, 0.0f, 1.0f);
        }

        public override void Draw()
        {
            Console.Write($"[Transparency:{_alpha:P0}] ");
            base.Draw();
        }
    }
}
