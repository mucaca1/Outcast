using UnityEngine;

namespace Outcast.Core {
    public interface IPredicateEvaluator {
        bool? Evaluate(string predicate, string[] parameters);
    }
}