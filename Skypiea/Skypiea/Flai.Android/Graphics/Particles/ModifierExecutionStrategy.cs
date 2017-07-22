using Flai.Graphics.Particles.Modifiers;

// OOKAY.. atm even for WP version Parallel is Serial because the Parallel library is not in .NET3.5 (the version WP has) and i cannot be bothered to do anything else...
// the Parallel library is actually ported for .NET3.5, but meh... : http://www.nuget.org/packages/System.Threading.Tasks

using ParallelType = 
#if WINDOWS_PHONE
 Flai.Graphics.Particles.ModifierExecutionStrategy.SerialModifierExecutionStrategy;
#else
Flai.Graphics.Particles.ModifierExecutionStrategy.ParallelModifierExecutionStrategy;
#endif

namespace Flai.Graphics.Particles
{
    public abstract class ModifierExecutionStrategy
    {
        public static ModifierExecutionStrategy Serial = new SerialModifierExecutionStrategy();
        public static ModifierExecutionStrategy Parallel = new ParallelType();

        internal abstract void ExecuteModifiers(UpdateContext updateContext, ParticleModifierCollection modifiers, ref ParticleBuffer.Iterator iterator);

        // hmm?
        internal ModifierExecutionStrategy()
        {          
        }

        internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
        {
            internal override void ExecuteModifiers(UpdateContext updateContext, ParticleModifierCollection modifiers, ref ParticleBuffer.Iterator iterator)
            {
                foreach (ParticleModifier modifier in modifiers)
                {
                    modifier.Process(updateContext, ref iterator);
                    iterator.Reset();
                }
            }
        }

#if !WINDOWS_PHONE
        internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
        {
            internal override void ExecuteModifiers(UpdateContext updateContext, ParticleModifierCollection modifiers, ref ParticleBuffer.Iterator iterator)
            {
                ParticleBuffer.Iterator iter = iterator; // make sure that the iterator is always copied, so that all modifiers don't concurrently process the same iterator
                System.Threading.Tasks.Parallel.ForEach(modifiers, modifier =>
                {
                    ParticleBuffer.Iterator iterInner = iter; // make sure that the iterator is always copied, so that all modifiers don't concurrently process the same iterator
                    modifier.Process(updateContext, ref iterInner);
                });
            }
        }
#endif
    }
}
