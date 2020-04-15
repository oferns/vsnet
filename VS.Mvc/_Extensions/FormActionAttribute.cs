namespace VS.Mvc._Extensions {

     using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FormActionAttribute : Attribute {
        public FormActionAttribute(string form, string buttonTitle, int position) {
            Form = form;
            ButtonTitle = buttonTitle;
            Position = position;
        }

        public string Form { get; }
        public string ButtonTitle { get; }
        public int Position { get; }
    }

}
