using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Common
{
    /// <summary>
    /// A class to be inherited by all core output models.
    /// Any common properties between all core output models will be contained here.
    /// </summary>
    public abstract class CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the result.
        /// This is used to indicate the core layer's success or failure in retrieving data,
        /// along with a message containing relevant information.
        /// </summary>
        public MethodResult<string> Result { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// This is used to indicate the error code occurred while retrieving data.       
        /// </summary>
        public string ErrorCode { get; set; }

    }
}
