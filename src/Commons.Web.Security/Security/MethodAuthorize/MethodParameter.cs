using System;

namespace Commons.Web.Security.MethodAuthorize
{
    public class MethodParameter
    {
        private string _parameterName;
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        private Type _parameterType;
        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        private object _parameterValue;
        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public object ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="parameterType">The type of the parameter.</param>
        /// <param name="parameterValue">The value of the parameter.</param>
        public MethodParameter(string parameterName, Type parameterType, object parameterValue)
        {
            _parameterName = parameterName;
            _parameterType = parameterType;
            _parameterValue = parameterValue;
        }

        /// <summary>
        /// Determines whether the specified object is identical to the current object.
        /// </summary>
        /// <param name="obj">The object to be compared with the current object.</param>
        /// <returns>true if the specified object and the current object are the same, otherwise false.</returns>
        // ReSharper disable once CSharpWarnings::CS0659
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            MethodParameter other = (MethodParameter)obj;

            if (!Equals(ParameterName, other.ParameterName))
            {
                return false;
            }
            if (ParameterType != other.ParameterType)
            {
                return false;
            }
            if (!Equals(ParameterValue, other.ParameterValue))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(_parameterName, _parameterType, _parameterValue, ParameterName, ParameterType, ParameterValue);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("Name: {0}; Type: {1}, Value: {2}", ParameterName, ParameterType, ParameterValue);
        }
    }
}