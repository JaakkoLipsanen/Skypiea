using Flai;
using Flai.Graphics;
using Flai.Graphics.Particles;
using Flai.Graphics.Particles.EmitterStyles;
using Flai.Graphics.Particles.Modifiers;
using Flai.Graphics.Particles.Renderers;
using Flai.Graphics.Particles.TriggerHandlers;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.View
{

    public class ParticleEffectRenderer : FlaiRenderer
    {
        private readonly ParticleEngine _particleEngine;
        private readonly SpriteBatchParticleRenderer _particleRenderer = new SpriteBatchParticleRenderer() { IsSpriteBatchAlreadyRunning = true };

        public ParticleEffectRenderer(ParticleEngine particleEngine)
        {
            _particleEngine = particleEngine;
        }

        protected override void LoadContentInner()
        {
            this.CreateParticleEffects();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _particleRenderer.RenderAllEffects(graphicsContext, _particleEngine);
        }

        #region Create Particle Effects

        private void CreateParticleEffects()
        {
            #region Zombie Explosion 

            _particleEngine.Add(ParticleEffectID.ZombieExplosion, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(500, 1f, new PointEmitter(true))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(30, 40),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.DarkRed,
                            Opacity = 0.5f,
                            Scale = 10,
                            Speed = new Range(0, 125),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new VelocityDampingModifier() { DampingPower =  2f },
                            new OpacityFadeModifier() { InitialOpacity = 0.75f },
                            new ColorInterpolatorModifier() { InitialColor = Color.DarkRed, FinalColor = Color.Black },
                            new ScaleTriInterpolatorModifier() { InitialScale =  0, MedianScale = 10, Median = 0.333f, FinalScale = 20 },
                            new RotationModifier() { RotationRate = 4f }
                        },
                    },
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                }
            });

            // "grunt birthday"
            //_particleEngine.Add(ParticleEffectID.ZombieExplosion, new ParticleEffect
            //{
            //    Emitters = new ParticleEmitterCollection
            //    {
            //        new ParticleEmitter(300, 1f, new PointEmitter(true))
            //        {
            //            ReleaseParameters = new ReleaseParameters
            //            {
            //                Quantity = new RangeInt(30, 40),
            //                Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
            //                Color = new ColorRange(new Range<byte>(0, 255), new Range<byte>(0, 255), new Range<byte>(0, 255)),
            //                Scale = 10,
            //                Speed = new Range(20, 150),
            //            },

            //            Modifiers = new ParticleModifierCollection
            //            {
            //                new VelocityDampingModifier() { DampingPower =  2f },
            //                new OpacityFadeModifier() { InitialOpacity = 1f },
            //               // new ColorInterpolatorModifier() { InitialColor = Color.DarkRed, FinalColor = Color.Black },
            //               new HueShiftModifier() { HueShiftAmount = 720 },
            //              //  new ScaleTriInterpolatorModifier() { InitialScale =  0, MedianScale = 10, Median = 0.333f, FinalScale = 20 },
            //                new RotationFromVelocityModifier(),
            //            },
            //        },
            //    },

            //    TriggerHandlers = new ParticleTriggerHandlerCollection()
            //    {
            //        new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
            //    }
            //});

            #endregion

            #region Rocket Launcher Smoke

            _particleEngine.Add(ParticleEffectID.RocketSmoke, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(4000, 2f, new CircleEmitter(10, false, false))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Speed = new Range(12, 24),
                            Quantity = new RangeInt(1, 3),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new OpacityFadeModifier() { InitialOpacity = 0.75f },
                            new ColorTriInterpolatorModifier() { InitialColor = Color.Yellow, MedianColor = Color.DimGray, Median = 0.025f, FinalColor = Color.Black },
                            new RotationModifier() { RotationRate = FlaiMath.PiOver2 },
                            new ScaleInterpolatorModifier { InitialScale = 7, FinalScale = 14 },
                        }
                    }
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                }
            });

            #endregion

            #region Rocket Launcher Explosion

            _particleEngine.Add(ParticleEffectID.RocketExplosion, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(4000, 2f, new CircleEmitter(20, false, false))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(30, 50),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.DarkRed,
                            Opacity = 0.5f,
                            Scale = new Range(20, 30),
                            Speed = new Range(10, 150),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new OpacityFadeModifier() { InitialOpacity = 0.75f },
                            new ColorInterpolatorModifier() { InitialColor = Color.Yellow, FinalColor = Color.Black },
                            new RotationModifier() { RotationRate = FlaiMath.PiOver2 },
                            new ScaleInterpolatorModifier { InitialScale = 7, FinalScale = 14 },
                            new VelocityDampingModifier() { DampingPower = 1.5f },
                        }
                    }
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                }
            });

            #endregion
        }

        #endregion

        private bool ShouldParticleEffectTrigger(ref TriggerContext triggerContext)
        {
            return SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active).Contains(triggerContext.Position);
        }
    }

}
