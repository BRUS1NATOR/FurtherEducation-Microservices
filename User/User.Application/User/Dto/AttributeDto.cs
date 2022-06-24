using System.Collections.Generic;

namespace User.Domain.Dto
{
    public class AttributeListDto
    {
        public List<AttributeDto> attributes { get; set; }
        public AttributeListDto (List<AttributeDto> attributes)
        {
            this.attributes = attributes;
        }
    }
    public class AttributeDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
    }
}
