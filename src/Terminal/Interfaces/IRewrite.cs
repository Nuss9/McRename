using System.Collections.Generic;

namespace Terminal
{
    public interface IRewrite
    {
        public void Rewrite(Dictionary<string, string> proposal);
    }
}
