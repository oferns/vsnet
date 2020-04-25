using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace VS.Mvc.Components {
    internal class DynamicViewData {
        private Func<ViewDataDictionary> p;

        public DynamicViewData(Func<ViewDataDictionary> p) {
            this.p = p;
        }
    }
}