namespace VS.Mvc.Components {

    using System.Collections.Generic;
    using VS.Abstractions;

    public class FormActionsModel {

        public FormActionsModel(IEnumerable<ContextualLink> actions) {
            Actions = actions;
        }


        public IEnumerable<ContextualLink> Actions { get; private set; }

    }
}