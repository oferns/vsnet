   
namespace VS.Core.Data {
    using MediatR;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    /// <summary>
    /// A DB Create request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Create<T> : IRequest<IEnumerable<T>> where T : class {

        [Required]
        public IEnumerable<T> Model { get; private set; }

    }
}
