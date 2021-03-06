using System.Reflection;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics;
using Flai.Graphics.Particles;
using Flai.Graphics.Particles.EmitterStyles;
using Flai.Graphics.Particles.Modifiers;
using Flai.Graphics.Particles.Renderers;
using Flai.Graphics.Particles.TriggerHandlers;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.View
{
    public static class ParticleEffectHelper
    {
        public static readonly Range ZombieExplosionDefaultSpeed = new Range(4, 150);
    }

    public class ParticleEffectRenderer : FlaiRenderer
    {
        private readonly EntityWorld _entityWorld;
        private readonly ParticleEngine _particleEngine;
        private readonly SpriteBatchParticleRenderer _particleRenderer = new SpriteBatchParticleRenderer { IsSpriteBatchAlreadyRunning = true };

        public ParticleEffectRenderer(EntityWorld entityWorld, ParticleEngine particleEngine)
        {
            _entityWorld = entityWorld;
            _particleEngine = particleEngine;
        }

        protected override void LoadContentInner()
        {
            this.CreateParticleEffects();
        }

        // name...
        public void DrawEffectsUnderObjects(GraphicsContext graphicsContext)
        {
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.GoldenGoblinPath]);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.ZombieExplosion]);
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.ZombieBloodSplatter]);
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.RocketSmoke]);
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.RocketExplosion]);
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.GoldenGoblinSpray]);
            _particleRenderer.RenderEffect(graphicsContext, _particleEngine[ParticleEffectID.GoldenGoblinExplosion]);
        }

        #region Create Particle Effects

        private void CreateParticleEffects()
        {
            #region Zombie Explosion

            this.CreateZombieExplosion();

            #endregion

            #region Zombie Blood Splatter

            _particleEngine.Add(ParticleEffectID.ZombieBloodSplatter, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(300, 0.5f, new PointEmitter(true))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(10, 20),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.DarkRed,
                            Opacity = 0.5f,
                            Scale = 7,
                            Speed = new Range(0, 75),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new VelocityDampingModifier() { DampingPower =  2f },
                            new OpacityFadeModifier() { InitialOpacity = 0.5f },
                            new ColorInterpolatorModifier() { InitialColor = Color.DarkRed, FinalColor = Color.Black },
                            new ScaleTriInterpolatorModifier() { InitialScale =  0, MedianScale = 7, Median = 0.333f, FinalScale = 14 },
                            new RotationModifier() { RotationRate = 3f }
                        },
                    },
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                }
            });


            #endregion

            #region Rocket Launcher Smoke

            _particleEngine.Add(ParticleEffectID.RocketSmoke, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(4000, 1f, new CircleEmitter(10, false, false))
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

            #region Golden Goblin Path

            _particleEngine.Add(ParticleEffectID.GoldenGoblinPath, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(250, 1f, new CircleEmitter(8, false, false))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(1, 2),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.Gold,
                            Opacity = 0.5f,
                            Scale = new Range(6, 12),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new OpacityTriInterpolatorModifier() { InitialOpacity = 0.2f, MedianOpacity = 0.15f, FinalOpacity = 0f, Median = 0.6f },
                          //  new ScaleInterpolatorModifier { InitialScale = 7, FinalScale = 14 },
                           //  new VelocityDampingModifier() { DampingPower = 1.5f },
                        }
                    }
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler((ref TriggerContext context) => SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active).Inflate(128).Contains(context.Position))
                }
            });

            #endregion

            #region Golden Goblin Spray

            _particleEngine.Add(ParticleEffectID.GoldenGoblinSpray, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(400, 2f, new CircleEmitter(24, false, false))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(6, 12),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.Yellow,
                            Opacity = 0.5f,
                            Scale = new Range(16, 32),
                            Speed = new Range(5, 40),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new OpacityTriInterpolatorModifier() { InitialOpacity = 0.0f, MedianOpacity = 0.3f, FinalOpacity = 0f, Median = 0.05f },
                          //  new ScaleInterpolatorModifier { InitialScale = 7, FinalScale = 14 },
                           //  new VelocityDampingModifier() { DampingPower = 1.5f },
                        },

                        Texture = _contentProvider.DefaultManager.LoadTexture("Sparkle"),
                    }
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler((ref TriggerContext context) => SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active).Inflate(128).Contains(context.Position))
                }
            });


            #endregion

            #region Golden Goblin Explosion

            _particleEngine.Add(ParticleEffectID.GoldenGoblinExplosion, new ParticleEffect
            {
                Emitters = new ParticleEmitterCollection
                {
                    new ParticleEmitter(200, 2f, new CircleEmitter(24, false, false))
                    {
                        ReleaseParameters = new ReleaseParameters
                        {
                            Quantity = new RangeInt(5, 100),
                            Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                            Color = Color.Yellow,
                            Opacity = 0.5f,
                            Scale = new Range(24, 48),
                            Speed = new Range(30, 160),
                        },

                        Modifiers = new ParticleModifierCollection
                        {
                            new OpacityTriInterpolatorModifier() { InitialOpacity = 0.0f, MedianOpacity = 0.3f, FinalOpacity = 0f, Median = 0.05f },
                            new VelocityDampingModifier() { DampingPower = 2f }
                          //  new ScaleInterpolatorModifier { InitialScale = 7, FinalScale = 14 },
                           //  new VelocityDampingModifier() { DampingPower = 1.5f },
                        },

                        Texture = _contentProvider.DefaultManager.LoadTexture("Sparkle"),
                    }
                },

                TriggerHandlers = new ParticleTriggerHandlerCollection()
                {
                    new CullerTriggerHandler((ref TriggerContext context) => SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active).Inflate(128).Contains(context.Position))
                }
            });


            #endregion
        }

        private void CreateZombieExplosion()
        {
            // UUGGLLYYY.. this could be done more wisely but whatever... basicly, first version is normal blood explosion and the second one is the "zombie birthday party" explosion
            IPlayerPassiveStats passiveStats = _entityWorld.Services.Get<IPlayerPassiveStats>();
            if (!passiveStats.ZombieBirthdayParty)
            {
                #region Blood

                _particleEngine.Add(ParticleEffectID.ZombieExplosion, new ParticleEffect
                {
                    Emitters = new ParticleEmitterCollection
                    {
                        new ParticleEmitter(1000, 1, new RotationalPointEmitter(true, Range.FullRotation))
                        {
                            ReleaseParameters = new ReleaseParameters
                            {
                                Quantity = new RangeInt(30, 40),
                                Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                                Color = Color.DarkRed,
                                Opacity = 0.5f,
                                Scale = 10,
                                Speed = ParticleEffectHelper.ZombieExplosionDefaultSpeed,
                            },

                            Modifiers = new ParticleModifierCollection
                            {
                                new VelocityDampingModifier() {DampingPower = 1.75f},
                                new OpacityFadeModifier() {InitialOpacity = 0.75f},
                                new ColorInterpolatorModifier() {InitialColor = Color.DarkRed, FinalColor = Color.Black},
                                new ScaleTriInterpolatorModifier() {InitialScale = 0, MedianScale = 10, Median = 0.333f, FinalScale = 18},
                            },
                        },

                        new ParticleEmitter(500, 1, new PointEmitter(true))
                        {
                            ReleaseParameters = new ReleaseParameters
                            {
                                Quantity = new RangeInt(10, 20),
                                Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                                Color = Color.DarkRed,
                                Opacity = 0.5f,
                                Scale = 10,
                                Speed = new Range(10, ParticleEffectHelper.ZombieExplosionDefaultSpeed.Max/2f)
                            },

                            Modifiers = new ParticleModifierCollection
                            {
                                new VelocityDampingModifier() {DampingPower = 1.75f},
                                new OpacityFadeModifier() {InitialOpacity = 0.4f},
                                new ColorInterpolatorModifier() {InitialColor = Color.DarkRed, FinalColor = Color.Black},
                                new ScaleTriInterpolatorModifier() {InitialScale = 0, MedianScale = 10, Median = 0.333f, FinalScale = 20},
                            },
                        },
                    },

                    TriggerHandlers = new ParticleTriggerHandlerCollection()
                    {
                        new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                    }
                });

                #endregion
            }
            else
            {
                #region Zombie Birthday Party

                _particleEngine.Add(ParticleEffectID.ZombieExplosion, new ParticleEffect
                {
                    Emitters = new ParticleEmitterCollection
                    {
                        new ParticleEmitter(1000, 1, new RotationalPointEmitter(true, Range.FullRotation))
                        {
                            ReleaseParameters = new ReleaseParameters
                            {
                                Quantity = new RangeInt(30, 40),
                                Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                                Color = ColorRange.FromHSV(new Range(0, 360), 1, new Range(0.5f, 1)), // new ColorRange(new Range<byte>(64, 224), new Range<byte>(64, 224), new Range<byte>(64, 224)),
                                Opacity = 0.5f,
                                Scale = 10,
                                Speed = ParticleEffectHelper.ZombieExplosionDefaultSpeed,
                            },

                            Modifiers = new ParticleModifierCollection
                            {
                                new VelocityDampingModifier() {DampingPower = 1.75f},
                                new OpacityFadeModifier() {InitialOpacity = 0.75f},
                                new HueShiftModifier() { HueShiftAmount = 360f },
                                new ScaleTriInterpolatorModifier() {InitialScale = 0, MedianScale = 10, Median = 0.333f, FinalScale = 18},
                            },
                        },

                        new ParticleEmitter(500, 1, new PointEmitter(true))
                        {
                            ReleaseParameters = new ReleaseParameters
                            {
                                Quantity = new RangeInt(10, 20),
                                Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi),
                                Color = ColorRange.FromHSV(new Range(0, 360), 1, new Range(0.5f, 1)), // new ColorRange(new Range<byte>(64, 255), new Range<byte>(64, 255), new Range<byte>(64, 255)),
                                Opacity = 0.5f,
                                Scale = 10,
                                Speed = new Range(10, ParticleEffectHelper.ZombieExplosionDefaultSpeed.Max/2f)
                            },

                            Modifiers = new ParticleModifierCollection
                            {
                                new VelocityDampingModifier() {DampingPower = 1.75f},
                                new OpacityFadeModifier() {InitialOpacity = 0.4f},
                                new HueShiftModifier() { HueShiftAmount = 360f },
                                new ScaleTriInterpolatorModifier() {InitialScale = 0, MedianScale = 10, Median = 0.333f, FinalScale = 20},
                            },
                        },
                    },

                    TriggerHandlers = new ParticleTriggerHandlerCollection()
                    {
                        new CullerTriggerHandler(this.ShouldParticleEffectTrigger)
                    }
                });

                #endregion
            }
        }

        #endregion

        private bool ShouldParticleEffectTrigger(ref TriggerContext triggerContext)
        {
            return SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active).Contains(triggerContext.Position);
        }
    }
}
