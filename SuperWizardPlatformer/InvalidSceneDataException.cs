using System;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// The exception that is thrown when invalid data is read from a description of an IScene.
    /// </summary>
    /// <seealso cref="IScene"/>
    class InvalidSceneDataException : ArgumentException
    {
        private const string DEFAULT_MESSAGE = "Invalid scene data.";

        public InvalidSceneDataException() : base(DEFAULT_MESSAGE) { }

        public InvalidSceneDataException(string message) : base(message) { }

        public InvalidSceneDataException(string message, string paramName) 
            : base(message, paramName) { }

        public InvalidSceneDataException(string message, Exception innerException) 
            : base(message, innerException) { }

        public InvalidSceneDataException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }
    }
}
