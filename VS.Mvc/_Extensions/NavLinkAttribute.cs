namespace VS.Mvc._Extensions {

    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class NavLinkAttribute : Attribute {

        public NavLinkAttribute(string menuGroup, string linkTitle, int position) {
            MenuGroup = menuGroup ?? throw new ArgumentNullException(nameof(menuGroup));
            LinkTitle = linkTitle ?? throw new ArgumentNullException(nameof(linkTitle));
            Position = position;
        }

        public string MenuGroup { get; }
        public string LinkTitle { get; }
        public int Position { get; }

    }
}
