using System;
using System.IO;
using System.Text;

namespace Swan.Ldap
{
    internal static class Extensions
    {
        /// <summary>
        /// Reads a number of characters from the current source Stream and writes the data to the target array at the
        /// specified index.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="target">The target.</param>
        /// <param name="start">The start.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// sourceStream
        /// or
        /// target.
        /// </exception>
        public static int ReadInput(this Stream sourceStream, ref sbyte[] target, int start, int count)
        {
            if (sourceStream == null)
                throw new ArgumentNullException(nameof(sourceStream));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            // Returns 0 bytes if not enough space in target
            if (target.Length == 0)
                return 0;

            var receiver = new byte[target.Length];
            var bytesRead = 0;
            var startIndex = start;
            var bytesToRead = count;
            while (bytesToRead > 0)
            {
                var n = sourceStream.Read(receiver, startIndex, bytesToRead);
                if (n == 0)
                    break;
                bytesRead += n;
                startIndex += n;
                bytesToRead -= n;
            }

            // Returns -1 if EOF
            if (bytesRead == 0)
                return -1;

            for (var i = start; i < start + bytesRead; i++)
                target[i] = (sbyte)receiver[i];

            return bytesRead;
        }

        /// <summary>
        /// Converts an array of sbytes to an array of bytes.
        /// </summary>
        /// <param name="sbyteArray">The sbyte array.</param>
        /// <returns>
        /// The byte array from conversion.
        /// </returns>
        /// <exception cref="ArgumentNullException">sbyteArray.</exception>
        public static byte[] ToByteArray(this sbyte[] sbyteArray)
        {
            if (sbyteArray == null)
                throw new ArgumentNullException(nameof(sbyteArray));

            var byteArray = new byte[sbyteArray.Length];
            for (var index = 0; index < sbyteArray.Length; index++)
                byteArray[index] = (byte)sbyteArray[index];

            return byteArray;
        }

        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>
        /// The sbyte array from conversion.
        /// </returns>
        /// <exception cref="ArgumentNullException">byteArray.</exception>
        public static sbyte[] ToSByteArray(this byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException(nameof(byteArray));

            var sbyteArray = new sbyte[byteArray.Length];
            for (var index = 0; index < byteArray.Length; index++)
                sbyteArray[index] = (sbyte)byteArray[index];
            return sbyteArray;
        }

        /// <summary>
        /// Gets the sbytes from a string.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="s">The s.</param>
        /// <returns>The sbyte array from string.</returns>
        public static sbyte[] GetSBytes(this Encoding encoding, string s)
            => encoding.GetBytes(s).ToSByteArray();

        /// <summary>
        /// Gets the string from a sbyte array.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="data">The data.</param>
        /// <returns>The string.</returns>
        public static string GetString(this Encoding encoding, sbyte[] data)
            => encoding.GetString(data.ToByteArray());
    }
}
