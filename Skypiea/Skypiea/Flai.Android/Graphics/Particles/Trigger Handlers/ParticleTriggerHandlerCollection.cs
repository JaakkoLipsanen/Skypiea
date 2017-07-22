using System.Collections.Generic;

namespace Flai.Graphics.Particles.TriggerHandlers
{
    public class ParticleTriggerHandlerCollection : List<TriggerHandler>
    {
        internal void Process(ref TriggerContext triggerContext)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Process(ref triggerContext);
            }
        }
    }
}
