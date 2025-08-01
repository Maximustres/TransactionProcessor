using AutoMapper;

namespace api_transaction.test.Mapper
{
    public class MappingProfileTests
    {
        [Fact]
        public void MappingProfile_Should_Have_Valid_Configuration()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
