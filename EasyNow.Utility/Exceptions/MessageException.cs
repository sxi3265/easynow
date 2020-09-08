using System;
using System.Runtime.Serialization;

namespace EasyNow.Utility.Exceptions
{
    /// <summary>
    /// 用于传递消息的异常，区别于Exception
    /// </summary>
    [Serializable]
    public class MessageException : Exception
    {
        /// <summary>
        /// 异常所携带的数据
        /// </summary>
        public new object Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="MessageException"/> object.
        /// </summary>
        public MessageException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="MessageException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MessageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="MessageException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MessageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}