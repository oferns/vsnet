
namespace VS.Data.PostGres.App {
    public class TenantCategory {

        public int Parent { get; set; }

        public int Id { get; set; }

        public int Level { get; set; }

        public decimal Weight { get; set; }

        public string Label { get; set; }

        public string ParentLabel { get; set; }

    }
}
