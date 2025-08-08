namespace Softoverse.CqrsKit.Attributes
{
    public class GroupAttribute: Attribute
    {
        public static readonly GroupAttribute Default = new GroupAttribute();

        public GroupAttribute() : this(string.Empty)
        {
        }

        public GroupAttribute(string name = null)
        {
            NameValue = name;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        public string Name => NameValue;

        /// <summary>
        /// Read/Write property that directly modifies the string stored in the description
        /// attribute. The default implementation of the <see cref="Name"/> property
        /// simply returns this value.
        /// </summary>
        protected string NameValue { get; set; }
    }
}