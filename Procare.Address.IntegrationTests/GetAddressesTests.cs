//-----------------------------------------------------------------------
// <copyright file="GetAddressesTests.cs" company="Procare Software, LLC">
//     Copyright © 2021-2022Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class GetAddressesTests
    {
        private readonly AddressService service = new AddressService(new Uri("https://addresses.dev-procarepay.com"));

        [Fact]
        public async Task GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress()
        {
            var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "OR" }).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.NotNull(result.Addresses);
            Assert.Equal(result.Count, result.Addresses!.Count);
            
        }

        [Fact]
        public async Task GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses()
        {
           var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 Main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(false);
                        
            //Validating Response Fields
            Assert.NotNull(result);
            Assert.True(result.Count>1);
            Assert.NotNull(result.Addresses);
            Assert.Equal(result.Count,result.Addresses!.Count);
            foreach(var temp in result.Addresses)
            {
                Assert.Equal(temp.City, "Ontario".ToUpper());
                Assert.Equal("CA", temp.StateCode);
            }
        }
        [Fact]
        public async Task GetAddresses_Without_ValidAddress_ShouldResultIn_ThrowingError()
        {
            var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "FA" }).ConfigureAwait(false);
            
            //Validating Response Fields 
            
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Addresses);
            Assert.Equal(result.Count, result.Addresses!.Count);

        }
    }
}
