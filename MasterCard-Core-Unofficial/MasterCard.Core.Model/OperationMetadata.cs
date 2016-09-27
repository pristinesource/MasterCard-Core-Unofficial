using System;
using System.Runtime.CompilerServices;

namespace MasterCard.Core.Model {
    public class OperationMetadata {
        public string Host { get; private set; }

        public string Version { get; private set; }

        public OperationMetadata(string version, string host) {
            this.Version = version;
            this.Host = host;
        }
    }
}
