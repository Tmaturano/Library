using Library.Application.DTOs;
using System.Collections.Generic;

namespace Library.API.Helpers
{
    /// <summary>
    /// This class is to wrap the links to a given object.
    /// It's needed for when returning a collection, the collection will have a Link for itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedCollectionResourceWrapperDto<T> : LinkedResourceBaseDto where T : LinkedResourceBaseDto
    {
        public IEnumerable<T> Values { get; private set; }

        public LinkedCollectionResourceWrapperDto(IEnumerable<T> values)
        {
            Values = values;
        }
    }
}
