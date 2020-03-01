using System.Collections.Generic;

namespace InteligenciaArtificial2 {
	public class StateConfigurer<T> {
		private State<T> _instance;
		private Dictionary<T, Transition<T>> _transitions = new Dictionary<T, Transition<T>>();

		public StateConfigurer(State<T> state) {
			_instance = state;
		}

		public StateConfigurer<T> SetTransitions(T input, State<T> target) {
			_transitions.Add(input, new Transition<T>(input, target));

			return this;
		}

		public void Configure() {
			_instance.Configure(_transitions);
		}
	}

	public static class StateConfigurer {
		public static StateConfigurer<T> Create<T>(State<T> state) {
			return new StateConfigurer<T>(state);
		}
	}
}
