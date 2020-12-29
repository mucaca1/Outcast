using System.Collections.Generic;

namespace Outcast.Stats {
    public interface IModifierProvider {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);

        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}