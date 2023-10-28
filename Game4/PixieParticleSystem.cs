using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    public class PixieParticleSystem : ParticleSystem
    {
        IParticleEmitter _emitter;

        public PixieParticleSystem(Game game, IParticleEmitter emitter) : base(game, 2000)
        {
            _emitter = emitter;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "circle";

            minNumParticles = 2;
            maxNumParticles = 5;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var velocity = _emitter.Velocity;

            var acceleration = Vector2.UnitY * 400;

            var scale = RandomHelper.NextFloat(0.1f, 0.5f);

            var lifetime = RandomHelper.NextFloat(0.1f, 0.75f);

            p.Initialize(where, velocity, acceleration, Color.Goldenrod, scale : scale, lifetime : lifetime);
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Changes needed for emittion to be at right spot

            Vector2 position = _emitter.Position - new Vector2(40,24);

            Vector2 p = new Vector2(_emitter.Position.X + 24, _emitter.Position.Y + 38);

            AddParticles(p);
        }
    }
}
