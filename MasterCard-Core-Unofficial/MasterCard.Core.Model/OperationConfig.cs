using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MasterCard.Core.Model {
    public class OperationConfig {
        public string ResourcePath { get; private set; }

        public string Action { get; private set; }

        public List<string> QueryParams { get; private set; }

        public List<string> HeaderParams { get; private set; }

        public OperationConfig(string resourcePath, string action, List<string> queryParams, List<string> headerParams) {
            this.ResourcePath = resourcePath;
            this.Action = action;
            this.QueryParams = queryParams;
            this.HeaderParams = headerParams;
        }
    }
}
