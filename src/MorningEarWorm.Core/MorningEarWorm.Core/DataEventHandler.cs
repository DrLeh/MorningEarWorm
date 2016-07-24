using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    /// <summary>
    /// Allows easier transfer of data through events.
    /// </summary>
    /// <typeparam name="T">Type of data being sent in args</typeparam>
    /// <param name="sender">sender of the event</param>
    /// <param name="e">args containing the datatype T</param>
    public delegate void DataEventHandler<T>(object sender, DataEventArgs<T> e);

    /// <summary>
    /// Event args that hold a data type for easier access.
    /// </summary>
    public class DataEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public DataEventArgs(T obj)
        {
            Data = obj;
        }

        public static implicit operator T(DataEventArgs<T> args)
        {
            return args.Data;
        }
        public static implicit operator DataEventArgs<T>(T t)
        {
            return new DataEventArgs<T>(t);
        }
        //public static implicit operator DataEventArgs<T>(T t) => DataEventArgs<T>(t);
        //public static implicit operator T(DataEventArgs<T> args) => args.Data;
    }
}
