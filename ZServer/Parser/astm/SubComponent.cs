namespace ZServer.Parser.astm
{
    public class SubComponent : MessageElement
    {
        public SubComponent(string val, ASTMEncoding encoding)
        {
            this.Encoding = encoding;
            this.Value = val;
        }

        protected override void ProcessValue()
        {
            
        }
    }
}
